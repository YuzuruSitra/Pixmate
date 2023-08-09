using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FoxEcology : MonoBehaviour
{
    private IAIState currentState;
    [SerializeField]
    private float _moveSpeed = 1.0f;
    public float MoveSpeed => _moveSpeed;

    private void Start()
    {
        currentState = new FoxIdoleState(); // 初期状態を待機状態に設定
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
        ChangeState(new FoxIdoleState());
        
    }

    public void ChangeState(IAIState newState)
    {
        currentState.ExitState(this);
        newState.EnterState(this);
        currentState = newState;
    }
}
