using UnityEngine;

// ゲームの立ち上げ時のロードを担うクラス
public class GameLauncher : MonoBehaviour
{
    [SerializeField]
    private MaterialBunker _materialBunker;

    [SerializeField]
    private WorldDataBunker _worldDataBunker;
    [SerializeField]
    private WorldGenerater _worldGenerater;

    [SerializeField]
    private PixmatesManager _pixmatesManager;

    void Start()
    {
        _materialBunker.Load();
        _worldDataBunker.LoadWorldData();
        _worldGenerater.WorldGenerate();
        _pixmatesManager.Load();
    }
}
