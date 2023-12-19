using System.Collections;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTransform;
    private Coroutine _spinCoroutine;
    private PredictManager _predictManager;
    [Header("回転にかける時間")]
    [SerializeField]
    private float _rotationDuration = 1f;

    void Start()
    {
        _predictManager = PredictManager.InstancePredictManager;
    }

    public void SpinningLeft()
    {
        if (_spinCoroutine != null || _predictManager == null) return;

        GameObject targetObj = _predictManager.NowHaveCube;
        if (targetObj == null) return;

        Quaternion startRotation = GetNormalizedRotation(targetObj.transform.eulerAngles);
        Quaternion targetRotation = Quaternion.Euler(0f, 90f, 0f) * startRotation;

        StartRotationCoroutine(startRotation, targetRotation, targetObj.transform);
    }

    public void SpinningUpwards()
    {
        if (_spinCoroutine != null || _predictManager == null) return;

        GameObject targetObj = _predictManager.NowHaveCube;
        if (targetObj == null) return;

        Quaternion startRotation = GetNormalizedRotation(targetObj.transform.eulerAngles);
        Quaternion targetRotation = Quaternion.Euler(ConvertUpwards(targetObj.transform)) * startRotation;

        StartRotationCoroutine(startRotation, targetRotation, targetObj.transform);
    }

    private void StartRotationCoroutine(Quaternion startRotation, Quaternion targetRotation, Transform targetTransform)
    {
        _spinCoroutine = StartCoroutine(DoRotation(startRotation, targetRotation, targetTransform));
        // ワールドデータの保存
        WorldManager.InstanceWorldManager.ChangeObjSaving(targetTransform.gameObject);
    }

    IEnumerator DoRotation(Quaternion startRotation, Quaternion targetRotation, Transform targetTransform)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _rotationDuration)
        {
            targetTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / _rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetTransform.rotation = targetRotation;
        // 予測線の表示
        _predictManager.IsRotate = false;
        _spinCoroutine = null;
    }

    private Quaternion GetNormalizedRotation(Vector3 eulerAngles)
    {
        float normalizedAngleX = Mathf.Repeat(eulerAngles.x + 180, 360) - 180;
        float normalizedAngleY = Mathf.Repeat(eulerAngles.y + 180, 360) - 180;
        float normalizedAngleZ = Mathf.Repeat(eulerAngles.z + 180, 360) - 180;
        return Quaternion.Euler(normalizedAngleX, normalizedAngleY, normalizedAngleZ);
    }

    Vector3 ConvertUpwards(Transform targetTransform)
    {
        Vector3 relativePosition = _cameraTransform.position - targetTransform.position;
        float distanceX = relativePosition.x;
        float distanceZ = relativePosition.z;

        Vector3 resultValue = Vector3.zero;
        if (distanceX >= 0 && distanceZ >= 0)
            resultValue = new Vector3(-90f, 0f, 0f);
        else if (distanceX < 0 && distanceZ >= 0)
            resultValue = new Vector3(0f, 0f, -90f);
        else if (distanceX < 0 && distanceZ < 0)
            resultValue = new Vector3(90f, 0f, 0f);
        else if (distanceX >= 0 && distanceZ < 0)
            resultValue = new Vector3(0f, 0f, 90f);
        return resultValue;
    }
}
