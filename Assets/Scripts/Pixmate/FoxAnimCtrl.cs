using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAnimCtrl : MonoBehaviour
{
    private Animator _animator;
    string _currentAnim;
    private int _idoleCount = 6;
    private int _walkCount = 1;
    private int _runCount = 1;
    private int _JumpCount = 1;
    private int _restCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        _currentAnim = "IsIdole";
        _animator = GetComponent<Animator>();
        _animator.SetInteger("AnimNumber", 1);
    }

    public void DoIdole()
    {
        ResetAnim();
        _currentAnim = "IsIdole";
        _animator.SetBool(_currentAnim, true);
        SelectNumber(_idoleCount);
    }

    public void DoWalk()
    {
        ResetAnim();
        _currentAnim = "IsWalk";
        _animator.SetBool(_currentAnim, true);
        SelectNumber(_walkCount);
    }

    public void DoRun()
    {
        ResetAnim();
        _currentAnim = "IsRun";
        _animator.SetBool(_currentAnim, true);
        SelectNumber(_runCount);
    }

    public void DoJump()
    {
        ResetAnim();
        _currentAnim = "IsJump";
        _animator.SetBool(_currentAnim, true);
        SelectNumber(_JumpCount);
    }

    public void DoAvoid()
    {
        ResetAnim();
        _currentAnim = "IsWalk";
        _animator.SetBool(_currentAnim, true);
        SelectNumber(_walkCount);
    }

    public void DoRest()
    {
        ResetAnim();
        _currentAnim = "IsRest";
        _animator.SetBool(_currentAnim, true);
        SelectNumber(_restCount);
    }

    // アニメーションのリセット
    void ResetAnim()
    {
        _animator.SetBool(_currentAnim, false);
        _animator.SetInteger("AnimNumber", 0);
    }

    // アニメーションの選定
    void SelectNumber(int count)
    {
        int selectAnim = Random.Range(1, count + 1);
        _animator.SetInteger("AnimNumber", selectAnim);
    }
}
