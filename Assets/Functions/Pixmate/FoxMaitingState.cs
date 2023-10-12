using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMaitingState : IAIState
{
    // 単位は秒
    private const float MAIT_TIME = 15f;
    private float _currentTime = 0f;
    public void EnterState(FoxEcology fe)
    {
        _currentTime = 0f;
    }

    public void UpdateState(FoxEcology fe)
    {
        _currentTime += Time.deltaTime;
        // 交配ゲージ蓄積処理

        if(MAIT_TIME < _currentTime) return;
        // ステートの終わり
        fe.FinSpecialaction();
    }

    public void ExitState(FoxEcology fe)
    {
        // Pixmate生成時に性別を生成して
        // if(メスなら) 子供を産む
        //とする
    }
}
