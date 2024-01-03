using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// エディットマットモードのリスナー登録
public class SelectMaterialModeAddListener : MonoBehaviour
{
    [SerializeField]
    private MaterialBunker _materialBunker;
    [SerializeField]
    private ImportImageFromGallery _importImageFromGallery;

    [SerializeField] 
    private StateManager _stateManager;

    [SerializeField] 
    private SpritePoolHandler _spritePoolHandler;

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
        if(newState != StateManager.GameState.SelectMaterialMode) return;
        // 重複ケア
        _spritePoolHandler.ReturnAllToPool(_poolObj);
        // 描画処理
        _spritePoolHandler.ShowSprites(_poolObj);
        // 選択フレームに画像をセット
        _spritePoolHandler.ShowSelectedItem(_setItemFlame, _itemNameText, _materialBunker.NowHavePhotoSprite, _materialBunker.NowHavePhotoNames);
    }

    void ReturnEditMode()
    {
        _stateManager.ChangeState(StateManager.GameState.EditMode);
    }

    void GoImportMode()
    {
        _stateManager.ChangeState(StateManager.GameState.ImportMaterialMode);
    }

    // インポートしなかった場合EditMatModeに戻す
    void CloseImporteMode(bool isSuccess)
    {
        if(isSuccess) return;
        _stateManager.ChangeState(StateManager.GameState.SelectMaterialMode);
    }

    /*---------------------------------------------*/

    // プールオブジェクトを生成格納しリスナー登録
    void SetPoolListener()
    {
        for(int i = 0;  i < _poolObj.Length; i++)
        {
            // プールの生成格納
            _poolObj[i]  = _spritePoolHandler.GeneratePool(_flamePrefab, _PoolParentObj);
            //ボタンをリスナー登録
            Button activeButton =  _poolObj[i].GetComponent<Button>();

            activeButton.onClick.AddListener(SelectFlame);
        }
    }

    // 特定のスプライトをピック
    void SelectFlame()
    {
        _materialBunker.SetNowHavePhoto(PushFlame());
        _spritePoolHandler.ShowSelectedItem(_setItemFlame, _itemNameText, _materialBunker.NowHavePhotoSprite, _materialBunker.NowHavePhotoNames);
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
        _spritePoolHandler.ReturnAllToPool(_poolObj);
        // 描画処理
        _spritePoolHandler.ShowSprites(_poolObj);
        // 選択フレームに画像をセット
        _spritePoolHandler.ShowSelectedItem(_setItemFlame, _itemNameText, _materialBunker.NowHavePhotoSprite, _materialBunker.NowHavePhotoNames);
    }

}
