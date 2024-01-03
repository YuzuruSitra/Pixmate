using UnityEngine;

public class ImportMaterialModeUIChanger : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField] 
    private GameObject _editCropPanel;

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
        if (newState == StateManager.GameState.ImportMaterialMode)
            OpenImportMaterialPanel();
        else
            CloseImportMaterialPanel();
    }

    void OpenImportMaterialPanel()
    {
        _editCropPanel.SetActive(true);
    }

    void CloseImportMaterialPanel()
    {
        _editCropPanel.SetActive(false);
    }
}
