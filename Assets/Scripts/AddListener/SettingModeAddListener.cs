using UnityEngine;
using UnityEngine.UI;

// 設定モードのリスナー登録
public class SettingModeAddListener : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField]
    private Button _returnButton;
    [SerializeField]
    private Button _deletePixmateButton;
    [SerializeField]
    private SaveManager _saveManager;

    void Start()
    {
        _returnButton.onClick.AddListener(ReturnMode);
        _deletePixmateButton.onClick.AddListener(DeletePixmate);
    }

    void ReturnMode()
    {
        _stateManager.ChangeState(_stateManager.BeforeState);
    }

    void DeletePixmate()
    {
        _saveManager.DoResetPixmates();
    }

}
