using UnityEngine;
using UnityEngine.UI;

// インポートモードのリスナー登録
public class ImportMaterialModeAddListener : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField]
    private ImportImageFromGallery _importImageFromGallery;
    private CroppedImageSaver _croppedImageSaver;
    [SerializeField] 
    private ImageCropper _imageCropper;

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

    void Start()
    {
        _croppedImageSaver = new CroppedImageSaver();
        _stateManager.OnStateChanged += OpenImageImport;

        // リスナー登録
        _cancelButton.onClick.AddListener(CancelImport);
        _importButton.onClick.AddListener(DoneImport);
        _reverseButton.onClick.AddListener(_imageCropper.ReverseTexture2D);
        _rotateButton.onClick.AddListener(_imageCropper.RotateTexture2D);

        _importImageFromGallery.IsImportedSuccess += SucceseImported;
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
        if(!isSuccess) return;
        _imageCropper.DoCropImage(_imageImportFlame);
    }

    private void CancelImport()
    {
        _imageCropper.FinishCrop();
        _stateManager.ChangeState(StateManager.GameState.SelectMaterialMode);
    }

    private void DoneImport()
    {
        _imageCropper.FinishCrop();
        _croppedImageSaver.ResizeToSaveSprite(_imageCropper.CroppedTexture);
        _stateManager.ChangeState(StateManager.GameState.SelectMaterialMode);
    }

}
