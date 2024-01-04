using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

// Scene上で初期マップのデータ入力を行うクラス
public class DraftMapDataCreator : MonoBehaviour
{
    [SerializeField] 
    private DefaultWorldData _defaultWorldData;
    [SerializeField]
    private WorldIO _worldIO;

    
#if UNITY_EDITOR
    void Start()
    {
        InputDraftMapData();
    }

    void InputDraftMapData()
    {
        // タグごとにゲームオブジェクトを検索し、taggedObjects1 に結合
        GameObject[] taggedObjects1 = GameObject.FindGameObjectsWithTag("Cube");
        GameObject[] taggedObjects2 = GameObject.FindGameObjectsWithTag("HalfCube");
        GameObject[] taggedObjects3 = GameObject.FindGameObjectsWithTag("Step");
        GameObject[] taggedObjects4 = GameObject.FindGameObjectsWithTag("SmallCube");

        // すべての結果を一つの配列に結合
        GameObject[] allTaggedObjects = taggedObjects1.Concat(taggedObjects2).Concat(taggedObjects3).Concat(taggedObjects4).ToArray();
        
        int worldObjCount = allTaggedObjects.Length;

        List<Vector3> objPosList = new List<Vector3>();
        List<Quaternion> objRotList = new List<Quaternion>();
        List<string> objShapeList = new List<string>();
        List<int> objMatList = new List<int>();

        for(int i = 0; i < worldObjCount; i++)
        {
            string key = WorldManager.WORLD_OBJ_KEY + i;
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
            objPosList.Add(pos);
            objRotList.Add(rot);
            objShapeList.Add(tag);
            objMatList.Add(targetNum);

            _worldIO.DoSaveWorld(key, pos, rot, tag, targetNum);
        }
        
        _defaultWorldData.ObjPositions = objPosList;
        _defaultWorldData.ObjRotations = objRotList;
        _defaultWorldData.ObjTags = objShapeList;
        _defaultWorldData.ObjKinds = objMatList;
        _defaultWorldData.WorldObjCount = _defaultWorldData.ObjPositions.Count;

        //ダーティとしてマークする(変更があった事を記録する)
        EditorUtility.SetDirty(_defaultWorldData);

        //保存する
        AssetDatabase.SaveAssets();
    }
#endif
}
