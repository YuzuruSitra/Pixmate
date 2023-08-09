using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxWalkState : IAIState
{
    Vector3 _targetPos = Vector3.zero;
    [SerializeField]
    private float _thresholdAngle = 5f;
    [SerializeField] 
    private float _rotationSpeed = 2.0f;

    public void EnterState(FoxEcology fe)
    {
        Debug.Log("Entering Walk State");
        // 目標座標の選定
        float targetPosX = UnityEngine.Random.Range(-1.0f, 1.0f);
        float targetPosZ = UnityEngine.Random.Range(-1.0f, 1.0f);

        _targetPos = new Vector3(targetPosX, 0.0f, targetPosZ);
    }

    public void UpdateState(FoxEcology fe)
    {
        // オブジェクトが _targetPos の方向を向いていないなら徐々に向ける
        Quaternion rotation = Quaternion.LookRotation(_targetPos - fe.transform.position, Vector3.up);
        
        if (!IsFacingTarget(fe, rotation)) // IsFacingTarget メソッドは回転を行う必要があるかどうかを判定する
        {
            fe.transform.rotation = Quaternion.Slerp(fe.transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
            return;
        }
        
        // 回転し終えていると前進
        fe.transform.position += fe.transform.forward * fe.MoveSpeed * Time.deltaTime;
        
    }

    public void ExitState(FoxEcology fe)
    {
        Debug.Log("Exiting Walk State");
        _targetPos = Vector3.zero;
    }


    /***************************************************************************/
    bool IsFacingTarget(FoxEcology fe, Quaternion targetRotation)
    {
        float angle = Quaternion.Angle(fe.transform.rotation, targetRotation);
        // thresholdAngle度のずれまで許容
        return angle < _thresholdAngle;
    }

}
