using UnityEngine;
using CI.QuickSave;
using System; 

// マテリアル関連のセーブ・ロード処理
public class MaterialIO : MonoBehaviour
{
    private QuickSaveReader _reader;
    private QuickSaveWriter _writer;
    private const string PHOTO_SPRITE_KEY = "_Sprite";
    private const string PHOTO_NAME_KEY = "_Name";

    void Awake()
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

    // Save
    public void DoSaveSprite(int matCount, Sprite sprite, string name, string key)
    {
        _writer.Write("matCount", matCount);
        _writer.Write(key + PHOTO_SPRITE_KEY, sprite);
        _writer.Write(key + PHOTO_NAME_KEY, name);

        _writer.Commit();
    }

    // Load
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

    // Delete
    public void DeleteDate()
    {
        // 初期データ
        _writer.Write("matCount", 0);
        // _materialBunker.CroppedImages.Clear();

        // データを書き込む
        _writer.Commit();
    }

}
