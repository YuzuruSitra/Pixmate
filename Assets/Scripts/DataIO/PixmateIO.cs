using UnityEngine;
using CI.QuickSave;
using System; 

public class PixmateIO : MonoBehaviour
{
    private QuickSaveReader _reader;
    private QuickSaveWriter _writer;
    private const string PIXMATE_TEXTURE_KEY = "_Texture";
    private const string PIXMATE_FORM_KEY = "_ForM";
    private const string PIXMATE_POS_KEY = "_Pos";
    private const string PIXMATE_SCALE_KEY = "_Scale";
    private const string PIXMATE_ROT_KEY = "_Rot";

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
    public void DoSavePixmates(int count, Sprite sprite, int ForM,string key)
    {
        // データを書き込む
        _writer.Write("pixmateCount", count);
        _writer.Write(key + PIXMATE_TEXTURE_KEY, sprite);
        Debug.Log("Save:"+key);
        _writer.Write(key + PIXMATE_FORM_KEY, ForM);
        // 変更を反映
        _writer.Commit();
    }

    public void DoSavePixmatePos(Vector3 pos, string key)
    {
        _writer.Write(key + PIXMATE_POS_KEY, pos);
        _writer.Commit();
    }

    public void DoSavePixmateScale(float scale, string key)
    {
        _writer.Write(key + PIXMATE_SCALE_KEY, scale);
        _writer.Commit();
    }

    public void DoSavePixmateRot(Quaternion rot, string key)
    {
        _writer.Write(key + PIXMATE_ROT_KEY, rot);
        _writer.Commit();
    }

    // Load
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

    public int LoadPixmateForM(string key)
    {
        // デバッグ用
        string loadForMKey = key + PIXMATE_FORM_KEY;
        try
        {
            return _reader.Read<int>(loadForMKey);
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

    public Vector3 LoadPixmatePosition(string key)
    {
        string loadPosKey = key + PIXMATE_POS_KEY;
        try
        {
            return _reader.Read<Vector3>(loadPosKey);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("データの読み込みエラー: " + ex.Message);

            return new Vector3( UnityEngine.Random.Range(-3f, 4f), 1f, UnityEngine.Random.Range(-3f, 4f));
        }
    }

    public float LoadPixmateScale(string key , float initialScale)
    {
        string loadPosKey = key + PIXMATE_SCALE_KEY;
        try
        {
            return _reader.Read<float>(loadPosKey);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("データの読み込みエラー: " + ex.Message);

            return initialScale;
        }
    }

    public Quaternion LoadPixmateRot(string key)
    {
        string loadPosKey = key + PIXMATE_ROT_KEY;
        try
        {
            return _reader.Read<Quaternion>(loadPosKey);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("データの読み込みエラー: " + ex.Message);

            return Quaternion.Euler(0f, UnityEngine.Random.Range(-180f, 181f), 0f); ;
        }
    }

    // Delete
    public void DeleteData()
    {
        // 初期データ
        _writer.Write("pixmateCount", 0);
        // データを書き込む
        _writer.Commit();
    }
}
