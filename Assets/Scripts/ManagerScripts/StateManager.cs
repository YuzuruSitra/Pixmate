using System;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum GameState
    {
        DefaultMode,
        CreateMode,
        CreateItemMode,
        EditMode,
        EditMatMode,
        EditMatImportMode,
        SettingsMode
    }

    public event Action<GameState> OnStateChanged;

    private GameState _currentState = GameState.DefaultMode;
    private GameState _beforeState;
    public GameState BeforeState => _beforeState;

    private void Start()
    {
        ChangeState(GameState.DefaultMode);
    }

    public void ChangeState(GameState newState)
    {
        // 同じステートを弾く
        ////////////////
        if(_currentState == newState) return;
        _beforeState = _currentState;
        _currentState = newState;
        
        OnStateChanged?.Invoke(_currentState);
    }
    
}