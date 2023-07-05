using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportModeOrder : MonoBehaviour
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
        //_reverseButton.onClick.AddListener(ReturnEditMode);
        //_rotateButton.onClick.AddListener(ReturnEditMode);
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
        if(newState != StateManager.GameState.EditMatImportMode) return;

        // インポート
        _importImageFromGallery.ImportImage(_imageImportFlame);
        _activeImportMode = _importImageFromGallery.ImportSuccess;
        // クロップ
        if(!_activeImportMode)return;

        _cropImage.DoCropImage(_imageImportFlame);
    }

    private void CancelImport()
    {
        _activeImportMode = false;
        _stateManager.ChangeState(StateManager.GameState.EditMatMode);
    }

    // ボタンで実行
    private void DoneImport()
    {
        // クロップのSpriteを保存
        _saveCroppedImage.AddCroppedSprite(_cropImage.CroppedTexture);
        _stateManager.ChangeState(StateManager.GameState.EditMatMode);
    }

}
