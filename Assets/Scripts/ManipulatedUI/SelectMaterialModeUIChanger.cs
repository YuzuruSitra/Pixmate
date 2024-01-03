using UnityEngine;

// SelectMaterialModeのUI管理クラス
public class SelectMaterialModeUIChanger : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField] 
    private GameObject _editMatPanel;

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
        if (newState == StateManager.GameState.SelectMaterialMode)
            OpenSelectMaterialPanel();
        else
            CloseSelectMaterialPanel();
    }

    void OpenSelectMaterialPanel()
    {
        _editMatPanel.SetActive(true);
    }

    void CloseSelectMaterialPanel()
    {
        _editMatPanel.SetActive(false);
    }
}
