using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxJumpState : IAIState
{
    float _forwardPower = 0.25f;
    private float _jumpPower = 250;
    public void EnterState(FoxEcology fe)
    {
        Rigidbody rb = fe.GetComponent<Rigidbody>();
        Vector3 jumpDirection = (fe.transform.forward * _forwardPower) + fe.transform.up;
        Vector3 jumpForce = jumpDirection.normalized * _jumpPower * Time.fixedDeltaTime;
        rb.AddForce(jumpForce, ForceMode.Impulse);
    }

    public void UpdateState(FoxEcology fe)
    {
        // 待機中の振る舞いを実装
        
    }

    public void ExitState(FoxEcology fe)
    {
        //Debug.Log("Exiting Wait State");
    }
}
