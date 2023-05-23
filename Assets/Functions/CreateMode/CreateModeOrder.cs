using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateModeOrder : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField] 
    private BlockInstantiater _blockInstantiater;

    // ブロック生成用
    [SerializeField]
    private GameObject _blockPrefab;
    [SerializeField]
    private GameObject _predictInsCube;
    [SerializeField]
    private GameObject _predictMatCube;
    
    [SerializeField]
    private Button _settingsButton;

    [SerializeField]
    private Button _homeButton;

    [SerializeField]
    private Button _putCubeButton;

    void Start()
    {
        // ボタンのリスナー登録
        _settingsButton.onClick.AddListener(GoSettingsMode);
        _homeButton.onClick.AddListener(GoDefaultMode);

        // キューブの生成
        _putCubeButton.onClick.AddListener(DoGenerate);
    }

    void Update()
    {
        _blockInstantiater.OutRay(_predictInsCube, _predictMatCube);
    }

    void GoSettingsMode()
    {
        _stateManager.ChangeState(StateManager.GameState.SettingsMode);
    }

    void GoDefaultMode()
    {
        _stateManager.ChangeState(StateManager.GameState.DefaultMode);
    }

    void DoGenerate()
    {
        _blockInstantiater.GenerateCube(_blockPrefab);
    }
}
