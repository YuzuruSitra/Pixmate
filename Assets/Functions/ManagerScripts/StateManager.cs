using System;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum GameState
    {
        DefaultMode,
        CreateMode,
        EditMode,
        EditMatMode,
        EditMatImportMode,
        SettingsMode
    }

    public event Action<GameState> OnStateChanged;

    private GameState _currentState;

    private void Start()
    {
        ChangeState(GameState.DefaultMode);
    }

    public void ChangeState(GameState newState)
    {
        _currentState = newState;
        OnStateChanged?.Invoke(_currentState);
    }
}
