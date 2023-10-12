using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

public class PixmatesManager : MonoBehaviour
{
    // 最初のサイズと最大サイズ設定
    public const float INITIAL_SCALE_FOX = 0.15f;
    public const float MAX_SIZE_FOX = 0.3f;
    // 単位は分(2は仮置き)
    private const float GROWTH_TIME = 2f;
    // Pixmateの成長時間
    private float _foxGrowSpeed = 0f;
    public static PixmatesManager InstancePixmatesManager;
    SaveManager _saveManager;

    public const string PIXMATE_KEY = "PixmateNo.";
    public string PixmateKey => PIXMATE_KEY;

    // 存在できるPixmateの最大値
    private const int MAX_PIXMATES = 10;
    // 現在いるPixmateの数
    private int _pixmatesCount = 0;
    public int PixmatesCount => _pixmatesCount;
    private FoxEcology[] _pixmateFoxes = new FoxEcology[0];

    // 生成Prefab
    [SerializeField]
    private GameObject _prefabFox;
    private Vector3[] _savePos;
    private float[] _saveScale;
    private Quaternion[] _saveRot;

    // セーブと紐づけ
    public Dictionary<string, Sprite> _textureImages = new Dictionary<string, Sprite>();
    // 定期セーブの間隔
    private const float PERIODIC_TIME = 15f;
    // 非同期用
    private CancellationTokenSource _cancellationTokenSource;
    void Awake()
    {
        if (InstancePixmatesManager == null)
        {
            InstancePixmatesManager = this;
        }
    }

    private void Start()
    {
        _saveManager = SaveManager.InstanceSaveManager;
        // 成長速度の計算
        _foxGrowSpeed = (MAX_SIZE_FOX - INITIAL_SCALE_FOX) / (GROWTH_TIME * 60);
        // 成長速度の受け渡し
        _prefabFox.GetComponent<FoxEcology>().GrowSpeed = _foxGrowSpeed;
        
        // ロード処理
        _pixmatesCount = _saveManager.LoadPixmateCount();
        for(int i = 0; i < _pixmatesCount; i++)
        {
            // CroppedImagesに追加。
            int nameCount = i + 1;
            string addTmpKey = PIXMATE_KEY + nameCount;
            Sprite sprite = _saveManager.LoadPixmateSprite(addTmpKey);
            if(sprite != null) _textureImages.Add(addTmpKey, sprite);
        }

        Vector3 insScale = new Vector3(0,0,0);
        // Pixmateの生成
        for(int i = 0; i < _pixmatesCount; i++)
        {
            string key = PIXMATE_KEY + (i + 1);
            Texture2D matTexture = Texture2D.whiteTexture;
            // テクスチャの取得
            if (_textureImages.TryGetValue(key, out Sprite targetSprite))
            {
                matTexture = new Texture2D(targetSprite.texture.width, targetSprite.texture.height, TextureFormat.RGBA32, false);
                matTexture.SetPixels(targetSprite.texture.GetPixels());
                matTexture.Apply();
            }
            else
            {
                continue;
            }

            Vector3 randomPosition = _saveManager.LoadPixmatePosition(key);
            float scale = _saveManager.LoadPixmateScale(key, INITIAL_SCALE_FOX);
            insScale.x = scale;
            insScale.y = scale;
            insScale.z = scale;
            Quaternion rot = _saveManager.LoadPixmateRot(key);
            Array.Resize(ref _pixmateFoxes, _pixmateFoxes.Length + 1);
            int ForM = _saveManager.LoadPixmateForM(key);
            _pixmateFoxes[_pixmateFoxes.Length - 1] = InstantiatePixmate(randomPosition, rot, insScale, matTexture ,ForM);
            FoxEcology pixmateEcology = _pixmateFoxes[i];
            ActivationPixmate(pixmateEcology);
        };

        _cancellationTokenSource = new CancellationTokenSource(); // キャンセルトークン生成
            
        PeriodicSaveAsync(_cancellationTokenSource.Token).Forget();
    }

    private void OnDestroy()
    {
        // Destroy時にキャンセルする
        _cancellationTokenSource?.Cancel();
    }

    // Pixmateの動き出し
    public void ActivationPixmate(FoxEcology pixmateEcology)
    {
        pixmateEcology.ComeAlive();
    }

    // Pixmateの追加処理
    public void SpawnPixmate(Texture2D texture, int ForM)
    {
        _pixmatesCount ++;
        
        //Textureをスプライトに変換し辞書へ追加
        PixmateOutSprite pixmateOutSprite = new PixmateOutSprite();
        Sprite pixmateSprite = pixmateOutSprite.AddPixmateSprite(_pixmatesCount, texture);
        if(pixmateSprite == null) return;
        string key = PIXMATE_KEY + _pixmatesCount;
        _textureImages.Add(key, pixmateSprite);
        // セーブ処理
        _saveManager.DoSavePixmates(_pixmatesCount, pixmateSprite, ForM, key);
    }

    public FoxEcology InstantiatePixmate(Vector3 insPos, Quaternion insRot, Vector3 scale, Texture2D texture, int ForM)
    {
        // Pixmateの生成処理
        GameObject insPixmate = Instantiate(_prefabFox, insPos, insRot);
        FoxEcology foxEcology = insPixmate.GetComponent<FoxEcology>();
        foxEcology.PixmateForM = ForM;
        insPixmate.transform.localScale = scale;
        // テクスチャの割り当て
        GameObject childObj = insPixmate.transform.GetChild(0).gameObject;
        childObj.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_BaseMap", texture);
        return foxEcology;
    }

    // 定期セーブ
    private async UniTask PeriodicSaveAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(PERIODIC_TIME));

            _savePos = new Vector3[_pixmateFoxes.Length];
            _saveScale = new float[_pixmateFoxes.Length];
            _saveRot = new Quaternion[_pixmateFoxes.Length];

            for(int i = 0; i < _pixmateFoxes.Length; i++)
            {
                Transform targetTransform = _pixmateFoxes[i].transform;
                _savePos[i] = targetTransform.position;
                _saveScale[i] = targetTransform.localScale.x;
                _saveRot[i] = targetTransform.rotation;
            }

            Debug.Log("Start:" + Time.time);
            // 保存処理をThreadPoolで非同期に実行
            await UniTask.RunOnThreadPool(() =>
            {
                for (int i = 0; i < _pixmateFoxes.Length; i++)
                {
                    string key = PIXMATE_KEY + (i + 1);
                    _saveManager.DoSavePixmatePos(_savePos[i], key);
                    _saveManager.DoSavePixmateScale(_saveScale[i], key);
                    _saveManager.DoSavePixmateRot(_saveRot[i], key);
                }
            });
            Debug.Log("Fin:" + Time.time);
        }
    }
}
