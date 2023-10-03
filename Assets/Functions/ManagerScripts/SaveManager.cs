using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using System; 

// ゲームデータのセーブ管理
public class SaveManager : MonoBehaviour
{
    // 他スクリプトでも呼べるようにインスタンス化
    public static SaveManager InstanceSaveManager;

    private QuickSaveReader _reader;
    private QuickSaveWriter _writer;

    // MaterialBunker
    private const string PHOTO_SPRITE_KEY = "_Sprite";
    private const string PHOTO_NAME_KEY = "_Name";
    // PixmateManager
    private const string PIXMATE_TEXTURE_KEY = "_Texture";
    // WorldManager
    private const string WORLDOBJ_POS_KEY = "_Pos";
    private const string WORLDOBJ_ROT_KEY = "_Rot";
    private const string WORLDOBJ_SHAPE_KEY = "_Shape";
    private const string WORLDOBJ_MAT_KEY = "_Mat";

    void Awake()
    {
        if (InstanceSaveManager == null)
        {
            InstanceSaveManager= this;
        }

    }

    void Start()
    {
        // データの保存先をApplication.persistentDataPathに変更
        QuickSaveGlobalSettings.StorageLocation = Application.persistentDataPath;

        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // 暗号化の方法
        settings.SecurityMode = SecurityMode.Aes;
        // 暗号化キー
        settings.Password = "Password";
        // 圧縮の方法
        settings.CompressionMode = CompressionMode.Gzip;

        _writer = QuickSaveWriter.Create("Player", settings);
        _reader = QuickSaveReader.Create("Player", settings);
        
    }

    // マテリアルのセーブ処理
    public void DoSaveMaterialCount(int matCount)
    {
        // データを書き込む
        _writer.Write("matCount", matCount);
        // 変更を反映
        _writer.Commit();
    }

    public void DoSaveMaterial(Sprite savaSprite, string savaName, string key)
    {
        // Spriteとその名前のセーブ
        string savePhotoSpriteKey = key + PHOTO_SPRITE_KEY;
        _writer.Write(savePhotoSpriteKey, savaSprite);

        string savePhotoNamesKey = key + PHOTO_NAME_KEY;
        _writer.Write(savePhotoNamesKey, savaName);

        // 変更を反映
        _writer.Commit();
    }

    // Spriteのセーブ処理
    public void DoSaveSprite(int matCount, Sprite sprite,string name,string key)
    {
        // データを書き込む
        _writer.Write("matCount", matCount);
        _writer.Write(key + PHOTO_SPRITE_KEY, sprite);
        _writer.Write(key + PHOTO_NAME_KEY, name);

        // 変更を反映
        _writer.Commit();
    }

    // Pixmateのセーブ処理
    public void DoSavePixmates(int count, Sprite sprite,string key)
    {
        // データを書き込む
        _writer.Write("pixmateCount", count);
        _writer.Write(key + PIXMATE_TEXTURE_KEY, sprite);
        
        // 変更を反映
        _writer.Commit();
    }

    // Worldのセーブ処理

    // Worldのオブジェクト数をセーブ

    public void DoSaveWorldInitialization(bool initial)
    {
        // データを書き込む
        _writer.Write("worldInitialization", initial);

        // 変更を反映
        _writer.Commit();
    }
    
    public void DoSaveWorldSize(int count)
    {
        // データを書き込む
        _writer.Write("worldObjCount", count);

        // 変更を反映
        _writer.Commit();
    }

    // Object情報をセーブ
    public void DoSaveWorld(string key, Vector3 pos, Quaternion rot, string shape, int mat)
    {
        // データを書き込む
        _writer.Write(key + WORLDOBJ_POS_KEY, pos);
        _writer.Write(key + WORLDOBJ_ROT_KEY, rot);
        _writer.Write(key + WORLDOBJ_SHAPE_KEY, shape);
        _writer.Write(key + WORLDOBJ_MAT_KEY, mat);

        // 変更を反映
        _writer.Commit();
    }

    // データの読み込み

    // MaterialBunker
    public int LoadCountMat()
    {
        try
        {
            return _reader.Read<int>("matCount");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("データの読み込みエラー: " + ex.Message);

            return 0; 
        }
    }

    public Sprite LoadMaterialSprite(string key)
    {
        string loadTextureKey = key + PHOTO_SPRITE_KEY;
        try
        {
            return _reader.Read<Sprite>(loadTextureKey);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("データの読み込みエラー: " + ex.Message);

            return null; 
        }
    }

    public string LoadMaterialSpriteName(string key)
    {
        string loadPhotoNamesKey = key + PHOTO_NAME_KEY;
        try
        {
            return _reader.Read<string>(loadPhotoNamesKey);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("データの読み込みエラー: " + ex.Message);

            return null; 
        }
    }

    // PixmatesManager
    public int LoadPixmateCount()
    {
        // デバッグ用
        //return 0;
        try
        {
            return _reader.Read<int>("pixmateCount");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("データの読み込みエラー: " + ex.Message);

            return 0; 
        }
    }

    public Sprite LoadPixmateSprite(string key)
    {
        string loadTextureKey = key + PIXMATE_TEXTURE_KEY;
        try
        {
            return _reader.Read<Sprite>(loadTextureKey);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("データの読み込みエラー: " + ex.Message);

            return null; 
        }
    }

    // WorldManager

    public bool LoadWorldInitialization()
    {        
        // ワールドのセーブデータがなかった場合はtrueを返す。
        try
        {
            return _reader.Read<bool>("worldInitialization");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("データの読み込みエラー: " + ex.Message);

            return true; 
        }
    }

    public int LoadWorldSize()
    {
        try
        {
            return _reader.Read<int>("worldObjCount");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("データの読み込みエラー: " + ex.Message);

            return 0; 
        }
    }

    public Vector3 LoadWorldObjPos(string key)
    {
        return _reader.Read<Vector3>(key + WORLDOBJ_POS_KEY);
    }

    public Quaternion LoadWorldObjRot(string key)
    {
        return _reader.Read<Quaternion>(key + WORLDOBJ_ROT_KEY);
    }

    public string LoadWorldObjShape(string key)
    {
        return _reader.Read<string>(key + WORLDOBJ_SHAPE_KEY);
    }

    public int LoadWorldObjMat(string key)
    {
        return _reader.Read<int>(key + WORLDOBJ_MAT_KEY);
    }

    //データ削除
    public void DeleteDate()
    {
        // 初期データ
        _writer.Write("matCount", 0);
        MaterialBunker materialBunker = MaterialBunker.InstanceMatBunker;
        materialBunker.CroppedImages.Clear();

        // データを書き込む
        _writer.Commit();
    }
}

// セーブクラスとマネージャークラスが相互に参照を持っているのでマネージャークラス > セーブクラスのみへ修正予定
