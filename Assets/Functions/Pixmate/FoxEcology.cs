using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FoxEcology : MonoBehaviour
{
    // 活動状態を管理
    private bool _isAllive = false;
    [SerializeField]
    private FoxAnimCtrl _foxAnimCtrl;
    public Dictionary<string, IAIState> _states;
    public Dictionary<string, IAIState> States => _states;
    private IAIState _currentState;
    private IAIState _beforeSpecialState;
    public IAIState BeforeSpecialState => _beforeSpecialState;
    [SerializeField]
    private float _moveSpeed = 1.0f;
    public float MoveSpeed => _moveSpeed;
    int nextActionTime;
    private float _elapsedTime = 0f;
    // 強制行動を踏んでいる時はfalseに。
    private bool _isNoObstacle = true;
    public bool IsNoObstacle => _isNoObstacle;
    private bool _isGround = true;
    // ステートから受け取り
    public bool _doSpecialAction = false;
    // 正面に1マス以内に床があるか判定
    private bool _isFrontFloorDecision = true;
    public bool IsFrontFloorDecision => _isFrontFloorDecision;

    private void Awake()
    {
        // Stateを辞書に格納
        _states = new Dictionary<string, IAIState>
        {
            { "Idole", new FoxIdoleState() },
            { "Walk", new FoxWalkState() },
            { "Jump", new FoxJumpState() },
            { "Avoid", new FoxAvoidState() }
        };
        _currentState = _states["Idole"]; // 初期状態を待機状態に設定
        _beforeSpecialState = _currentState;
        _currentState.EnterState(this);
    }

    // 他の管理クラスから呼びだすとPixmateが動き出す
    public void ComeAlive()
    {
        // 最初のState遷移インターバル
        _isAllive = true;
        nextActionTime = Random.Range(5, 10);
        ChangeState(_states["Idole"]);
    }

    private void Update()
    {
        if(!_isAllive) return;

        // 障害物判定
        _isNoObstacle = IsObstacleDecision();

        // 接地判定
        _isGround = isGroundStay();

        // 目の前の1.5マス下に地面があるか判定
        _isFrontFloorDecision = FrontFloorDecisio();
        
        // 現在ステートのUpdate処理
        _currentState.UpdateState(this);

        // 特殊行動をとっていない時のみ抽選時間を加算
        if(!_doSpecialAction && _isGround) 
        {
            _elapsedTime += Time.deltaTime;    
        }

        SetNextAction();
    }

    // AIの行動選択
    void SetNextAction()
    {
        // 指定時間に達すると処理を開始
        if(nextActionTime > _elapsedTime) return;
        _elapsedTime = 0f;
        nextActionTime = Random.Range(2, 8);

        // ランダム選択を変更する余地あり
        int nextState = Random.Range(0, 2);
        switch (nextState)
        {
            case 0:
                ChangeState(_states["Idole"]);
                break;
            case 1:
                ChangeState(_states["Walk"]);
                break;
        }
    }

    // 条件から呼び出す。
    public void ChangeState(IAIState newState)
    {
        // 空中と特殊行動中は処理を返す。
        if(!_isGround || _doSpecialAction) return;
        if(newState == _currentState) return;
        _currentState.ExitState(this);
        newState.EnterState(this);
        _currentState = newState;

        if(newState == _states["Jump"] || newState.GetType() == typeof(FoxAvoidState))
        {
            // 時間だと管理しづらいので一考の余地あり
            _doSpecialAction = true;
            int waitTime = Random.Range(4, 8);
            Invoke("FinSpecialaction",waitTime);
        }

        // アニメーションの呼び出し
        switch(newState)
        {
            case FoxIdoleState:
                _foxAnimCtrl.DoIdole();
                break;
            case FoxWalkState:
                _foxAnimCtrl.DoWalk();
                break;
            case FoxJumpState:
                _foxAnimCtrl.DoJump();
                break;
            case FoxAvoidState:
                _foxAnimCtrl.DoAvoid();
                break;
        }
    }


    /****************************************************/

    // 障害物判定
    private bool IsObstacleDecision()
    {
        bool isNoObstac = true;
        float rayOffsetX = 0f;
        float rayOffsetY = 0.9f;
        float rayOffsetZ = 0.8f;
        float rayLength = 0.75f;

        Vector3 rayOrigin = transform.position + transform.up * rayOffsetY + transform.right * rayOffsetX + transform.forward * rayOffsetZ;
        Vector3 rayDirection = transform.forward - transform.up;
        rayDirection.Normalize();

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayLength))
        {
            isNoObstac = false;
            GameObject hitObject = hit.collider.gameObject;
            string hitObjectTag = hitObject.tag;

            switch (hitObjectTag)
            {
                case "Player":
                case "MainCamera":
                    break;
                default:
                    Vector3 hitObjOrigin = hitObject.transform.position + Vector3.up * 0.35f;
                    Vector3 hitObjDirection = Vector3.up;
                    Ray hitObjRay = new Ray(hitObjOrigin, hitObjDirection);

                    IAIState newState = Physics.Raycast(hitObjRay, rayLength) ?_states["Avoid"] : _states["Jump"];
                    Debug.DrawRay(hitObjRay.origin, hitObjRay.direction * rayLength, Color.blue);
                    
                    ChangeState(newState);
                    break;
            }
        }

        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red);
        return isNoObstac;
    }

    // 接地判定
    private bool isGroundStay()
    {
        float groundRayLength = 0.2f;

        Vector3 groundOffset = new Vector3(0, 0.1f, 0);
        Vector3 groundRayOffset = transform.forward * groundOffset.z + transform.up * groundOffset.y + transform.right * groundOffset.x;
        Vector3 groundOrigin = transform.position + groundRayOffset;
        Vector3 groundDirection = -transform.up;

        bool isGroundHit = Physics.Raycast(groundOrigin, groundDirection, out RaycastHit groundHit, groundRayLength);

        // レイの可視化
        Color rayColor = isGroundHit ? Color.green : Color.red;
        Debug.DrawRay(groundOrigin, groundDirection * groundRayLength, rayColor);

        return isGroundHit && groundHit.distance <= groundRayLength;
    }

    // 領域内か判定
    private bool FrontFloorDecisio()
    {
        float rayLength = 1.0f;
        Vector3 dropOffset = new Vector3(0, 0.5f, 1.0f);
        Vector3 dropRayOffset = transform.forward * dropOffset.z + transform.up * dropOffset.y + transform.right * dropOffset.x;
        Vector3 dropOrigin = dropRayOffset + transform.position;
        Vector3 dropDirection = -transform.up * rayLength;
        Ray dropRay = new Ray(dropOrigin, dropDirection);
        bool isGroundDecisio = Physics.Raycast(dropRay, out RaycastHit dropHit);
        if(!IsFrontFloorDecision) ChangeState(_states["Avoid"]);

        Color rayColor = IsFrontFloorDecision ? Color.green : Color.red;
        Debug.DrawRay(dropRay.origin, dropRay.direction * rayLength, rayColor);

        return isGroundDecisio;
    }

    // 特殊行動が終わるとIdoleへ
    public void FinSpecialaction()
    {
        _beforeSpecialState = _currentState;
        _doSpecialAction = false;
        ChangeState(_states["Idole"]);
    }

}