using System;
using UnityEngine;

// ゲームのステートを保持するクラス
public class StateManager : MonoBehaviour
{
    public enum GameState
    {
        DefaultMode,
        CreateMode,
        SelectItemMode,
        EditMode,
        SelectMaterialMode,
        ImportMaterialMode,
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
