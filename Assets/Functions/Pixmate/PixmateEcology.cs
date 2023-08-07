using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PixmateEcology : MonoBehaviour
{
    
    [SerializeField]
    private WorldManager _worldManager;

    [SerializeField]
    private Vector3 _rayOffset = new Vector3(0, 0.6f, 0.5f);
    [SerializeField]
    float _rayLength = 1.5f;

    [SerializeField]
    private Transform target;
    [SerializeField] 
    private float _rotationSpeed = 2.0f;
    [SerializeField]
    private float _moveSpeed = 1.0f;
    [SerializeField]
    private Vector3 _targetPos = Vector3.zero;

    public enum PixmateAiState
    {
        WAIT,
        MOVE,
        Jump,
        Avoid
    }

    private PixmateAiState _currentState;

    public event Action<PixmateAiState> OnAIStateChanged;

    void Start()
    {
        OnAIStateChanged += UpdateAI;
        _worldManager = WorldManager.InstanceWorldManager;
        StartCoroutine("DoMove");
    }

    void ChangeAIState(PixmateAiState newState)
    {
        // 同じステートを弾く
        //if(_currentState == newState) return;
        _currentState = newState;
        OnAIStateChanged?.Invoke(_currentState);
    }
    
    void Update()
    {
        // 原点と方向を設定しrayを生成
        Vector3 dynamicOffset = transform.forward * _rayOffset.z + transform.up * _rayOffset.y + transform.right * _rayOffset.x;
        Vector3 origin = dynamicOffset + transform.position;
        Vector3 direction = transform.forward * _rayLength;
        Ray ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out RaycastHit hit)) HitRayActiion(hit.collider.gameObject);
    
        Debug.DrawRay(ray.origin, ray.direction * _rayLength, Color.red);


        // 目の前に地面があるか判定
        Vector3 dropOffset = new Vector3(0, 0.5f, 1.0f);
        Vector3 dropRayOffset = transform.forward * dropOffset.z + transform.up * dropOffset.y + transform.right * dropOffset.x;
        Vector3 dropOrigin = dropRayOffset + transform.position;
        Vector3 dropDirection = -transform.up * _rayLength;
        Ray dropRay = new Ray(dropOrigin, dropDirection);
        if (!Physics.Raycast(dropRay, out RaycastHit dropHit)) ChangeAIState(PixmateAiState.Avoid);
    
        Debug.DrawRay(dropRay.origin, dropRay.direction * _rayLength, Color.red);
    }

    public void HitRayActiion(GameObject hitObj)
    {
        string hitObjTag = hitObj.tag;
        switch (hitObjTag)
        {   
            case "Player":
                break;
            default:
                Vector3 hitObjOrigin = hitObj.transform.position;
                Vector3 hitObjDirection = new Vector3(0, 1.0f, 0);
                float rayLength = 1.0f;
                Ray hitObjRay = new Ray(hitObjOrigin, hitObjDirection * rayLength);
                // 前に障害物があり縦1マスならジャンプ、違う場合避ける
                PixmateAiState state = PixmateAiState.Jump;
                if (Physics.Raycast(hitObjRay, out RaycastHit hitObjHit)) state = PixmateAiState.Avoid;

                ChangeAIState(state);

                Debug.DrawRay(hitObjRay.origin, hitObjRay.direction * 1, Color.blue);
                break;
        }
    }

    void UpdateAI(PixmateAiState state)
    {
        switch(state)
        {
            case PixmateAiState.WAIT:
                StartCoroutine("DoWait");
                break;
            case PixmateAiState.MOVE:
                StartCoroutine("DoMove");
                break;
            case PixmateAiState.Jump:
                DoJump();
                break;
            case PixmateAiState.Avoid:
                DoAvoid();
                break;
        }
    }

    IEnumerator DoWait()
    {
        // Idole
        // 再抽選
        yield return new WaitForSeconds(2.0f);
        SelectNextAction();
    }

    IEnumerator DoMove()
    {
        // 目標座標の選定
        float targetPosX = UnityEngine.Random.Range(-1.0f, 1.0f);
        float targetPosZ = UnityEngine.Random.Range(-1.0f, 1.0f);

        Vector3 targetPos = new Vector3(targetPosX, 0.0f, targetPosZ);
        Quaternion rotation = Quaternion.LookRotation(targetPos, Vector3.up);
        
        float elapsedTime = 0.0f;

        // 回転処理
        while (elapsedTime < 3.0f)
        {
            elapsedTime += Time.deltaTime * _rotationSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, elapsedTime);
            yield return null;
        }

        // 移動時間の選定
        float continueTime = UnityEngine.Random.Range(0f, 3.0f);
        elapsedTime = 0.0f;
        // 移動処理
        while (elapsedTime < continueTime)
        {
            elapsedTime += Time.deltaTime;
            transform.position += transform.forward * _moveSpeed * Time.deltaTime;
            yield return null;
        }
        SelectNextAction();
    }

    void DoJump()
    {
        // 1マス分ジャンプ
    }

    void DoAvoid()
    {
        // 問題ない方向へかわす

        // ステート再抽選
    }

    // 次の行動の選択
    public void SelectNextAction()
    {
        int nextAction = UnityEngine.Random.Range(1, 2);
        PixmateAiState nextState = (PixmateAiState)nextAction;
        ChangeAIState(nextState);
        Debug.Log("aaaaaa");
    }
}
