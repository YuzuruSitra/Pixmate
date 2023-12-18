using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoxMaitingState : IAIState
{
    // 単位は秒
    private const float MAIT_TIME = 15f;
    private float _currentTime = 0f;
    private Slider _maitingSlider;
    public void EnterState(FoxEcology fe)
    {
        _currentTime = 0f;
        fe.MaitingUI.SetActive(true);
        _maitingSlider = fe.MaitingSlider;
        _maitingSlider.value = 0f;
    }

    public void UpdateState(FoxEcology fe)
    {
        _currentTime += Time.deltaTime;
        float ratio = _currentTime / MAIT_TIME;
        _maitingSlider.value = ratio;
        // 交配ゲージ蓄積処理
        if(MAIT_TIME > _currentTime) return;
        
        // ステートの終わり
        fe.FinSpecialaction();
    }

    public void ExitState(FoxEcology fe)
    {
        if(fe.PixmateForM == 1) fe.PixmatesManager.MaitingStart(fe.ThisTexture, fe.TargetTexture, fe.transform);
        fe.MaitingUI.SetActive(false);
        fe.ElapseMateTime = 0;
    }
}
