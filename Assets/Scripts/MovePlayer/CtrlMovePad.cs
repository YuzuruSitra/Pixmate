using System;
using UnityEngine;
using UnityEngine.EventSystems;

// MovePad関連のUI描画を担当しているクラス
public class CtrlMovePad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private GameObject _indicatUI;
    [SerializeField]
    private GameObject[] _criteriaPoints = new GameObject[8];

    private bool _isPushedPad;
    public bool IsPushedPad => _isPushedPad;

    private Vector3 _padPos;
    public Vector3 PadPos => _padPos;
    private const int _heightPadding = 125;
    public int HeightPadding => _heightPadding;
    private const int _widthPadding = 100;
    public int WidthPadding => _widthPadding;

    public enum Direction
    {
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        BottomLeft,
        Left,
        TopLeft,
        Mid
    }

    // イベントの定義
    public event Action<bool> ChangeMoveState;

    void Start()
    {
        // イベントのリスナー登録
        ChangeMoveState += ChangeIndicat;
        _padPos = this.transform.position;
        _indicatUI.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPushedPad = true;
        ChangeMoveState?.Invoke(_isPushedPad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPushedPad = false;
        ChangeMoveState?.Invoke(_isPushedPad);
    }

    public void MoveIndicat(int diX , int diY)
    {
        _indicatUI.SetActive(true);
        int setPoint = SetIndicat(diX, diY);
        
        // 中央は非表示
        if(setPoint == 8)
        {
            _indicatUI.SetActive(false);
            return;
        }

        Transform indicatTransform = _indicatUI.transform;
        
        // 親子関係
        RectTransform indicatRectTransform = _indicatUI.GetComponent<RectTransform>();
        RectTransform newParentRectTransform = _criteriaPoints[setPoint].GetComponent<RectTransform>();
        indicatRectTransform.SetParent(newParentRectTransform, true);
        // 角度
        Vector3 setRot = new Vector3(0f , 0f, 0f); 
        indicatTransform.localEulerAngles = setRot;
        // 位置
        Vector3 setPos = new Vector3(0f , 50f, 0f); 
        indicatTransform.localPosition = setPos;
    }

    /*---------------------------------------------*/

    void OnDestroy()
    {
        // イベントのリスナー削除
        ChangeMoveState -= ChangeIndicat;
    }

    // 指示UIの表示切り替え
    void ChangeIndicat(bool isPushed)
    {
        if(!isPushed)_indicatUI.SetActive(false);   
    }

    int SetIndicat(int tmpX , int tmpY)
    {
        Direction currentDirection = Direction.Top;

        if(tmpX == 0 && tmpY == 1) 
        {
            currentDirection = Direction.Top;
        }
        else if(tmpX == 1 && tmpY == 1) 
        {
            currentDirection = Direction.TopRight;
        }
        else if(tmpX == 1 && tmpY == 0) 
        {
            currentDirection = Direction.Right;
        }
        else if(tmpX == 1 && tmpY == -1) 
        {
            currentDirection = Direction.BottomRight;
        }
        else if(tmpX == 0 && tmpY == -1)
        {
            currentDirection = Direction.Bottom;
        }
        else if(tmpX == -1 && tmpY == -1)
        {
            currentDirection = Direction.BottomLeft;
        }
        else if(tmpX == -1 && tmpY == 0)
        {
            currentDirection = Direction.Left;
        }
        else if(tmpX == -1 && tmpY == 1)
        {
            currentDirection = Direction.TopLeft;
        }
        else
        {
            currentDirection = Direction.Mid;
        }

        return (int)currentDirection;
    }

}
