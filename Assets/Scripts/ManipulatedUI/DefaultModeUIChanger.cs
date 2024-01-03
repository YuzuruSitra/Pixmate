using UnityEngine;

// DefaultModeのパネル制御
public class DefaultModeUIChanger : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField] 
    private GameObject _defaultPanel;

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
        if (newState == StateManager.GameState.DefaultMode)
            OpenDefaultPanel();
        else
            CloseDefaultPanel();
    }

    void OpenDefaultPanel()
    {
        _defaultPanel.SetActive(true);
    }

    void CloseDefaultPanel()
    {
        _defaultPanel.SetActive(false);
    }
}
