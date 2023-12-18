using UnityEngine;

public interface IAIState
{
    void EnterState(FoxEcology fe);
    void UpdateState(FoxEcology fe);
    void ExitState(FoxEcology fe);
}