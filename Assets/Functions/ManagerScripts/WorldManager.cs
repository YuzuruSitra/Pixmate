using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager InstanceWorldManager;
    SaveManager _saveManager;
    public const string WORLDOBJ_KEY = "WorldObj.";
    List<GameObject> _worldObjList = new List<GameObject>();
    List<Vector3> _objPosList = new List<Vector3>();
    List<Quaternion> _objRotList = new List<Quaternion>();
    List<string> _objTagList = new List<string>();
    List<int> _objKindList = new List<int>();
    
    [SerializeField]
    private int _worldObjCount;


    // 仮置き
    public int WorldEdgeXMax = 5;
    public int WorldEdgeXMin = -5;
    public int WorldEdgeZMax = 5;
    public int WorldEdgeZMin = -5;

    public WorldData worldData;

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

        // タグごとにゲームオブジェクトを検索し、taggedObjects1 に結合
        GameObject[] taggedObjects1 = GameObject.FindGameObjectsWithTag("Cube");
        GameObject[] taggedObjects2 = GameObject.FindGameObjectsWithTag("HalfCube");
        GameObject[] taggedObjects3 = GameObject.FindGameObjectsWithTag("Step");
        GameObject[] taggedObjects4 = GameObject.FindGameObjectsWithTag("SmallCube");

        // すべての結果を一つの配列に結合
        GameObject[] allTaggedObjects = taggedObjects1.Concat(taggedObjects2).Concat(taggedObjects3).Concat(taggedObjects4).ToArray();
        
        _worldObjCount = allTaggedObjects.Length;

        for(int i = 0; i < _worldObjCount; i++)
        {
            string key = WORLDOBJ_KEY + i;
            Vector3 pos = allTaggedObjects[i].transform.position;
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
            Debug.Log(tag);

            _objPosList.Add(pos);
            _objRotList.Add(rot);
            _objTagList.Add(tag);
            _objKindList.Add(targetNum);

            // 仮置き
            _saveManager.DoSaveWorld(key, pos, rot, tag, targetNum);
        }
        
        worldData.objPositions = _objPosList;
        worldData.objRotations = _objRotList;
        worldData.objtags = _objTagList;
        worldData.objKinds = _objKindList;
    }

    // マテリアルのロード処理（仮）
    void WorldLoad()
    {
        MaterialBunker materialBunker = MaterialBunker.InstanceMatBunker;
        Material assignMat;
        // ロードした値(仮置き)
        int targetNum = 0;
        if(targetNum < MaterialBunker.MATERIAL_AMOUNT)
        {
            string tmpKey = materialBunker.KeyName + targetNum;
            assignMat = materialBunker.ImageMaterials[tmpKey];
        }
        else
        {
            int defaultMat = targetNum - MaterialBunker.MATERIAL_AMOUNT;
            assignMat = materialBunker.DefaultMat[defaultMat];
        }

    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
