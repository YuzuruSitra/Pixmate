using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreateModeOrder : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;

    [SerializeField]
    private Image _havingItemImage;

    void Start()
    {
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
        _havingItemImage.sprite = itemBunker.NowHaveItemSprite;
    }

    public void GoSettingsMode()
    {
        _stateManager.ChangeState(StateManager.GameState.SettingsMode);
    }

    public void GoDefaultMode()
    {
        _stateManager.ChangeState(StateManager.GameState.DefaultMode);
    }

    public void GoItemMode()
    {
        _stateManager.ChangeState(StateManager.GameState.CreateItemMode);
    }
    
}
