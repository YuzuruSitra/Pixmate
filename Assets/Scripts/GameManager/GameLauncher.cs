using UnityEngine;

// ゲームの立ち上げ時のロードを担うクラス
public class GameLauncher : MonoBehaviour
{
    [SerializeField]
    private MaterialBunker _materialBunker;

    [SerializeField]
    private WorldManager _worldManager;

    [SerializeField]
    private PixmatesManager _pixmatesManager;

    void Start()
    {
        _materialBunker.Load();
        _worldManager.Load();
        _pixmatesManager.Load();
    }
}
