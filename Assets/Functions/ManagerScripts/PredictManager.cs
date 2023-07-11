using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictManager : MonoBehaviour
{
    // 他スクリプトでも呼べるようにインスタンス化
    public static PredictManager InstancePredictManager;

    [SerializeField] 
    private StateManager _stateManager;

    private bool _inLange;
    public bool InLange => _inLange;

    // オブジェクト回転中
    public bool IsRotate = false;
    private bool _isRotate => IsRotate;
    
    [SerializeField]
    private GameObject _predictAdjCube;
    private bool _isAdjVisible;
    public Vector3 AdjCubePos => _predictAdjCube.transform.position;
    [SerializeField]
    private GameObject _predictSameCube;
    [SerializeField]
    private GameObject _predictSameSurfaceCube;
    private bool _isSameVisible;
    public Vector3 SameCubePos => _predictSameCube.transform.position;
    
    // 0_Cube 1_HalfCube 2_Step 3_SmallCube
    [SerializeField]
    private Mesh[] _predictSurfeceMesh = new Mesh[4];
    [SerializeField]
    private Mesh[] _predictFlameMesh = new Mesh[4];

    private string _currentObj;
    private string _nowHaveCubeTag;
    private GameObject _nowHaveCube;
    public  GameObject NowHaveCube => _nowHaveCube;
    private string _targetObjTag;

    void Awake()
    {
        if (InstancePredictManager == null)
        {
            InstancePredictManager = this;
        }
    }

    void Start()
    {
        _stateManager.OnStateChanged += SwitchPredictObj;
    }

    private void OnDestroy()
    {
        _stateManager.OnStateChanged -= SwitchPredictObj;
    }

    private void SwitchPredictObj(StateManager.GameState newState)
    {
        switch (newState)
        {
            case StateManager.GameState.DefaultMode:
                _isAdjVisible = false;
                _isSameVisible = false;
                _predictAdjCube.SetActive(false);
                _predictSameCube.SetActive(false);
                break;
            case StateManager.GameState.CreateMode:
                CreatePredict();
                break;
            case StateManager.GameState.EditMode:
                _isAdjVisible = false;
                _isSameVisible = true;
                break;
        }
    }

    void Update()
    {
        CtrlPredictObj();

        // 対象と重ねる予測線の更新
        if(_nowHaveCube == null) return;
        if(_currentObj == _nowHaveCubeTag) return;
        EditPredictMeshChange(_nowHaveCubeTag);
        _currentObj = _nowHaveCubeTag;
    }

    //Trnsform変更
    private void CtrlPredictObj()
    {
        if(!_isAdjVisible && !_isSameVisible)return;
        // 予測オブジェクトの座標切り替え
        MovePredictCube(_predictAdjCube, _predictSameCube);
        // 表示切り替え
        _predictAdjCube.SetActive(_inLange && _isAdjVisible && !_isRotate);
        _predictSameCube.SetActive(_inLange && _isSameVisible && !_isRotate);
    }

    // 隣接オブジェクトへのRay
    void MovePredictCube(GameObject predictAdjCube, GameObject predictSameCube)
    {
        // 画面中央の平面座標を取得する
        Vector2 displayCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        // カメラからのレイを画面中央の平面座標から飛ばす
        Ray ray = Camera.main.ScreenPointToRay(displayCenter);
        // 当たったオブジェクト情報を格納する変数
        RaycastHit hit;

        _inLange = false;
        // 対象レイヤーの指定
        int targetLayerMask = LayerMask.GetMask("TouchLayer");
        // Physics.Raycast() でレイを飛ばす
        if (Physics.Raycast(ray, out hit, 3f, targetLayerMask)) 
        {   
            _nowHaveCube = hit.collider.gameObject;
            _nowHaveCubeTag = _nowHaveCube.tag;
            // 生成位置の変数の値を「ブロックの向き + ブロックの位置」
            predictAdjCube.transform.position = hit.normal + hit.collider.transform.position;
            // EditPredict
            predictSameCube.transform.position = hit.collider.transform.position;
            
            // 対象の角度に合わせて予測線を回転
            _predictSameCube.transform.rotation = _nowHaveCube.transform.rotation;
            
            _inLange = true;
        }
    }

    void CreatePredict()
    {   
        // Meshをインスタンス化
        Mesh flameMesh = _predictAdjCube.GetComponent<MeshFilter>().mesh;
        if(flameMesh == null) return;

        ItemBunker itemBunker = ItemBunker.InstanceItemBunker;
        switch(itemBunker.NowHaveItem)
        {
            case "Cube":
                _isAdjVisible = true;
                _isSameVisible = false;
                
                ChangeMesh(flameMesh,_predictFlameMesh[0]);
                break;
            case "HalfCube":
                _isAdjVisible = true;
                _isSameVisible = false;

                ChangeMesh(flameMesh,_predictFlameMesh[1]);
                break;
            case "Step":
                _isAdjVisible = true;
                _isSameVisible = false;

                ChangeMesh(flameMesh,_predictFlameMesh[2]);
                break;
            case "SmallCube":
                _isAdjVisible = true;
                _isSameVisible = false;

                ChangeMesh(flameMesh,_predictFlameMesh[3]);
                break;
            case "Gene":
                _isAdjVisible = false;
                _isSameVisible = true;
                break;
        }
    }

    // 対象のメッシュが変わった時
    void EditPredictMeshChange(string targetObj)
    {
        Mesh flameMesh = _predictSameCube.GetComponent<MeshFilter>().mesh;
        Mesh surfaceMesh = _predictSameSurfaceCube.GetComponent<MeshFilter>().mesh;
        if(flameMesh == null || surfaceMesh == null) return;
        //対象のオブジェクトを散策
        switch(targetObj)
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