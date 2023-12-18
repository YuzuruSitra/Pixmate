using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxJumpState : IAIState
{
    private bool _oneTime;
    private const float DELAY_TIME = 0.3f;
    private float _progressTime;
    float _forwardPower = 0.4f;
    private float _jumpPower = 280;
    public void EnterState(FoxEcology fe)
    {
        _oneTime = true;
        _progressTime = 0f;
    }

    public void UpdateState(FoxEcology fe)
    {
        _progressTime += Time.deltaTime;
        if(_progressTime < DELAY_TIME || !_oneTime) return;
        
        Rigidbody rb = fe.GetComponent<Rigidbody>();
        Vector3 jumpDirection = (fe.transform.forward * _forwardPower) + fe.transform.up;
        Vector3 jumpForce = jumpDirection.normalized * _jumpPower * Time.fixedDeltaTime;
        rb.AddForce(jumpForce, ForceMode.Impulse);
        _oneTime = false;
    }

    public void ExitState(FoxEcology fe)
    {

    }
}
