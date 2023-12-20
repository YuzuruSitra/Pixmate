using UnityEngine;
using UnityEngine.UI;

// デフォルトモードのリスナー登録
public class DefaultModeAddListener : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    
    [SerializeField]
    private Button _settingsButton;

    [SerializeField]
    private Button _createButton;

    [SerializeField]
    private Button _editButton;

    void Start()
    {
        // ボタンのリスナー登録
        _settingsButton.onClick.AddListener(GoSettingsMode);
        _createButton.onClick.AddListener(GoCreateMode);
        _editButton.onClick.AddListener(GoEditMode);
    }

    void GoSettingsMode()
    {
        _stateManager.ChangeState(StateManager.GameState.SettingsMode);
    }

    void GoCreateMode()
    {
        _stateManager.ChangeState(StateManager.GameState.CreateMode);
    }

    void GoEditMode()
    {
        _stateManager.ChangeState(StateManager.GameState.EditMode);
    }

}
