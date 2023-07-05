using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

// ゲームデータのセーブ管理
public class SaveManager : MonoBehaviour
{
    // 他スクリプトでも呼べるようにインスタンス化
    public static SaveManager InstanceSaveManager;
    private const string PHOTO_SPRITE_KEY = "_Sprite";
    private const string PHOTO_NAME_KEY = "_Name";

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
        Doload();
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

    //基本セーブ処理
    public void Dosave()
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("Player",settings);
        MaterialBunker instanceMatBunker = MaterialBunker.InstanceMatBunker;

        // データを書き込む
        writer.Write("matCount", instanceMatBunker.MatCount);

        // MaterialBunkerのセーブ処理        
        string keyTemplate = instanceMatBunker.KeyName;
        Dictionary<string, Sprite> tmpPhotoSprites = instanceMatBunker.CroppedImages;
        Dictionary<string, string> tmpPhotoNames = instanceMatBunker.ImageNames;
        for(int i = 0; i < instanceMatBunker.MatCount; i++)
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

    //Spriteセーブ処理
    public void DoSaveSprite(int matCount, Sprite sprite,string name,string key)
    {
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveWriterのインスタンスを作成
        QuickSaveWriter writer = QuickSaveWriter.Create("Player",settings);
        MaterialBunker instanceMatBunker = MaterialBunker.InstanceMatBunker;

        // データを書き込む
        writer.Write("matCount", matCount);
        writer.Write(key + PHOTO_SPRITE_KEY, sprite);
        writer.Write(key + PHOTO_NAME_KEY, name);

        // 変更を反映
        writer.Commit();
    }

    // データを読み込む
    public void Doload()
    {        
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // QuickSaveReaderのインスタンスを作成
        QuickSaveReader reader = QuickSaveReader.Create("Player", settings);
        MaterialBunker instanceMatBunker = MaterialBunker.InstanceMatBunker;
        
        // MaterialBunkerのセーブ処理        
        string keyTemplate = instanceMatBunker.KeyName;
        instanceMatBunker.MatCount = reader.Read<int>("matCount");

        for(int i = 0; i < instanceMatBunker.MatCount; i++)
        {
            int nameCount = i + 1;
            string addTmpKey = keyTemplate + nameCount;

            // CroppedImagesに追加。
            string loadPhotoSpritesKey = addTmpKey + PHOTO_SPRITE_KEY;
            Sprite tmpPhotoSpriteValue = reader.Read<Sprite>(loadPhotoSpritesKey);
            instanceMatBunker.CroppedImages.Add(addTmpKey, tmpPhotoSpriteValue);

            // ImageNamesに追加。
            string loadPhotoNamesKey = addTmpKey + PHOTO_NAME_KEY;
            string tmpPhotoNamesValue = reader.Read<string>(loadPhotoNamesKey);
            instanceMatBunker.ImageNames.Add(addTmpKey, tmpPhotoNamesValue);
        }

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
        MaterialBunker.InstanceMatBunker.CroppedImages.Clear();

        // データを書き込む
        writer.Commit();
    }
}
