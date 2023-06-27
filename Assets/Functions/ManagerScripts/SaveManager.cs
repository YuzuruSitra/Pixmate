using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

// ゲームデータのセーブ管理
public class SaveManager : MonoBehaviour
{
    
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

        // データを書き込む
        writer.Write("matCount", MaterialBunker.InstanceMatBunker.MatCount);

        //スプライトのセーブ処理
        foreach (KeyValuePair<string, Sprite> entry in MaterialBunker.InstanceMatBunker.CroppedImages)
        {
            string key = entry.Key;
            Sprite value = entry.Value;

            // ここでキーと値に対する処理を行う
            writer.Write(key, value);
        }

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
        
        MaterialBunker.InstanceMatBunker.MatCount = reader.Read<int>("matCount");
        for(int i = 0; i < MaterialBunker.InstanceMatBunker.MatCount; i++)
        {
            int nameCount = i + 1;
            string tmpKey = MaterialBunker.InstanceMatBunker.KeyName;
            tmpKey += nameCount;
            Sprite tmpValue = reader.Read<Sprite>(tmpKey);
            MaterialBunker.InstanceMatBunker.CroppedImages.Add(tmpKey, tmpValue);
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
