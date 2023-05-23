using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditMatModeOrder : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;

    [SerializeField] 
    private ImportModeOrder _importModeOrder;

    [SerializeField] 
    private GalleryShow _galleryShow;

    // ビュー用
    [SerializeField] 
    private GameObject _flamePrefab;

    [SerializeField] 
    private GameObject _setItemFlame;

    [SerializeField]
    private Transform _PoolParentObj;

    [SerializeField]
    private Image _assignMatImage;

    // ボタン類
    [SerializeField]
    private Button _importButton;
    [SerializeField]
    private Button _returnButton;

    // アクティブなプールオブジェクトを保持
    private int _materialAmount => MaterialBunker.MATERIAL_AMOUNT;
    private GameObject[] _poolObj;


    void Start()
    {
        // プールオブジェクトの配列を初期化
        _poolObj = new GameObject[_materialAmount];

        _stateManager.OnStateChanged += OpenEditMat;

        // ボタンのリスナーに登録
        _importButton.onClick.AddListener(GoImportMode);
        _returnButton.onClick.AddListener(ReturnEditMode);

        // プールの生成とリスナー登録
        SetPoolListener();
    }

    void OnDestroy()
    {
        _stateManager.OnStateChanged -= OpenEditMat;
    }

    // OpenEditMatパネル展開時の処理
    void OpenEditMat(StateManager.GameState newState)
    {
        if(newState != StateManager.GameState.EditMatMode) return;
        // 重複ケア
        _galleryShow.AllReturnPooled(_poolObj);
        // 描画処理
        _galleryShow.ShowSprits(_poolObj);
    }

    void ReturnEditMode()
    {
        _assignMatImage.sprite = MaterialBunker.InstanceMatBunker.NowHaveSprite;
        _stateManager.ChangeState(StateManager.GameState.EditMode);
    }

    void GoImportMode()
    {
        _stateManager.ChangeState(StateManager.GameState.EditMatImportMode);
        // インポートしなかった場合EditMatModeに戻す
        if(!_importModeOrder.ActiveImportMode)_stateManager.ChangeState(StateManager.GameState.EditMatMode);
    }

    /*---------------------------------------------*/

    // プールオブジェクトを生成格納しリスナー登録
    void SetPoolListener()
    {
        for(int i = 0;  i < _poolObj.Length; i++)
        {
            // プールの生成格納
            _poolObj[i]  = _galleryShow.GeneratePool(_flamePrefab, _PoolParentObj);
            //ボタンをリスナー登録
            Button activeButton =  _poolObj[i].GetComponent<Button>();

            activeButton.onClick.AddListener(SelectFlame);
        }
    }

    // 特定のスプライトをピック
    void SelectFlame()
    {
        MaterialBunker materialBunker = MaterialBunker.InstanceMatBunker;
        string getKey = PushFlame();
        Sprite selectSprite = materialBunker.CroppedImages[getKey];
        materialBunker.NowHaveSprite = selectSprite;
        _galleryShow.ShowSelectItem(_setItemFlame, selectSprite);
        Material selectMaterial = materialBunker.ImageMaterials[getKey];
        
       materialBunker.NowHaveMaterial = selectMaterial;
    }

    public string PushFlame()
    {
        // 選択したスプライトのキーの取得
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        string tmpKey = clickedButton.gameObject.name;

        return tmpKey;
    }
}
