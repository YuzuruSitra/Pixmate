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
    public void DoSaveMaterial()
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("Player",settings);

        MaterialBunker materialBunker = MaterialBunker.InstanceMatBunker;   
        // データを書き込む
        writer.Write("matCount", materialBunker.MatCount);

        // MaterialBunkerのセーブ処理        
        string keyTemplate = materialBunker.KeyName;
        Dictionary<string, Sprite> tmpPhotoSprites = materialBunker.CroppedImages;
        Dictionary<string, string> tmpPhotoNames = materialBunker.ImageNames;
        for(int i = 0; i < materialBunker.MatCount; i++)
        {
            int nameCount = i + 1;
            string pullTmpKey = keyTemplate + nameCount;

            // PhotoSpriteの書き込み
            string savePhotoSpriteKey = pullTmpKey + PHOTO_SPRITE_KEY;
            Sprite tmpPhotoSpriteValue = tmpPhotoSprites[pullTmpKey];
            writer.Write(savePhotoSpriteKey, tmpPhotoSpriteValue);

            // PhotoNameの書き込み
            string savePhotoNamesKey = pullTmpKey + PHOTO_NAME_KEY;
            string tmpPhotoNamesValue = tmpPhotoNames[pullTmpKey];
            writer.Write(savePhotoNamesKey, tmpPhotoNamesValue);
        }

        // 変更を反映
        writer.Commit();
    }

    //Spriteのセーブ処理
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
