using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditModeOrder : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;

    [SerializeField] 
    private  AssignMaterial _assignMaterial;

    
    // EditModeのボタン
    [SerializeField]
    private Button _settingsButton;

    [SerializeField]
    private Button _homeButton;

    [SerializeField]
    private Button _setMatButton;

    [SerializeField]
    private Button _rotLeftButton;

    [SerializeField]
    private Button _rotRightButton;

    [SerializeField]
    private Button _assignMatButton;


    void Start()
    {
        // ボタンのリスナーに登録
        _settingsButton.onClick.AddListener(GoSettingsMode);
        _homeButton.onClick.AddListener(GoDefaultMode);
        _setMatButton.onClick.AddListener(GoSetMatMode);
        //_rotLeftButton.onClick.AddListener(PutCube);
        //_rotRightButton.onClick.AddListener(PutCube);
        _assignMatButton.onClick.AddListener(_assignMaterial.DoAssignMat);
    }

    void GoSettingsMode()
    {
        _stateManager.ChangeState(StateManager.GameState.SettingsMode);
    }

    void GoDefaultMode()
    {
        _stateManager.ChangeState(StateManager.GameState.DefaultMode);
    }

    void GoSetMatMode()
    {
        _stateManager.ChangeState(StateManager.GameState.EditMatMode);
    }

    void returnEditMode()
    {
        _stateManager.ChangeState(StateManager.GameState.EditMode);
    }
}
