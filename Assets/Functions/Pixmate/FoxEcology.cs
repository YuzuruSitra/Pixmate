using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FoxEcology : MonoBehaviour
{
    // 仮置き
    public PixmatesManager PixmatesManager;
    // 成長速度
    public float GrowSpeed;
    public float PixmateForM;
    private int _interval = 5;
    private bool _oneTime = true;
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
    // 交配クールタイム(分)
    private const float MAITE_COOL_TIME = 0.1f;
    public float ElapseMateTime = 0;
    // 交配相手のテクスチャ
    private Texture2D _targetTexture;
    public Texture2D TargetTexture => _targetTexture;
    private Texture2D _thisTexture;
    public Texture2D ThisTexture => _thisTexture;
    // 交配用UI
    public GameObject MaitingUI;
    public Slider MaitingSlider;
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
            { "Avoid", new FoxAvoidState() },
            { "Maiting", new FoxMaitingState() }
        };
        _currentState = _states["Idole"]; // 初期状態を待機状態に設定
        _beforeSpecialState = _currentState;
        _currentState.EnterState(this);

        // 初期サイズから最大サイズを計算して指定時間で育つように設定
    }

    // 他の管理クラスから呼びだすとPixmateが動き出す
    public void ComeAlive()
    {
        // 最初のState遷移インターバル
        _isAllive = true;
        nextActionTime = Random.Range(8, 10);
        ChangeState(_states["Idole"]);
        _thisTexture =  gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[0].GetTexture("_BaseMap") as Texture2D;
    }
    
    private void Update()
    {
        if(!_isAllive) return;
        // 交配時間の計測
        ElapseMateTime += Time.deltaTime;

        // 成長
        if ((int)Time.time % _interval == 0 && _oneTime)
        {
            float currentScale = transform.localScale.x;
            if (currentScale < PixmatesManager.MAX_SIZE_FOX)
            {
                currentScale += GrowSpeed * _interval;
                // 最大サイズを超えないように設定
                currentScale = Mathf.Min(currentScale, PixmatesManager.MAX_SIZE_FOX);
                transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            }
            _oneTime = false;
        }
        else if((int)Time.time % _interval != 0)
        {
            if(!_oneTime) _oneTime = true;
        }
        

        // 障害物判定
        _isNoObstacle = IsObstacleDecision();

        // 接地判定
        _isGround = IsGroundStay();

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

        if(newState == _states["Jump"] || newState == _states["Avoid"])
        {
            // 時間だと管理しづらいので一考の余地あり
            _doSpecialAction = true;
            int waitTime = Random.Range(4, 8);
            Invoke("FinSpecialaction",waitTime);
        }
        if(newState == _states["Maiting"])
        {
            _doSpecialAction = true;
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
            case FoxMaitingState:
                _foxAnimCtrl.DoIdole();
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
    private bool IsGroundStay()
    {
        float groundRayLength = 0.4f;

        Vector3 groundOffset = new Vector3(0, 0.1f, 0.35f);
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

    // 交配判定
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PixmateFox"))
        {
            // 交配可能時間なら実行
            if(MAITE_COOL_TIME * 60 > ElapseMateTime) return;
            if(PixmatesManager.MAX_SIZE_FOX > transform.localScale.x && PixmatesManager.MAX_SIZE_FOX > other.transform.localScale.x) return;
            if(PixmateForM == 1 && other.GetComponent<FoxEcology>().PixmateForM == 0) 
            {
                GameObject targetObj = other.gameObject;
                _targetTexture = targetObj.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[0].GetTexture("_BaseMap") as Texture2D;
                // 相手のステートチェンジ
                targetObj.GetComponent<FoxEcology>().ChangeState(_states["Maiting"]);
                ChangeState(_states["Maiting"]);
            }
        }
    }

    // 特殊行動が終わるとIdoleへ
    public void FinSpecialaction()
    {
        _beforeSpecialState = _currentState;
        _doSpecialAction = false;
        ChangeState(_states["Idole"]);
    }

}

// Pixmanagerが監視して交配があれば子オブジェクトを生成するように変更予定