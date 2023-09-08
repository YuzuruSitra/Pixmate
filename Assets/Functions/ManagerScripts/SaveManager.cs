using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

// ゲームデータのセーブ管理
public class SaveManager : MonoBehaviour
{
    // 他スクリプトでも呼べるようにインスタンス化
    public static SaveManager InstanceSaveManager;
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
        // データの保存先をApplication.dataPathに変更
        QuickSaveGlobalSettings.StorageLocation = Application.dataPath;

        DoEncryption();
    }

    private void DoEncryption()//暗号化処理
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // 暗号化の方法
        settings.SecurityMode = SecurityMode.None;
        // 暗号化キー
        settings.Password = "Pass";
        // 圧縮の方法
        settings.CompressionMode = CompressionMode.Gzip;
    }

    // マテリアルのセーブ処理
    public void DoSaveMaterialCount(int matCount)
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("Player",settings);

        // データを書き込む
        writer.Write("matCount", matCount);
        // 変更を反映
        writer.Commit();
    }

    public void DoSaveMaterial(Sprite savaSprite, string savaName, string key)
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("Player",settings);

        // Spriteとその名前のセーブ
        string savePhotoSpriteKey = key + PHOTO_SPRITE_KEY;
        writer.Write(savePhotoSpriteKey, savaSprite);

        string savePhotoNamesKey = key + PHOTO_NAME_KEY;
        writer.Write(savePhotoNamesKey, savaName);

        // 変更を反映
        writer.Commit();
    }

    // Spriteのセーブ処理
    public void DoSaveSprite(int matCount, Sprite sprite,string name,string key)
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("Player",settings);

        // データを書き込む
        writer.Write("matCount", matCount);
        writer.Write(key + PHOTO_SPRITE_KEY, sprite);
        writer.Write(key + PHOTO_NAME_KEY, name);

        // 変更を反映
        writer.Commit();
    }

    // Pixmateのセーブ処理
    public void DoSavePixmates(int count, Sprite sprite,string key)
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("Player",settings);

        // データを書き込む
        writer.Write("pixmateCount", count);
        writer.Write(key + PIXMATE_TEXTURE_KEY, sprite);
        
        // 変更を反映
        writer.Commit();
    }

    // Worldのセーブ処理

    // Worldのオブジェクト数をセーブ
    
    public void DoSaveWorldSize(int count)
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("Player",settings);

        // データを書き込む
        writer.Write("worldObjCount", count);

        // 変更を反映
        writer.Commit();
    }

    // Object情報をセーブ
    public void DoSaveWorld(string key, Vector3 pos, Quaternion rot, string shape, int mat)
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("Player",settings);

        // データを書き込む
        writer.Write(key + WORLDOBJ_POS_KEY, pos);
        writer.Write(key + WORLDOBJ_ROT_KEY, rot);
        writer.Write(key + WORLDOBJ_SHAPE_KEY, shape);
        writer.Write(key + WORLDOBJ_MAT_KEY, mat);
        

        // 変更を反映
        writer.Commit();
    }

    // データの読み込み

    // MaterialBunker
    public int LoadCountMat()
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveReaderのインスタンスを作成
        QuickSaveReader reader = QuickSaveReader.Create("Player", settings);

        return reader.Read<int>("matCount");
    }

    public Sprite LoadMaterialSprite(string key)
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveReaderのインスタンスを作成
        QuickSaveReader reader = QuickSaveReader.Create("Player", settings);

        string loadTextureKey = key + PHOTO_SPRITE_KEY;
        return reader.Read<Sprite>(loadTextureKey);
    }

    public string LoadMaterialSpriteName(string key)
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveReaderのインスタンスを作成
        QuickSaveReader reader = QuickSaveReader.Create("Player", settings);

        string loadPhotoNamesKey = key + PHOTO_NAME_KEY;
        return reader.Read<string>(loadPhotoNamesKey);
    }

    // PixmatesManager
    public int LoadPixmateCount()
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveReaderのインスタンスを作成
        QuickSaveReader reader = QuickSaveReader.Create("Player", settings);

        return reader.Read<int>("pixmateCount");
        // デバッグ用
        //return 0;
    }

    public Sprite LoadPixmateSprite(string key)
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveReaderのインスタンスを作成
        QuickSaveReader reader = QuickSaveReader.Create("Player", settings);

        string loadTextureKey = key + PIXMATE_TEXTURE_KEY;
        return reader.Read<Sprite>(loadTextureKey);
    }




    //データ削除
    public void DeleteDate()
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("Player", settings);

        // 初期データ
        writer.Write("matCount", 0);
        MaterialBunker materialBunker = MaterialBunker.InstanceMatBunker;
        materialBunker.CroppedImages.Clear();

        // データを書き込む
        writer.Commit();
    }
}

// セーブクラスとマネージャークラスが相互に参照を持っているのでマネージャークラス > セーブクラスのみへ修正予定
