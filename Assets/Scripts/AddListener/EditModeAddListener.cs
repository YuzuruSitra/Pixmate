using UnityEngine;
using UnityEngine.UI;

// 編集モードのリスナー登録
public class EditModeAddListener : MonoBehaviour
{
    // ワールドデータの保存
    [SerializeField]
    WorldManager _worldManager;
    [SerializeField]
    private ObjectManipulator _objectManipulator;
    [SerializeField]
    private PredictionAdjuster _predictionAdjuster;
    [SerializeField] 
    private StateManager _stateManager;

    private MaterialAssigner _materialAssigner;
    [SerializeField] 
    private RotateObject _rotateObject;

    
    [SerializeField]
    private Image _assignMatImage;

    // EditModeのボタン
    [SerializeField]
    private Button _settingsButton;

    [SerializeField]
    private Button _homeButton;

    [SerializeField]
    private Button _setMatButton;

    [SerializeField]
    private Button _rotLeftButton;

    [SerializeField]
    private Button _rotUpwardsButton;

    [SerializeField]
    private Button _assignMatButton;

    void Start()
    {
        _materialAssigner = new MaterialAssigner(_objectManipulator, _predictionAdjuster);
        // ボタンのリスナーに登録
        _settingsButton.onClick.AddListener(GoSettingsMode);
        _homeButton.onClick.AddListener(GoDefaultMode);
        _setMatButton.onClick.AddListener(GoSetMatMode);
        _rotLeftButton.onClick.AddListener(_rotateObject.SpinningLeft);
        _rotUpwardsButton.onClick.AddListener(_rotateObject.SpinningUpwards);
        _assignMatButton.onClick.AddListener(DoAssignMatSaving);
        _stateManager.OnStateChanged += OpenEdit;
    }

    void OnDestroy()
    {
        _stateManager.OnStateChanged -= OpenEdit;
    }

    void OpenEdit(StateManager.GameState newState)
    {
        if(newState != StateManager.GameState.EditMode) return;
        
        MaterialBunker instanceMatBunker = MaterialBunker.InstanceMatBunker;
        _assignMatImage.sprite = instanceMatBunker.NowHavePhotoSprite;
    }

    void GoSettingsMode()
    {
        _stateManager.ChangeState(StateManager.GameState.SettingsMode);
    }

    void GoDefaultMode()
    {
        _stateManager.ChangeState(StateManager.GameState.DefaultMode);
    }

    void GoSetMatMode()
    {
        _stateManager.ChangeState(StateManager.GameState.SelectMaterialMode);
    }

    void returnEditMode()
    {
        _stateManager.ChangeState(StateManager.GameState.EditMode);
    }

    void DoAssignMatSaving()
    {
        GameObject changeObj = _materialAssigner.DoAssignMat();
        if (changeObj == null) return;
        _worldManager.ChangeObjSaving(changeObj);
    }
}
