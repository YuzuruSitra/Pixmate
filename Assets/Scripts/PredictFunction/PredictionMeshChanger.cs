using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 予測線の形を担当するクラス
public class PredictionMeshChanger : MonoBehaviour
{
    [SerializeField]
    private ItemBunker _itemBunker;
    [SerializeField]
    private PredictionAdjuster _predictionAdjuster;

    [SerializeField]
    private GameObject _predictAdjCube;
    [SerializeField]
    private GameObject _predictSameCube;
    // 0_Cube 1_HalfCube 2_Step 3_SmallCube
    [SerializeField]
    private Mesh[] _predictSurfeceMesh = new Mesh[4];
    [SerializeField]
    private Mesh[] _predictFlameMesh = new Mesh[4];
    [SerializeField]
    private GameObject _predictSameSurfaceCube;

    void Start()
    {
        _itemBunker.OnItemChanged += CreateObjPredict;
    }
    private void OnDestroy()
    {
        _itemBunker.OnItemChanged -= CreateObjPredict;
    }

    void CreateObjPredict(string currentItem)
    {   
        // Meshをインスタンス化
        Mesh flameMesh = _predictAdjCube.GetComponent<MeshFilter>().mesh;
        if(flameMesh == null) return;

        ItemBunker itemBunker = ItemBunker.InstanceItemBunker;
        switch(currentItem)
        {
            case "Cube":
                ChangeMesh(flameMesh,_predictFlameMesh[0]);
                break;
            case "HalfCube":
                ChangeMesh(flameMesh,_predictFlameMesh[1]);
                break;
            case "Step":
                ChangeMesh(flameMesh,_predictFlameMesh[2]);
                break;
            case "SmallCube":
                ChangeMesh(flameMesh,_predictFlameMesh[3]);
                break;
            default:
                break;
        }
    }

    // 対象のメッシュが変わった時
    void EditPredictMeshChange()
    {
        Mesh flameMesh = _predictSameCube.GetComponent<MeshFilter>().mesh;
        Mesh surfaceMesh = _predictSameSurfaceCube.GetComponent<MeshFilter>().mesh;
        if(flameMesh == null || surfaceMesh == null) return;
        //対象のオブジェクトを散策
        switch(_predictionAdjuster.NowHaveCube.tag)
        {
            case "Cube":
                ChangeMesh(flameMesh,_predictFlameMesh[0]);
                ChangeMesh(surfaceMesh,_predictSurfeceMesh[0]);
                break;
            case "HalfCube":
                ChangeMesh(flameMesh,_predictFlameMesh[1]);
                ChangeMesh(surfaceMesh,_predictSurfeceMesh[1]);
                break;
            case "Step":
                ChangeMesh(flameMesh,_predictFlameMesh[2]);
                ChangeMesh(surfaceMesh,_predictSurfeceMesh[2]);
                break;
            case "SmallCube":
                ChangeMesh(flameMesh,_predictFlameMesh[3]);
                ChangeMesh(surfaceMesh,_predictSurfeceMesh[3]);
                break;
            default:
                break;
        }
        
    }

    // 予測線オブジェクトのフレームとサーフェイスのメッシュ変更
    private void ChangeMesh(Mesh receiveMesh,Mesh tosObj)
    {
        receiveMesh.Clear();
        receiveMesh.vertices = tosObj.vertices;
        receiveMesh.triangles = tosObj.triangles;
        receiveMesh.uv = tosObj.uv;
        receiveMesh.RecalculateBounds();
        receiveMesh.RecalculateNormals();
    }
}
