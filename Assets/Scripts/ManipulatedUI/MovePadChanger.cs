using UnityEngine;

// MovePadの表示切り替え管理クラス
public class MovePadChanger : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField] 
    private GameObject _movePad;

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
        switch (newState)
        {
            case StateManager.GameState.DefaultMode:
            case StateManager.GameState.CreateMode:
            case StateManager.GameState.EditMode:
                OpenMovePad();
                break;
            default:
                CloseMovePad();
                break;
            
        }
    }

    void OpenMovePad()
    {
        _movePad.SetActive(true);
    }

    void CloseMovePad()
    {
        _movePad.SetActive(false);
    }
}
