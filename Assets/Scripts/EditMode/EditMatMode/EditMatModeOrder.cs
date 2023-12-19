using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class EditMatModeOrder : MonoBehaviour
{
    MaterialBunker _materialBunker;
    [SerializeField]
    private ImportImageFromGallery _importImageFromGallery;

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
    private InputField _itemNameText;

    [SerializeField]
    private Transform _PoolParentObj;

    // ボタン類
    [SerializeField]
    private Button _importButton;
    [SerializeField]
    private Button _returnButton;
    [SerializeField]
    private Button _deleteButton;

    // アクティブなプールオブジェクトを保持
    private int _materialAmount => MaterialBunker.MATERIAL_AMOUNT;
    private GameObject[] _poolObj;

    void Start()
    {
        // マテリアル管理のインスタンス
        _materialBunker = MaterialBunker.InstanceMatBunker;

        // プールオブジェクトの配列を初期化
        _poolObj = new GameObject[_materialAmount];

        _stateManager.OnStateChanged += OpenEditMat;

        // ボタンのリスナーに登録
        _importButton.onClick.AddListener(GoImportMode);
        _returnButton.onClick.AddListener(ReturnEditMode);
        _deleteButton.onClick.AddListener(DeleteSprite);
        _itemNameText.onEndEdit.AddListener(ChangePhotoNames);

        // プールの生成とリスナー登録
        SetPoolListener();
        // ImageImport add listener
        _importImageFromGallery.IsImportedSuccess += CloseImporteMode;
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
        // 選択フレームに画像をセット
        _galleryShow.ShowSelectItem(_setItemFlame, _itemNameText, _materialBunker.NowHavePhotoSprite, _materialBunker.NowHavePhotoNames);
    }

    void ReturnEditMode()
    {
        _stateManager.ChangeState(StateManager.GameState.EditMode);
    }

    void GoImportMode()
    {
        Debug.Log("PushImport");
        _stateManager.ChangeState(StateManager.GameState.EditMatImportMode);
    }

    // インポートしなかった場合EditMatModeに戻す
    void CloseImporteMode(bool isSuccess)
    {
        if(isSuccess) return;
        Debug.Log("CloseImporteMode" + isSuccess);
        _stateManager.ChangeState(StateManager.GameState.EditMatMode);
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
        _materialBunker.NowHavePhoto = PushFlame();
        _galleryShow.ShowSelectItem(_setItemFlame, _itemNameText, _materialBunker.NowHavePhotoSprite, _materialBunker.NowHavePhotoNames);
    }

    public string PushFlame()
    {
        // 選択したスプライトのキーの取得
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        string tmpKey = clickedButton.gameObject.name;

        return tmpKey;
    }

    // InputFieldにて写真の名前を変更時の処理
    void ChangePhotoNames(string receiveName) 
    {
        _materialBunker.ChangePhotoName(receiveName);
    }

    void DeleteSprite()
    {
        _materialBunker.DeleteSortDictionary();
        // 重複ケア
        _galleryShow.AllReturnPooled(_poolObj);
        // 描画処理
        _galleryShow.ShowSprits(_poolObj);
        // 選択フレームに画像をセット
        _galleryShow.ShowSelectItem(_setItemFlame, _itemNameText, _materialBunker.NowHavePhotoSprite, _materialBunker.NowHavePhotoNames);
    }

}