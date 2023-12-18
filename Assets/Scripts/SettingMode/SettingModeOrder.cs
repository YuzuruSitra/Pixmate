using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingModeOrder : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField]
    private Button _returnButton;
    [SerializeField]
    private Button _deletePixmateButton;
    private SaveManager _saveManager;

    // Start is called before the first frame update
    void Start()
    {
        _saveManager = SaveManager.InstanceSaveManager;
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
