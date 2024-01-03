using UnityEngine;

// SettingsModeのUI管理クラス
public class SettingsModeUIChanger : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField] 
    private GameObject _settingPanel;

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
        if (newState == StateManager.GameState.SettingsMode)
            OpenSettingsPanel();
        else
            CloseSettingsPanel();
    }

    void OpenSettingsPanel()
    {
        _settingPanel.SetActive(true);
    }

    void CloseSettingsPanel()
    {
        _settingPanel.SetActive(false);
    }
}
