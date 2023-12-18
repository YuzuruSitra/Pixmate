using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxWalkState : IAIState
{
    Vector3 _targetRotation = Vector3.zero;
    private float _thresholdAngle = 5.0f;
    private float _rotationSpeed = 1.0f;
    private const float WALK_MAX_SPEED = 1.0f;
    float _movingSpeed;
    // 最大速度への到達速度
    float _changePerFrame = 0.3f;

    public void EnterState(FoxEcology fe)
    {
        _movingSpeed = WALK_MAX_SPEED * 0.2f;
        Debug.Log("Entering Walk State");
        // 目標座標の選定
        _targetRotation = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
        _targetRotation.Normalize();
        
    }

    public void UpdateState(FoxEcology fe)
    {
        _movingSpeed = Mathf.MoveTowards(_movingSpeed, WALK_MAX_SPEED, _changePerFrame * Time.deltaTime);
        if(WALK_MAX_SPEED < _movingSpeed) _movingSpeed = WALK_MAX_SPEED;

        Quaternion targetQuaternion = Quaternion.LookRotation(_targetRotation, Vector3.up);
        // 前のstateが回避ではない且つ、目標角度ではない場合回転する。
        if (fe.BeforeSpecialState != fe.States["Avoid"] && Quaternion.Angle(fe.transform.rotation, targetQuaternion) > _thresholdAngle)
        {
            fe.transform.rotation = Quaternion.Slerp(fe.transform.rotation, targetQuaternion, _rotationSpeed * Time.deltaTime);
            fe.transform.position += fe.transform.forward * _movingSpeed * Time.deltaTime;
            return;
        }

        // 前進
        fe.transform.position += fe.transform.forward * _movingSpeed * Time.deltaTime;
        
    }

    public void ExitState(FoxEcology fe)
    {
        //Debug.Log("Exiting Walk State");
        _targetRotation = Vector3.zero;
    }

}
