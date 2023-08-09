using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxWalkState : IAIState
{
    Vector3 _targetRotation = Vector3.zero;
    private float _thresholdAngle = 5.0f;
    private float _rotationSpeed = 1.0f;

    public void EnterState(FoxEcology fe)
    {
        //Debug.Log("Entering Walk State");
        // 目標座標の選定
        _targetRotation = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0.0f, UnityEngine.Random.Range(-1.0f, 1.0f));
        _targetRotation.Normalize();
        
    }

    public void UpdateState(FoxEcology fe)
    {
        Quaternion targetQuaternion = Quaternion.LookRotation(_targetRotation, Vector3.up);

        // 回転がほぼ完了したら直進する
        if (Quaternion.Angle(fe.transform.rotation, targetQuaternion) > _thresholdAngle)
        {
            fe.transform.rotation = Quaternion.Slerp(fe.transform.rotation, targetQuaternion, _rotationSpeed * Time.deltaTime);
            return;
        }
        
        // 回転し終えていると前進
        fe.transform.position += fe.transform.forward * fe.MoveSpeed * Time.deltaTime;
        
    }

    public void ExitState(FoxEcology fe)
    {
        //Debug.Log("Exiting Walk State");
        _targetRotation = Vector3.zero;
    }

}
