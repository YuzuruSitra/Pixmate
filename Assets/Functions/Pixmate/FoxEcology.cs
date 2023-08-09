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
    int nextActionTime;
    private float _elapsedTime = 0f;
    // 強制行動を踏んでいる時はfalseに。
    private bool _isFreedom = true;
    private bool _isGround = true;
    private float _currentPosY;
    private float _groundRayLength = 0.2f;
    bool tmp;

    private void Start()
    {
        currentState = new FoxIdoleState(); // 初期状態を待機状態に設定
        currentState.EnterState(this);
        // 最初のState遷移インターバル
        nextActionTime = UnityEngine.Random.Range(2, 8);
        SetNextAction();
    }

    private void Update()
    {
        // 接地判定
        _isGround = isGroundStay();
        Debug.Log("IsGround : " + _isGround);

        currentState.UpdateState(this);
        //ChangeState(new FoxIdoleState());

        if(_isFreedom && _isGround) _elapsedTime += Time.deltaTime;
        SetNextAction();
    }

    // AIの行動選択
    void SetNextAction()
    {
        // 指定時間に達すると処理を開始
        if(nextActionTime > _elapsedTime) return;
        _elapsedTime = 0f;
        nextActionTime = UnityEngine.Random.Range(2, 8);

        // ランダム選択を変更する余地あり
        int nextState = UnityEngine.Random.Range(0, 2);
        switch (nextState)
        {
            case 0:
                ChangeState(new FoxIdoleState());
                break;
            case 1:
                ChangeState(new FoxWalkState());
                break;
        }
    }

    // 条件から呼び出す。
    public void ChangeState(IAIState newState)
    {
        currentState.ExitState(this);
        newState.EnterState(this);
        currentState = newState;
    }


    /****************************************************/

    // 接地判定
    private bool isGroundStay()
    {
        Vector3 groundOffset = new Vector3(0, 0.1f, 0);
        Vector3 groundRayOffset = transform.forward * groundOffset.z + transform.up * groundOffset.y + transform.right * groundOffset.x;
        Vector3 groundOrigin = transform.position + groundRayOffset;
        Vector3 groundDirection = -transform.up;

        RaycastHit groundHit;
        bool isGroundHit = Physics.Raycast(groundOrigin, groundDirection, out groundHit, _groundRayLength);

        // レイの可視化
        Color rayColor = isGroundHit ? Color.green : Color.red;
        Debug.DrawRay(groundOrigin, groundDirection * _groundRayLength, rayColor);

        return isGroundHit && groundHit.distance <= _groundRayLength;
    }


}
