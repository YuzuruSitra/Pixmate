using UnityEngine;
using UnityEngine.UI;

// インポートモードのリスナー登録
public class ImportMaterialModeAddListener : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField]
    private ImportImageFromGallery _importImageFromGallery;

    [SerializeField] 
    private SaveCroppedImage _saveCroppedImage;
    [SerializeField] 
    private CropImage _cropImage;

    // クロップ対象
    [SerializeField]
    private GameObject _imageImportFlame;

    [SerializeField]
    private Button _cancelButton;
    [SerializeField]
    private Button _importButton;
    [SerializeField]
    private Button _reverseButton;
    [SerializeField]
    private Button _rotateButton;

    private bool _activeImportMode = false;
    public bool ActiveImportMode => _activeImportMode;

    void Start()
    {
        _stateManager.OnStateChanged += OpenImageImport;

        // リスナー登録
        _cancelButton.onClick.AddListener(CancelImport);
        _importButton.onClick.AddListener(DoneImport);
        _reverseButton.onClick.AddListener(_cropImage.ReverseTexture2D);
        _rotateButton.onClick.AddListener(_cropImage.RotateTexture2D);

    _importImageFromGallery.IsImportedSuccess += SucceseImported;
    }

    void Update()
    {
        // クロップ画像の入力処理
        _cropImage.CropInput(_activeImportMode);
    }

    void OnDestroy()
    {
        _stateManager.OnStateChanged -= OpenImageImport;
    }

    // ImageImportパネル展開時の処理
    void OpenImageImport(StateManager.GameState newState)
    {
        if(newState != StateManager.GameState.ImportMaterialMode) return;

        // インポート
        _importImageFromGallery.ImportImage(_imageImportFlame);

    }

    void SucceseImported(bool isSuccess)
    {
        _activeImportMode = isSuccess;
        if(!isSuccess) return;
        _cropImage.DoCropImage(_imageImportFlame);
    }

    private void CancelImport()
    {
        _activeImportMode = false;
        _stateManager.ChangeState(StateManager.GameState.SelectMaterialMode);
    }

    // ボタンで実行
    private void DoneImport()
    {
        // クロップのSpriteを保存
        _saveCroppedImage.AddCroppedSprite(_cropImage.CroppedTexture);
        _stateManager.ChangeState(StateManager.GameState.SelectMaterialMode);
    }

}
