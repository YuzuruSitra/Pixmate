using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField] 
    private GameObject _movePads;
    [SerializeField] 
    private GameObject _defaultPanel;
    [SerializeField] 
    private GameObject _createPanel;
    [SerializeField]
    private GameObject _createItemPanel;
    [SerializeField] 
    private GameObject _editPanel;
    [SerializeField] 
    private GameObject _editMatPanel;
    [SerializeField] 
    private GameObject _editCropPanel;

    private void Start()
    {
        _stateManager.OnStateChanged += UpdateUI;
        _stateManager.ChangeState(StateManager.GameState.DefaultMode);
    }

    private void OnDestroy()
    {
        _stateManager.OnStateChanged -= UpdateUI;
    }

    private void UpdateUI(StateManager.GameState newState)
    {
        switch (newState)
        {
            case StateManager.GameState.DefaultMode:
                OnDefaultUI();
                break;
            case StateManager.GameState.CreateMode:
                OnCreateUI();
                break;
            case StateManager.GameState.CreateItemMode:
                OnCreateItemUI();
                break;
            case StateManager.GameState.EditMode:
                OnEditUI();
                break;
            case StateManager.GameState.EditMatMode:
                OnEditMatUI();
                break;
            case StateManager.GameState.EditMatImportMode:
                OnEditCropUI();
                break;
            case StateManager.GameState.SettingsMode:
                OnSettingsUI();
                break;
        }
    }

    /*---------------------------------------------*/

    void OnDefaultUI()
    {
        AllInactiveUI();
        _movePads.SetActive(true);
        _defaultPanel.SetActive(true);
    }

    void OnCreateUI()
    {
        AllInactiveUI();
        _movePads.SetActive(true);
        _createPanel.SetActive(true);
    }

    void OnCreateItemUI()
    {
        AllInactiveUI();
        _createItemPanel.SetActive(true);
    }

    void OnEditUI()
    {
        AllInactiveUI();
        _movePads.SetActive(true);
        _editPanel.SetActive(true);
    }

    void OnEditMatUI()
    {
        AllInactiveUI();
        _editMatPanel.SetActive(true);
    }

    void OnEditCropUI()
    {
        AllInactiveUI();
        _editCropPanel.SetActive(true);
    }

    void OnSettingsUI()
    {
        AllInactiveUI();
    }

    // 全てのUIを非アクティブ化
    void AllInactiveUI()
    {
        _movePads.SetActive(false);
        _defaultPanel.SetActive(false);
        _createPanel.SetActive(false);
        _createItemPanel.SetActive(false);
        _editPanel.SetActive(false);
        _editMatPanel.SetActive(false);
        _editCropPanel.SetActive(false);

    }


}
