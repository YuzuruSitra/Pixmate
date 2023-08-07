using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixmates;

public class PixmateEcology : MonoBehaviour
{
    
    [SerializeField]
    private WorldManager _worldManager;
    [SerializeField]
    private PixmateHandler _pixmateHandler;

    [SerializeField]
    private Vector3 _rayOffset = new Vector3(0, 0.5f, -0.5f);
    [SerializeField]
    float _rayLength = 1.0f;

    [SerializeField]
    private Transform target;
    [SerializeField] 
    private float _rotationSpeed = 2.0f;
    [SerializeField]
    private float _moveSpeed = 1.0f;
    [SerializeField] 
    private float _arrivalThreshold = 0.1f; // 到達閾値
    [SerializeField]
    private Vector3 _targetPos = Vector3.zero;

    void Start()
    {
        _pixmateHandler.OnAIStateChanged += UpdateAI;
        _worldManager = WorldManager.InstanceWorldManager;
        StartCoroutine("DoMove");
    }
    
    void Update()
    {
        // 原点と方向を設定しrayを生成
        Vector3 origin = _rayOffset + this.transform.position;
        Vector3 direction = transform.forward;
        Ray ray = new Ray(origin, direction * _rayLength);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            _pixmateHandler.HitRayActiion(hit.collider.gameObject);
        }
        Debug.DrawRay(ray.origin, ray.direction * 1, Color.red);
        
    }

    void UpdateAI(Pixmates.PixmateHandler.PixmateAiState state)
    {
        switch(state)
        {
            case Pixmates.PixmateHandler.PixmateAiState.WAIT:
                DoWait();
                break;
            case Pixmates.PixmateHandler.PixmateAiState.MOVE:
                StartCoroutine("DoMove");
                break;
            case Pixmates.PixmateHandler.PixmateAiState.Jump:
                DoJump();
                break;
            case Pixmates.PixmateHandler.PixmateAiState.Avoid:
                DoAvoid();
                break;
        }
    }

    void DoWait()
    {
        
    }

    IEnumerator DoMove()
    {
        // 目標座標の選定
        int targetPosX;
        int edgeXMax = _worldManager.WorldEdgeXMax;
        int edgeXMin = _worldManager.WorldEdgeXMin;

        int targetPosZ;
        int edgeZMax = _worldManager.WorldEdgeZMax;
        int edgeZMin = _worldManager.WorldEdgeZMin;
        while(true)
        {   
            targetPosX = (int) transform.position.x + Random.Range(-3, 4);
            targetPosX = 4;
            if(targetPosX >= edgeXMin && targetPosX <= edgeXMax) break;
        }
        while(true)
        {   
            targetPosZ = (int) transform.position.z + Random.Range(-3, 4);
            targetPosZ = 4;
            if(targetPosZ >= edgeZMin && targetPosZ <= edgeZMax) break;
        }
        Vector3 targetPos = new Vector3(targetPosX, 0.0f, targetPosZ);
        Vector3 targetDir = targetPos - transform.position;
        targetDir.y = 0.0f;
        
        Quaternion rotation = Quaternion.LookRotation(targetDir, Vector3.up);
        
        float elapsedTime = 0.0f;

        while (elapsedTime < 3.0f)
        {
            elapsedTime += Time.deltaTime * _rotationSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, elapsedTime);
            yield return null;
        }

        while (Vector3.Distance(transform.position, targetPos) > _arrivalThreshold)
        {
            transform.position += transform.forward * _moveSpeed * Time.deltaTime;
            yield return null;
        }

        // 移動方向と時間をランダムに設定
        // 移動処理に変更

        Debug.Log("aaaaaa");
        // 現在の位置
        //float present_Location = (Time.time * speed) / distance_two;

        // オブジェクトの移動
        //transform.position = Vector3.Lerp(startMarker.position, endMarker.position, present_Location);
        
    }

    void DoJump()
    {

    }

    void DoAvoid()
    {

    }
}
