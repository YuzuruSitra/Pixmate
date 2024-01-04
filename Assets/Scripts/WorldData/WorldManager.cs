using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    private MaterialBunker _materialBunker;
    [SerializeField]
    private ItemBunker _itemBunker;
    private bool _isInitialization;
    [SerializeField]
    private WorldIO _worldIO;
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

    // ワールドのロード処理（仮）
    
    public void Load()
    {
        _isInitialization = _worldIO.LoadWorldInitialization();
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
            _worldObjCount = _worldIO.LoadWorldSize();
            // 例外が起きた場合初期マップをロード
            if(_worldObjCount == 0) 
            {
                _worldIO.DoSaveWorldInitialization(true);
                Load();
                return;
            }

            for(int i = 0; i < _worldObjCount; i++)
            {
                string key = WORLD_OBJ_KEY + i;
                _objPosList.Add(_worldIO.LoadWorldObjPos(key));
                _objRotList.Add(_worldIO.LoadWorldObjRot(key));
                _objShapeList.Add(_worldIO.LoadWorldObjShape(key));
                _objMatList.Add(_worldIO.LoadWorldObjMat(key));
            }
        }  

        for (int i = 0; i < _worldObjCount; i++)
        {
            // 生成するオブジェクト
            int insKind = GetItemKind(_objShapeList[i]);

            if (insKind == -1)
            {
                Debug.LogError("Invalid tag: " + _objShapeList[i]);
                continue;
            }

            GameObject insObj = Instantiate(_itemBunker.ItemObject[insKind], _objPosList[i], _objRotList[i], _mapParent);

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
            _worldIO.DoSaveWorld(key, _objPosList[i], _objRotList[i], _objShapeList[i], _objMatList[i]);
        }
        _worldIO.DoSaveWorldSize(_worldObjCount);

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
        int keyNum = targetNum + 1;
        if (keyNum < MaterialBunker.MATERIAL_AMOUNT)
        {
            string tmpKey = MaterialBunker.KEY_NAME + keyNum;
            return _materialBunker.ImageMaterials.ContainsKey(tmpKey) ? _materialBunker.ImageMaterials[tmpKey] : null;
        }
        else
        {
            int defaultMat = targetNum - MaterialBunker.MATERIAL_AMOUNT;
            if (defaultMat >= 0 && defaultMat < _materialBunker.DefaultMat.Length)
            {
                return _materialBunker.DefaultMat[defaultMat];
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
        _worldIO.DoSaveWorld(key, pos, rot, tag, targetNum);
        _worldIO.DoSaveWorldSize(_worldObjCount);

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
        _worldIO.DoSaveWorld(key, pos, rot, tag, targetNum);
        _worldIO.DoSaveWorldSize(_worldObjCount);

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
            _worldIO.DoSaveWorld(key, _objPosList[i], _objRotList[i], _objShapeList[i], _objMatList[i]);
        }
        // saving.
        _worldIO.DoSaveWorldSize(_worldObjCount);
        SettingInitial();
    }

    void SettingInitial()
    {
        if(!_isInitialization) return;
        _isInitialization = false;
        _worldIO.DoSaveWorldInitialization(_isInitialization);
    } 
    
}
