using UnityEngine;

// カメラの操作を担当しているクラス
public class CtrlMainCam : MonoBehaviour
{
    [Header("回転範囲の上限値")]
    [SerializeField]
    private float maxLimit = 45.0f;

    [Header("回転範囲の下限値")]
    [SerializeField]
    private float minLimit = -45.0f;
    
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
