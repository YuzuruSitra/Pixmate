using UnityEngine;
using CI.QuickSave;
using System; 

public class WorldIO : MonoBehaviour
{
    private QuickSaveReader _reader;
    private QuickSaveWriter _writer;
    private const string WORLDOBJ_POS_KEY = "_Pos";
    private const string WORLDOBJ_ROT_KEY = "_Rot";
    private const string WORLDOBJ_SHAPE_KEY = "_Shape";
    private const string WORLDOBJ_MAT_KEY = "_Mat";

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

    // Load
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

    // Delete
    public void DeleteData()
    {
        DoSaveWorldInitialization(true);
    }
}
