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
    private bool _isSameVisible;
    public Vector3 SameCubePos => _predictSameCube.transform.position;
    
    private GameObject _nowHaveCube;
    public GameObject NowHaveCube => _nowHaveCube;

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
                EditPredict();
                break;
        }
    }

    void Update()
    {
        CtrlPredictObj();
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
            // 生成位置の変数の値を「ブロックの向き + ブロックの位置」
            predictAdjCube.transform.position = hit.normal + hit.collider.transform.position;
            predictSameCube.transform.position = hit.collider.transform.position;
            _inLange = true;
        }
    }

    void CreatePredict()
    {   
        ItemBunker itemBunker = ItemBunker.InstanceItemBunker;
        switch(itemBunker.NowHaveItem)
        {
            case "Cube":
                _isAdjVisible = true;
                _isSameVisible = false;
                break;
            case "Gene":
                _isAdjVisible = false;
                _isSameVisible = true;
                break;
        }
    }

    void EditPredict()
    {
        _isAdjVisible = false;
        _isSameVisible = true;
    }

}

