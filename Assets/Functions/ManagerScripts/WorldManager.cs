using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class WorldManager : MonoBehaviour
{
    public static WorldManager InstanceWorldManager;
    SaveManager _saveManager;
    public const string WORLDOBJ_KEY = "WorldObj";
    [SerializeField] 
    private Transform _mapParent;

    [SerializeField] 
    private DefaultWorldData _defaultWorldData;

    private List<Vector3> _objPosList = new List<Vector3>();
    private List<Quaternion> _objRotList = new List<Quaternion>();
    private List<string> _objTagList = new List<string>();
    private List<int> _objKindList = new List<int>();
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
        _saveManager = SaveManager.InstanceSaveManager;
        //InitialWorldSetting();
        WorldLoad();
    }

    /*------------------------------------------------------------------------*/

    // マテリアルのロード処理（仮）
    void WorldLoad()
    {
        _objPosList = _defaultWorldData.ObjPositions;
        _objRotList = _defaultWorldData.ObjRotations;
        _objTagList = _defaultWorldData.ObjTags;
        _objKindList = _defaultWorldData.ObjKinds;
        _worldObjCount  = _defaultWorldData.WorldObjCount;

        ItemBunker itemBunker = ItemBunker.InstanceItemBunker;

        for (int i = 0; i < _worldObjCount; i++)
        {
            // 生成するオブジェクト
            int insKind = GetItemKind(_objTagList[i]);

            if (insKind == -1)
            {
                Debug.LogError("Invalid tag: " + _objTagList[i]);
                continue;
            }

            GameObject insObj = Instantiate(itemBunker.ItemObject[insKind], _objPosList[i], _objRotList[i], _mapParent);

            // マテリアルの選定
            Material assignMat = GetAssignedMaterial(_objKindList[i]);

            if (assignMat == null)
            {
                Debug.LogError("Invalid material index: " + _objKindList[i]);
                continue;
            }

            Renderer renderer = insObj.GetComponent<Renderer>();
            renderer.material = assignMat;

            // IDの割り振り
            ObjectID objectID = insObj.GetComponent<ObjectID>();
            objectID.SetObjID(i);
        }
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

        if (targetNum < MaterialBunker.MATERIAL_AMOUNT)
        {
            string tmpKey = materialBunker.KeyName + targetNum;
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
        // IDの設定
        ObjectID objectID = targetObj.GetComponent<ObjectID>();
        objectID.SetObjID(_worldObjCount);

        string key = WORLDOBJ_KEY + _worldObjCount;
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
            case "GrassMat (Instance)":
                targetNum = maxMat;
                break;
            case "SandMat (Instance)":
                targetNum = maxMat + 1;
                break;
            case "StemMat (Instance)":
                targetNum = maxMat + 2;
                break;
            case "LeafMat (Instance)":
                targetNum = maxMat + 3;
                break;
            default:
                TryGetKey tryGetKey = new TryGetKey();
                targetNum = tryGetKey.GetMatNumber(targetName);
                break;
        }
        _objPosList.Add(pos);
        _objRotList.Add(rot);
        _objTagList.Add(tag);
        _objKindList.Add(targetNum);

        _worldObjCount  = _objPosList.Count;
        
        // saving.
        _saveManager.DoSaveWorld(key, pos, rot, tag, targetNum);
        _saveManager.DoSaveWorldSize(_worldObjCount);
    }

    // オブジェクト変更時のセーブ処理
    public void ChangeObjSaving(GameObject targetObj)
    {
        // IDの取得
        ObjectID objectID = targetObj.GetComponent<ObjectID>();
        int targetID = objectID.ThisObjID;

        string key = WORLDOBJ_KEY + targetID;
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
            case "GrassMat (Instance)":
                targetNum = maxMat;
                break;
            case "SandMat (Instance)":
                targetNum = maxMat + 1;
                break;
            case "StemMat (Instance)":
                targetNum = maxMat + 2;
                break;
            case "LeafMat (Instance)":
                targetNum = maxMat + 3;
                break;
            default:
                TryGetKey tryGetKey = new TryGetKey();
                targetNum = tryGetKey.GetMatNumber(targetName);
                break;
        }
        _objPosList[targetID] = pos;
        _objRotList[targetID] = rot;
        _objTagList[targetID] = tag;
        _objKindList[targetID] = targetNum;

        // saving.
        _saveManager.DoSaveWorld(key, pos, rot, tag, targetNum);
        _saveManager.DoSaveWorldSize(_worldObjCount);
    }

    // ワールドのオブジェクト削除時のセーブ
    public void DeleteObjSaving(GameObject targetObj)
    {
        // IDの取得
        ObjectID objectID = targetObj.GetComponent<ObjectID>();
        int targetID = objectID.ThisObjID;

        if(targetID >= _worldObjCount)
        {
            Debug.LogError("A value that does not exist.");
            return;
        }
        
        _objPosList.RemoveAt(targetID);
        _objRotList.RemoveAt(targetID);
        _objTagList.RemoveAt(targetID);
        _objKindList.RemoveAt(targetID);

        _worldObjCount  = _objPosList.Count;

        for(int i = targetID; i < _worldObjCount; i++)
        {
            string key = WORLDOBJ_KEY + i;
            // saving.
            _saveManager.DoSaveWorld(key, _objPosList[i], _objRotList[i], _objTagList[i], _objKindList[i]);
        }

        // saving.
        _saveManager.DoSaveWorldSize(_worldObjCount);
    }

    /*------------------------------------------------------------------------*/

    // 初期マップの作成用のセーブ処理
    /*
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

        for(int i = 0; i < worldObjCount; i++)
        {
            string key = WORLDOBJ_KEY + i;
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
                case "GrassMat (Instance)":
                    targetNum = maxMat;
                    break;
                case "SandMat (Instance)":
                    targetNum = maxMat + 1;
                    break;
                case "StemMat (Instance)":
                    targetNum = maxMat + 2;
                    break;
                case "LeafMat (Instance)":
                    targetNum = maxMat + 3;
                    break;
                default:
                    TryGetKey tryGetKey = new TryGetKey();
                    targetNum = tryGetKey.GetMatNumber(targetName);
                    break;
            }
            _objPosList.Add(pos);
            _objRotList.Add(rot);
            _objTagList.Add(tag);
            _objKindList.Add(targetNum);
        }
        
        _defaultWorldData.ObjPositions = _objPosList;
        _defaultWorldData.ObjRotations = _objRotList;
        _defaultWorldData.ObjTags = _objTagList;
        _defaultWorldData.ObjKinds = _objKindList;
        _defaultWorldData.WorldObjCount = _defaultWorldData.ObjPositions.Count;

        //ダーティとしてマークする(変更があった事を記録する)
        EditorUtility.SetDirty(_defaultWorldData);

        //保存する
        AssetDatabase.SaveAssets();
    }
    */
    
}
