using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreateModeOrder : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField]
    private ObjInstantiater _objInstantiater;


    [SerializeField]
    private Image _havingItemImage;

    // ブロック生成用
    [SerializeField]
    private GameObject _blockPrefab;
    // [SerializeField]
    // private GameObject _predictInsCube;
    // [SerializeField]
    // private GameObject _predictMatCube;
    
    [SerializeField]
    private Button _settingsButton;

    [SerializeField]
    private Button _homeButton;

    [SerializeField]
    private Button _ItemButton;

    [SerializeField]
    private Button _putCubeButton;


    void Start()
    {

        // ボタンのリスナー登録
        _settingsButton.onClick.AddListener(GoSettingsMode);
        _homeButton.onClick.AddListener(GoDefaultMode);
        _ItemButton.onClick.AddListener(GoItemMode);

        // キューブの生成
        _putCubeButton.onClick.AddListener(DoGenerate);
        _stateManager.OnStateChanged += OpenCreate;
    }

    void OnDestroy()
    {
        _stateManager.OnStateChanged -= OpenCreate;
    }

    void OpenCreate(StateManager.GameState newState)
    {
        if(newState != StateManager.GameState.CreateMode) return;
        ItemBunker itemBunker = ItemBunker.InstanceItemBunker;
        string nowHaveItem = itemBunker.NowHaveItem;
        _havingItemImage.sprite = itemBunker.NowHaveItemSprite;
    }

    void GoSettingsMode()
    {
        _stateManager.ChangeState(StateManager.GameState.SettingsMode);
    }

    void GoDefaultMode()
    {
        _stateManager.ChangeState(StateManager.GameState.DefaultMode);
    }

    void GoItemMode()
    {
        _stateManager.ChangeState(StateManager.GameState.CreateItemMode);
    }

    void DoGenerate()
    {
        _objInstantiater.GenerateCube();
    }

    
}
