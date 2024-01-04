using UnityEngine;

// ゲームの設定をするクラス
public class Config : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;
    }
}
