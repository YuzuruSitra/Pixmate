using UnityEngine;

public class SelectItemModeUIChanger : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    
    [SerializeField]
    private GameObject _createItemPanel;

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
        if (newState == StateManager.GameState.SelectItemMode)
            OpenSelectItemPanel();
        else
            CloseSelectItemPanel();
    }

    void OpenSelectItemPanel()
    {
        _createItemPanel.SetActive(true);
    }

    void CloseSelectItemPanel()
    {
        _createItemPanel.SetActive(false);
    }
}
