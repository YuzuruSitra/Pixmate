using System.Collections.Generic;
using UnityEngine;

public class WorldDataBunker : MonoBehaviour
{
    [SerializeField]
    private WorldIO _worldIO;
    [SerializeField]
    private DefaultWorldData _defaultWorldData;

    public List<Vector3> ObjPosList => _objPosList;
    public List<Quaternion> ObjRotList => _objRotList;
    public List<string> ObjShapeList => _objShapeList;
    public List<int> ObjMatList => _objMatList;
    public int WorldObjCount => _worldObjCount;
    public const string WORLD_OBJ_KEY = "WorldObj";

    private bool _isInitialization;
    private List<Vector3> _objPosList = new List<Vector3>();
    private List<Quaternion> _objRotList = new List<Quaternion>();
    private List<string> _objShapeList = new List<string>();
    private List<int> _objMatList = new List<int>();
    private int _worldObjCount;

    public void LoadWorldData()
    {
        _isInitialization = _worldIO.LoadWorldInitialization();

        if (_isInitialization)
            LoadDefaultWorldData();
        else
            LoadCustomWorldData();

        if (_isInitialization) AllDataSaving();
    }

    private void LoadDefaultWorldData()
    {
        _worldObjCount = _defaultWorldData.WorldObjCount;
        _objPosList = _defaultWorldData.ObjPositions;
        _objRotList = _defaultWorldData.ObjRotations;
        _objShapeList = _defaultWorldData.ObjTags;
        _objMatList = _defaultWorldData.ObjKinds;
    }

    private void LoadCustomWorldData()
    {
        _worldObjCount = _worldIO.LoadWorldSize();

        if (_worldObjCount == 0)
        {
            _worldIO.DoSaveWorldInitialization(true);
            LoadWorldData();
            return;
        }

        for (int i = 0; i < _worldObjCount; i++)
        {
            LoadWorldObjectData(i);
        }
    }

    private void LoadWorldObjectData(int index)
    {
        string key = WORLD_OBJ_KEY + index;
        _objPosList.Add(_worldIO.LoadWorldObjPos(key));
        _objRotList.Add(_worldIO.LoadWorldObjRot(key));
        _objShapeList.Add(_worldIO.LoadWorldObjShape(key));
        _objMatList.Add(_worldIO.LoadWorldObjMat(key));
    }

    public void AllDataSaving()
    {
        for (int i = 0; i < _worldObjCount; i++)
        {
            SaveWorldObjectData(i);
        }

        _worldIO.DoSaveWorldSize(_worldObjCount);
        _isInitialization = false;
        _worldIO.DoSaveWorldInitialization(_isInitialization);
    }

    private void SaveWorldObjectData(int index)
    {
        string key = WORLD_OBJ_KEY + index;
        _worldIO.DoSaveWorld(key, _objPosList[index], _objRotList[index], _objShapeList[index], _objMatList[index]);
    }

    public void InsObjSaving(GameObject targetObj)
    {
        if (targetObj == null) return;

        ObjectID objectID = targetObj.GetComponent<ObjectID>();
        objectID.SetObjID(_worldObjCount);

        string key = WORLD_OBJ_KEY + _worldObjCount;
        Vector3 pos = RoundVector3ToInt(targetObj.transform.position);
        Quaternion rot = targetObj.transform.rotation;
        string tag = targetObj.tag;
        int targetNum = GetMaterialNumber(targetObj);

        _objPosList.Add(pos);
        _objRotList.Add(rot);
        _objShapeList.Add(tag);
        _objMatList.Add(targetNum);

        _worldObjCount = _objPosList.Count;

        SaveWorldObjectData(_worldObjCount - 1);
        _worldIO.DoSaveWorldSize(_worldObjCount);
    }

    public void ChangeObjSaving(GameObject targetObj)
    {
        if (targetObj == null) return;

        ObjectID objectID = targetObj.GetComponent<ObjectID>();
        int targetID = objectID.ThisObjID;

        string key = WORLD_OBJ_KEY + targetID;
        Vector3 pos = RoundVector3ToInt(targetObj.transform.position);
        Quaternion rot = targetObj.transform.rotation;
        string tag = targetObj.tag;
        int targetNum = GetMaterialNumber(targetObj);

        _objPosList[targetID] = pos;
        _objRotList[targetID] = rot;
        _objShapeList[targetID] = tag;
        _objMatList[targetID] = targetNum;

        SaveWorldObjectData(targetID);
        _worldIO.DoSaveWorldSize(_worldObjCount);
    }

    public void DeleteObjSaving(GameObject targetObj)
    {
        ObjectID objectID = targetObj.GetComponent<ObjectID>();
        int targetID = objectID.ThisObjID;

        if (targetID > _worldObjCount)
        {
            Debug.LogError("A value that does not exist.");
            return;
        }

        _objPosList.RemoveAt(targetID);
        _objRotList.RemoveAt(targetID);
        _objShapeList.RemoveAt(targetID);
        _objMatList.RemoveAt(targetID);

        _worldObjCount = _objPosList.Count;

        for (int i = targetID; i < _worldObjCount; i++)
        {
            SaveWorldObjectData(i);
        }

        _worldIO.DoSaveWorldSize(_worldObjCount);
    }

    private Vector3 RoundVector3ToInt(Vector3 vector)
    {
        return new Vector3(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
    }

    private int GetMaterialNumber(GameObject targetObj)
    {
        string targetName = targetObj.GetComponent<MeshRenderer>().material.name;
        int maxMat = MaterialBunker.MATERIAL_AMOUNT;

        switch (targetName)
        {
            case "DefaultMat (Instance)":
                return maxMat;
            case "GrassMat (Instance)":
                return maxMat + 1;
            case "SandMat (Instance)":
                return maxMat + 2;
            case "StemMat (Instance)":
                return maxMat + 3;
            case "LeafMat (Instance)":
                return maxMat + 4;
            default:
                TryGetKey tryGetKey = new TryGetKey();
                return tryGetKey.GetMatNumber(targetName);
        }
    }
}
