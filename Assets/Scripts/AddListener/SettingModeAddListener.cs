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
    private MaterialIO _materialIO;
    [SerializeField]
    private PixmateIO _pixmateIO;
    [SerializeField]
    private WorldIO _worldIO;

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
        //_materialIO.DeleteDate();
        _pixmateIO.DeleteData();
        //_worldIO.DeleteData();
    }

}
