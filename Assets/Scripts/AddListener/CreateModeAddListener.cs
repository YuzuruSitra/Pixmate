using UnityEngine;
using UnityEngine.UI;

// クリエイトモードのリスナー登録
public class CreateModeAddListener : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;

    [SerializeField]
    private Image _havingItemImage;
    [SerializeField]
    private Button _homeButton;
    [SerializeField]
    private Button _itemButton;
    [SerializeField]
    private Button _settingsButton;
    [SerializeField]
    private Button _createButton1;
    [SerializeField]
    private Button _createButton2;
    [SerializeField]
    private ObjInstantiater _objInstantiater;

    void Start()
    {
        _stateManager.OnStateChanged += OpenCreate;
        _homeButton.onClick.AddListener(GoDefaultMode);
        _itemButton.onClick.AddListener(GoItemMode);
        _settingsButton.onClick.AddListener(GoSettingsMode);
        _createButton1.onClick.AddListener(_objInstantiater.Generate1);
        _createButton2.onClick.AddListener(_objInstantiater.Generate2);
    }

    void OnDestroy()
    {
        _stateManager.OnStateChanged -= OpenCreate;
    }

    void OpenCreate(StateManager.GameState newState)
    {
        if(newState != StateManager.GameState.CreateMode) return;
        ItemBunker itemBunker = ItemBunker.InstanceItemBunker;
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
        _stateManager.ChangeState(StateManager.GameState.SelectItemMode);
    }
    
}
