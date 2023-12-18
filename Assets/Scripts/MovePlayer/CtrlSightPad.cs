using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CtrlSightPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isViewPad;
    public bool IsViewPad => _isViewPad;

    // イベントの定義
    public event Action<bool> ChangeViewState;

    // イベント発行
    public void OutChangeView()
    {
        ChangeViewState?.Invoke(_isViewPad);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isViewPad = true;
        ChangeViewState?.Invoke(_isViewPad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isViewPad = false;
        ChangeViewState?.Invoke(_isViewPad);
    }

}
