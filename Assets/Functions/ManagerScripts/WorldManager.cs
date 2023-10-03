using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class WorldManager : MonoBehaviour
{
    private bool _isInitialization;
    public static WorldManager InstanceWorldManager;
    SaveManager _saveManager;
    public const string WORLD_OBJ_KEY = "WorldObj";
    [SerializeField] 
    private Transform _mapParent;

    [SerializeField] 
    private DefaultWorldData _defaultWorldData;

    [SerializeField]
    private List<Vector3> _objPosList = new List<Vector3>();
    [SerializeField]
    private List<Quaternion> _objRotList = new List<Quaternion>();
    [SerializeField]
    private List<string> _objShapeList = new List<string>();
    [SerializeField]
    private List<int> _objMatList = new List<int>();
    [SerializeField]
    private int _worldObjCount;

    void Awake()
    {
        if (InstanceWorldManager == null)
        {
            InstanceWorldManager = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*------------------------------------------------------------------------*/

    // ワールドのロード処理（仮）
    
    public void WorldLoad()
    {
        _saveManager = SaveManager.InstanceSaveManager;
        _isInitialization = _saveManager.LoadWorldInitialization();
        //_isInitialization = true;
        if(_isInitialization)
        {
            _worldObjCount  = _defaultWorldData.WorldObjCount;
            _objPosList = _defaultWorldData.ObjPositions;
            _objRotList = _defaultWorldData.ObjRotations;
            _objShapeList = _defaultWorldData.ObjTags;
            _objMatList = _defaultWorldData.ObjKinds;
        }
        else
        {
            _worldObjCount = _saveManager.LoadWorldSize();
            // 例外が起きた場合初期マップをロード
            if(_worldObjCount == 0) 
            {
                _saveManager.DoSaveWorldInitialization(true);
                WorldLoad();
                return;
            }

            for(int i = 0; i < _worldObjCount; i++)
            {
                string key = WORLD_OBJ_KEY + i;
                _objPosList.Add(_saveManager.LoadWorldObjPos(key));
                _objRotList.Add(_saveManager.LoadWorldObjRot(key));
                _objShapeList.Add(_saveManager.LoadWorldObjShape(key));
                _objMatList.Add(_saveManager.LoadWorldObjMat(key));
            }
        }

        ItemBunker itemBunker = ItemBunker.InstanceItemBunker;

        for (int i = 0; i < _worldObjCount; i++)
        {
            // 生成するオブジェクト
            int insKind = GetItemKind(_objShapeList[i]);

            if (insKind == -1)
            {
                Debug.LogError("Invalid tag: " + _objShapeList[i]);
                continue;
            }

            GameObject insObj = Instantiate(itemBunker.ItemObject[insKind], _objPosList[i], _objRotList[i], _mapParent);

            // マテリアルの選定
            Material assignMat = GetAssignedMaterial(_objMatList[i]);

            if (assignMat == null)
            {
                Debug.LogError("Invalid material index: " + _objMatList[i]);
                continue;
            }

            Renderer renderer = insObj.GetComponent<Renderer>();
            renderer.GetComponent<Renderer>().material = assignMat;

            // IDの割り振り
            ObjectID objectID = insObj.GetComponent<ObjectID>();
            objectID.SetObjID(i);
        }

        // 初めてワールドを生成した場合はセーブする。
        if(!_isInitialization) return;

        Debug.Log(_worldObjCount);
        for(int i = 0; i < _worldObjCount; i++)
        {
            string key = WORLD_OBJ_KEY + i;
            _saveManager.DoSaveWorld(key, _objPosList[i], _objRotList[i], _objShapeList[i], _objMatList[i]);
        }
        _saveManager.DoSaveWorldSize(_worldObjCount);

    }
    

    int GetItemKind(string tag)
    {
        switch (tag)
        {
            case "Cube":
                return 0;
            case "HalfCube":
                return 1;
            case "Step":
                return 2;
            case "SmallCube":
                return 3;
            default:
                return -1; // 無効なタグの場合
        }
    }

    Material GetAssignedMaterial(int targetNum)
    {
        MaterialBunker materialBunker = MaterialBunker.InstanceMatBunker;
        
        int keyNum = targetNum + 1;
        if (keyNum < MaterialBunker.MATERIAL_AMOUNT)
        {
            string tmpKey = materialBunker.KeyName + keyNum;
            return materialBunker.ImageMaterials.ContainsKey(tmpKey) ? materialBunker.ImageMaterials[tmpKey] : null;
        }
        else
        {
            int defaultMat = targetNum - MaterialBunker.MATERIAL_AMOUNT;
            if (defaultMat >= 0 && defaultMat < materialBunker.DefaultMat.Length)
            {
                return materialBunker.DefaultMat[defaultMat];
            }
            return null;
        }
    }

    /*------------------------------------------------------------------------*/

    // オブジェクト追加時のセーブ処理
    public void InsObjSaving(GameObject targetObj)
    {
        if(targetObj == null) return;
        Debug.Log("Add Saving");

        // IDの設定
        ObjectID objectID = targetObj.GetComponent<ObjectID>();
        objectID.SetObjID(_worldObjCount);

        string key = WORLD_OBJ_KEY + _worldObjCount;
        // オブジェクトの位置を整数に変換し格納
        Vector3 tmpPos = targetObj.transform.position;
        int x = Mathf.RoundToInt(tmpPos.x);
        int y = Mathf.RoundToInt(tmpPos.y);
        int z = Mathf.RoundToInt(tmpPos.z); 
        Vector3 pos = new Vector3(x, y, z);

        Quaternion rot = targetObj.transform.rotation;
        string tag = targetObj.tag;

        // 対象のマテリアルからキーを抽出
        string targetName = targetObj.GetComponent<MeshRenderer>().material.name;
        
        int targetNum = 0;
        // 初期マテリアル保存用の処理
        int maxMat = MaterialBunker.MATERIAL_AMOUNT;
        switch(targetName)
        {
            case "DefaultMat (Instance)":
                targetNum = maxMat;
                break;
            case "GrassMat (Instance)":
                targetNum = maxMat + 1;
                break;
            case "SandMat (Instance)":
                targetNum = maxMat + 2;
                break;
            case "StemMat (Instance)":
                targetNum = maxMat + 3;
                break;
            case "LeafMat (Instance)":
                targetNum = maxMat + 4;
                break;
            default:
                TryGetKey tryGetKey = new TryGetKey();
                targetNum = tryGetKey.GetMatNumber(targetName);
                break;
        }
        _objPosList.Add(pos);
        _objRotList.Add(rot);
        _objShapeList.Add(tag);
        _objMatList.Add(targetNum);

        _worldObjCount  = _objPosList.Count;
        
        // saving.
        _saveManager.DoSaveWorld(key, pos, rot, tag, targetNum);
        _saveManager.DoSaveWorldSize(_worldObjCount);

        SettingInitial();
    }

    // オブジェクト変更時のセーブ処理
    public void ChangeObjSaving(GameObject targetObj)
    {
        if(targetObj == null) return;
        Debug.Log("Change Saving");

        // IDの取得
        ObjectID objectID = targetObj.GetComponent<ObjectID>();
        int targetID = objectID.ThisObjID;

        string key = WORLD_OBJ_KEY + targetID;
        // オブジェクトの位置を整数に変換し格納
        Vector3 tmpPos = targetObj.transform.position;
        int x = Mathf.RoundToInt(tmpPos.x);
        int y = Mathf.RoundToInt(tmpPos.y);
        int z = Mathf.RoundToInt(tmpPos.z); 
        Vector3 pos = new Vector3(x, y, z);

        Quaternion rot = targetObj.transform.rotation;
        string tag = targetObj.tag;

        // 対象のマテリアルからキーを抽出
        string targetName = targetObj.GetComponent<MeshRenderer>().material.name;
        
        int targetNum = 0;
        // 初期マテリアル保存用の処理
        int maxMat = MaterialBunker.MATERIAL_AMOUNT;
        switch(targetName)
        {
            case "DefaultMat (Instance)":
                targetNum = maxMat;
                break;
            case "GrassMat (Instance)":
                targetNum = maxMat + 1;
                break;
            case "SandMat (Instance)":
                targetNum = maxMat + 2;
                break;
            case "StemMat (Instance)":
                targetNum = maxMat + 3;
                break;
            case "LeafMat (Instance)":
                targetNum = maxMat + 4;
                break;
            default:
                TryGetKey tryGetKey = new TryGetKey();
                targetNum = tryGetKey.GetMatNumber(targetName);
                break;
        }
        _objPosList[targetID] = pos;
        _objRotList[targetID] = rot;
        _objShapeList[targetID] = tag;
        _objMatList[targetID] = targetNum;

        // saving.
        _saveManager.DoSaveWorld(key, pos, rot, tag, targetNum);
        _saveManager.DoSaveWorldSize(_worldObjCount);

        SettingInitial();
    }

    // ワールドのオブジェクト削除時のセーブ
    public void DeleteObjSaving(GameObject targetObj)
    {
        // IDの取得
        ObjectID objectID = targetObj.GetComponent<ObjectID>();
        int targetID = objectID.ThisObjID;

        if(targetID > _worldObjCount)
        {
            Debug.LogError("A value that does not exist.");
            return;
        }

        Debug.Log("Delete Saving");
        // 修正予定:削除時にIDをソートし直す必要あり
        _objPosList.RemoveAt(targetID);
        _objRotList.RemoveAt(targetID);
        _objShapeList.RemoveAt(targetID);
        _objMatList.RemoveAt(targetID);

        _worldObjCount  = _objPosList.Count;

        for(int i = targetID; i < _worldObjCount; i++)
        {
            string key = WORLD_OBJ_KEY + i;
            // saving.
            _saveManager.DoSaveWorld(key, _objPosList[i], _objRotList[i], _objShapeList[i], _objMatList[i]);
        }
        // saving.
        _saveManager.DoSaveWorldSize(_worldObjCount);
        SettingInitial();
    }

    void SettingInitial()
    {
        if(!_isInitialization) return;
        _isInitialization = false;
        _saveManager.DoSaveWorldInitialization(_isInitialization);
    }

    /*------------------------------------------------------------------------*/

    // 初期マップの作成用のセーブ処理

    
#if UNITY_EDITOR
    void InitialWorldSetting()
    {
        // タグごとにゲームオブジェクトを検索し、taggedObjects1 に結合
        GameObject[] taggedObjects1 = GameObject.FindGameObjectsWithTag("Cube");
        GameObject[] taggedObjects2 = GameObject.FindGameObjectsWithTag("HalfCube");
        GameObject[] taggedObjects3 = GameObject.FindGameObjectsWithTag("Step");
        GameObject[] taggedObjects4 = GameObject.FindGameObjectsWithTag("SmallCube");

        // すべての結果を一つの配列に結合
        GameObject[] allTaggedObjects = taggedObjects1.Concat(taggedObjects2).Concat(taggedObjects3).Concat(taggedObjects4).ToArray();
        
        int worldObjCount = allTaggedObjects.Length;

        SaveManager saveManager = SaveManager.InstanceSaveManager;

        for(int i = 0; i < worldObjCount; i++)
        {
            string key = WORLD_OBJ_KEY + i;
            // オブジェクトの位置を整数に変換し格納
            Vector3 tmpPos = allTaggedObjects[i].transform.position;
            int x = Mathf.RoundToInt(tmpPos.x);
            int y = Mathf.RoundToInt(tmpPos.y);
            int z = Mathf.RoundToInt(tmpPos.z); 
            Vector3 pos = new Vector3(x, y, z);

            Quaternion rot = allTaggedObjects[i].transform.rotation;
            string tag = allTaggedObjects[i].tag;

            // 対象のマテリアルからキーを抽出
            string targetName = allTaggedObjects[i].GetComponent<MeshRenderer>().material.name;
            
            int targetNum = 0;
            // 初期マテリアル保存用の処理
            int maxMat = MaterialBunker.MATERIAL_AMOUNT;
            switch(targetName)
            {
                case "DefaultMat (Instance)":
                    targetNum = maxMat;
                    break;
                case "GrassMat (Instance)":
                    targetNum = maxMat + 1;
                    break;
                case "SandMat (Instance)":
                    targetNum = maxMat + 2;
                    break;
                case "StemMat (Instance)":
                    targetNum = maxMat + 3;
                    break;
                case "LeafMat (Instance)":
                    targetNum = maxMat + 4;
                    break;
                default:
                    TryGetKey tryGetKey = new TryGetKey();
                    targetNum = tryGetKey.GetMatNumber(targetName);
                    break;
            }
            _objPosList.Add(pos);
            _objRotList.Add(rot);
            _objShapeList.Add(tag);
            _objMatList.Add(targetNum);

            saveManager.DoSaveWorld(key, pos, rot, tag, targetNum);
        }
        
        _defaultWorldData.ObjPositions = _objPosList;
        _defaultWorldData.ObjRotations = _objRotList;
        _defaultWorldData.ObjTags = _objShapeList;
        _defaultWorldData.ObjKinds = _objMatList;
        _defaultWorldData.WorldObjCount = _defaultWorldData.ObjPositions.Count;

        //ダーティとしてマークする(変更があった事を記録する)
        EditorUtility.SetDirty(_defaultWorldData);

        //保存する
        AssetDatabase.SaveAssets();
    }
#endif
    
    
}
