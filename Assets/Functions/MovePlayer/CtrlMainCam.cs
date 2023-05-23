using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlMainCam : MonoBehaviour
{
    [SerializeField]
    private CtrlPlayer _ctrlPlayer;

    [SerializeField]
    private CtrlSightPad _ctrlSightPad;

    [SerializeField]
    private float maxLimit = 45.0f;

    [SerializeField]
    private float minLimit = -45.0f;
    
    
    // 設計見直し
    public void CtrlRotCamera(float rotY)
    {
        // 視野角度の制限
        float clampedXRotation = transform.localEulerAngles.x - rotY;
        if (clampedXRotation < -180f)clampedXRotation += 360f;
        else if (clampedXRotation > 180f)clampedXRotation -= 360f;
        clampedXRotation = Mathf.Clamp(clampedXRotation, minLimit, maxLimit);
        
        // 角度の更新処理
        Quaternion newRotation = Quaternion.Euler(clampedXRotation, 0f, 0f);
        transform.localRotation = newRotation;
    }

}
