using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditModeOrder : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;

    private AssignMaterial _assignMaterial = new AssignMaterial();
    [SerializeField] 
    private RotateObject _rotateObject;

    
    [SerializeField]
    private Image _assignMatImage;

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
    private Button _rotUpwardsButton;

    [SerializeField]
    private Button _assignMatButton;


    void Start()
    {
        // ボタンのリスナーに登録
        _settingsButton.onClick.AddListener(GoSettingsMode);
        _homeButton.onClick.AddListener(GoDefaultMode);
        _setMatButton.onClick.AddListener(GoSetMatMode);
        _rotLeftButton.onClick.AddListener(_rotateObject.SpinningLeft);
        _rotUpwardsButton.onClick.AddListener(_rotateObject.SpinningUpwards);
        _assignMatButton.onClick.AddListener(_assignMaterial.DoAssignMat);
        _stateManager.OnStateChanged += OpenEdit;
    }

    void OnDestroy()
    {
        _stateManager.OnStateChanged -= OpenEdit;
    }

    void OpenEdit(StateManager.GameState newState)
    {
        if(newState != StateManager.GameState.EditMode) return;
        _assignMatImage.sprite = MaterialBunker.InstanceMatBunker.NowHavePhotoSprite;
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
