using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAvoidState : IAIState
{
    private float _elapsedTime = 0f;
    // 回転継続時間
    private float _rotationDuration = 2.0f;
    private float _rotationSpeed = 30.0f;

    public void EnterState(FoxEcology fe)
    {
        _elapsedTime = 0f;
        // 回転方向の決定
        _rotationSpeed *= Random.value < 0.5f ? -1f : 1f;
        Debug.Log("Entering Avoid State");
    }

    public void UpdateState(FoxEcology fe)
    {
        // 前にオブジェクトがなくなるまで回転
        if(!fe.IsNoObstacle || fe.IsFrontFloorDecision)
        {
            fe.transform.rotation *= Quaternion.AngleAxis(_rotationSpeed * Time.deltaTime, Vector3.up);
            return;
        }
        
        _elapsedTime += Time.deltaTime;
        if(_rotationDuration < _elapsedTime) 
        {
            fe.transform.rotation *= Quaternion.AngleAxis(_rotationSpeed * Time.deltaTime, Vector3.up);
            return;
        }
    }

    public void ExitState(FoxEcology fe)
    {
        //Debug.Log("Exiting Wait State");
    }
}
