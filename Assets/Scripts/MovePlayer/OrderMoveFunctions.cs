using UnityEngine;

public class OrderMoveFunctions : MonoBehaviour
{
    // プレイヤー制御
    [SerializeField]
    private CtrlPlayer _ctrlPlayer;
    // カメラ制御
    [SerializeField]
    private CtrlMainCam _ctrlMainCam;
    
    // 入力装置
    [SerializeField]
    private CtrlSightPad _ctrlSightPad;
    [SerializeField]
    private CtrlMovePad _ctrlMovePad;

    Vector2 lastTouchPos = Vector2.zero;
    private bool _pushedViewPad;

    void Start()
    {
        // イベントのリスナー登録
        _ctrlSightPad.ChangeViewState += SetViewStartPos;
    }

    void FixedUpdate()
    {
        CtrlView();
        CtrlMove();
    }

    // 視点の制御
    void CtrlView()
    {
        if(!_pushedViewPad)return;

        // 変更角度の算出
        Vector2 currentTouchPos = Input.mousePosition;
        float rotationX = (currentTouchPos.x - lastTouchPos.x) * 0.05f;
        float rotationY = (currentTouchPos.y - lastTouchPos.y) * 0.05f;
        lastTouchPos = currentTouchPos;
        // プレイヤーの角度制御
        _ctrlPlayer.MoveRot(rotationX);
        // カメラの角度制御
        _ctrlMainCam.CtrlRotCamera(rotationY);
        // 計算座標の更新
        lastTouchPos = currentTouchPos;
    }

    // 動きの制御
    void CtrlMove()
    {
        if(!_ctrlMovePad.IsPushedPad) return;

        // 現在のマウスのワールド座標を取得
        Vector3 pushMousePos = Input.mousePosition;
        // タッチパッドの初期座標
        Vector3 _ctrlMovePadPos = _ctrlMovePad.PadPos;
        // 一つ前のマウス座標との差分を計算して変化量を取得
        Vector3 differencePos = pushMousePos - _ctrlMovePadPos;
        // 入力の判定
        int _convertX = CalcDecisionX(differencePos.x);
        int _convertZ = CalcDecisionZ(differencePos.y);
        // プレイヤーの移動
        _ctrlPlayer.CtrlMovePlayer(_convertX, _convertZ);
        // 方向指示UIの制御
        _ctrlMovePad.MoveIndicat(_convertX, _convertZ); 
    }

    /*---------------------------------------------*/
    void OnDestroy()
    {
        // イベントのリスナー削除
        _ctrlSightPad.ChangeViewState -= SetViewStartPos;
    }

    void SetViewStartPos(bool isPushed)
    {
        lastTouchPos = Input.mousePosition;
        _pushedViewPad = isPushed;
    }

    int CalcDecisionX(float calcX)
    {
        if (calcX < -_ctrlMovePad.WidthPadding)
            return -1;
        else if (calcX > _ctrlMovePad.WidthPadding)
            return 1;
        else
            return 0;
    }

    int CalcDecisionZ(float calcZ)
    {
        if (calcZ < -_ctrlMovePad.HeightPadding)
            return -1;
        else if (calcZ > _ctrlMovePad.HeightPadding)
            return 1;
        else
            return 0;
    }
}
