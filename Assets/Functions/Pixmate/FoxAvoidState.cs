using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAvoidState : IAIState
{
    public void EnterState(FoxEcology fe)
    {
        //Debug.Log("Entering Wait State");
    }

    public void UpdateState(FoxEcology fe)
    {
        // 待機中の振る舞いを実装
    }

    public void ExitState(FoxEcology fe)
    {
        //Debug.Log("Exiting Wait State");
    }
}
