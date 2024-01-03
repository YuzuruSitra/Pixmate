using UnityEngine;

public class EditModeUIChanger : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField] 
    private GameObject _editPanel;

    void Start()
    {
        _stateManager.OnStateChanged += ChangePanel;
    }

    void OnDestroy()
    {
        _stateManager.OnStateChanged -= ChangePanel;
    }

    void ChangePanel(StateManager.GameState newState)
    {
        if (newState == StateManager.GameState.EditMode)
            OpenEditPanel();
        else
            CloseEditPanel();
    }

    void OpenEditPanel()
    {
        _editPanel.SetActive(true);
    }

    void CloseEditPanel()
    {
        _editPanel.SetActive(false);
    }
}
