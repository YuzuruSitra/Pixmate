using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    private Transform _camPos;
    private Coroutine _spinCoroutine;
    public Coroutine SpinCoroutine => _spinCoroutine;
    private enum _spinState
    {
        LEFT,
        UPWARDS
    };

    private float _rotationDuration = 1f;

    void Update()
    {
        // PredictManager _predictManager = PredictManager.InstancePredictManager;
        // GameObject targetObj = _predictManager.NowHaveCube;
        // if(targetObj == null)return;
        // // オブジェクトAの位置からオブジェクトBの位置を引いて、ローカル座標を取得
        // Vector3 relativePosition = _camPos.position - targetObj.transform.position;

        // // X軸方向の距離（相対座標のX成分）を取得
        // float distanceX = relativePosition.x;

        // // Z軸方向の距離（相対座標のZ成分）を取得
        // float distanceZ = relativePosition.z;

        // // X軸方向とZ軸方向の距離を比較してオブジェクトAがどのエリアに属するか判定
        // if (distanceX >= 0 && distanceZ >= 0)
        // {
        //     Debug.Log("ObjectAはエリア1に属しています");
        // }
        // else if (distanceX < 0 && distanceZ >= 0)
        // {
        //     Debug.Log("ObjectAはエリア2に属しています");
        // }
        // else if (distanceX < 0 && distanceZ < 0)
        // {
        //     Debug.Log("ObjectAはエリア3に属しています");
        // }
        // else if (distanceX >= 0 && distanceZ < 0)
        // {
        //     Debug.Log("ObjectAはエリア4に属しています");
        // }
    }

    public void SpinningLeft()
    {
        if (_spinCoroutine != null) return;
        PredictManager _predictManager = PredictManager.InstancePredictManager;
        GameObject targetObj = _predictManager.NowHaveCube;
        // 対象オブジェクトと回転方向をメソッドに渡す
        _spinCoroutine = StartCoroutine(DoRotation(_spinState.LEFT, targetObj));
    }

    public void SpinningUpwards()
    {
        if (_spinCoroutine != null) return;
        PredictManager _predictManager = PredictManager.InstancePredictManager;
        GameObject targetObj = _predictManager.NowHaveCube;
        // 対象オブジェクトと回転方向をメソッドに渡す
        _spinCoroutine = StartCoroutine(DoRotation(_spinState.UPWARDS, targetObj));
    }

    IEnumerator DoRotation(_spinState state, GameObject targetObj)
    {   
        // オイラー角を正規化して格納
        float normalizedAngleX = Mathf.Repeat(targetObj.transform.eulerAngles.x + 180, 360) - 180;
        float normalizedAngleY = Mathf.Repeat(targetObj.transform.eulerAngles.y + 180, 360) - 180;
        float normalizedAngleZ = Mathf.Repeat(targetObj.transform.eulerAngles.z + 180, 360) - 180;
        Vector3 targetObjRot = new Vector3(normalizedAngleX,normalizedAngleY,normalizedAngleZ);
        //targetObj.transform.Rotate(targetObjRot);

        Quaternion startRotation = Quaternion.Euler(targetObjRot);
        Quaternion targetRotation = Quaternion.identity;

        switch (state)
        {
            case _spinState.LEFT:
                targetRotation = Quaternion.Euler(0f, 90f, 0f) * startRotation;
                break;

            case _spinState.UPWARDS:
                Vector3 convertUpwards = ConvertUpwards(targetObj.transform);
                targetRotation = Quaternion.Euler(convertUpwards) * startRotation;
                break;
        }

        float elapsedTime = 0f;
        // 1秒かけて回転する
        while (elapsedTime < _rotationDuration)
        {
            // 回転処理
            targetObj.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / _rotationDuration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 正確な目標角度に設定する（誤差修正）
        targetObj.transform.rotation = targetRotation;
        _spinCoroutine = null;
    }

    /*-------------------------------------------------------*/

    // 縦回転の遷移方向を変換して返す処理
    //---判定の取り方に改良の余地あり。---//
    Vector3 ConvertUpwards(Transform targetPos)
    {
        // オブジェクトAの位置からオブジェクトBの位置を引いて、ローカル座標を取得
        Vector3 relativePosition = _camPos.position - targetPos.position;

        // X軸方向の距離（相対座標のX成分）を取得
        float distanceX = relativePosition.x;

        // Z軸方向の距離（相対座標のZ成分）を取得
        float distanceZ = relativePosition.z;

        Vector3 resultValue = Vector3.zero;
        // X軸方向とZ軸方向の距離を比較してオブジェクトAがどのエリアに属するか判定
        if (distanceX >= 0 && distanceZ >= 0)
        {
            Debug.Log("ObjectAはエリア1に属しています");
            resultValue = new Vector3(-90f, 0f, 0f);
        }
        else if (distanceX < 0 && distanceZ >= 0)
        {
            Debug.Log("ObjectAはエリア2に属しています");
            resultValue = new Vector3(0f, 0f, -90f);
        }
        else if (distanceX < 0 && distanceZ < 0)
        {
            Debug.Log("ObjectAはエリア3に属しています");
            resultValue = new Vector3(90f, 0f, 0f);
        }
        else if (distanceX >= 0 && distanceZ < 0)
        {
            Debug.Log("ObjectAはエリア4に属しています");
            resultValue = new Vector3(0f, 0f, 90f);
        }

        return resultValue;
    }


}

