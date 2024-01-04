using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class PixmatesManager : MonoBehaviour
{
    // 最初のサイズと最大サイズ設定
    public const float INITIAL_SCALE_FOX = 0.15f;
    public const float MAX_SIZE_FOX = 0.3f;
    // 単位は分(2は仮置き)
    private const float GROWTH_TIME = 0.5f;
    // Pixmateの成長時間
    private float _foxGrowSpeed = 0f;
    public static PixmatesManager InstancePixmatesManager;
    [SerializeField]
    private PixmateIO _pixmateIO;

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

    public void Load()
    {
        // 成長速度の計算
        _foxGrowSpeed = (MAX_SIZE_FOX - INITIAL_SCALE_FOX) / (GROWTH_TIME * 60);
        // 成長速度の受け渡し
        _prefabFox.GetComponent<FoxEcology>().GrowSpeed = _foxGrowSpeed;
        
        // ロード処理
        _pixmatesCount = _pixmateIO.LoadPixmateCount();
        for(int i = 0; i < _pixmatesCount; i++)
        {
            // CroppedImagesに追加。
            int nameCount = i + 1;
            string addTmpKey = PIXMATE_KEY + nameCount;
            Sprite sprite = _pixmateIO.LoadPixmateSprite(addTmpKey);
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

            Vector3 insPosition = _pixmateIO.LoadPixmatePosition(key);
            float scale = _pixmateIO.LoadPixmateScale(key, INITIAL_SCALE_FOX);
            insScale.x = scale;
            insScale.y = scale;
            insScale.z = scale;
            Quaternion rot = _pixmateIO.LoadPixmateRot(key);
            int ForM = _pixmateIO.LoadPixmateForM(key);
            Array.Resize(ref _pixmateFoxes, _pixmateFoxes.Length + 1);
            _pixmateFoxes[_pixmateFoxes.Length - 1] = InstantiatePixmate(insPosition, rot, insScale, matTexture ,ForM);
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
        _pixmateIO.DoSavePixmates(_pixmatesCount, pixmateSprite, ForM, key);
    }

    public FoxEcology InstantiatePixmate(Vector3 insPos, Quaternion insRot, Vector3 scale, Texture2D texture, int ForM)
    {
        // Pixmateの生成処理
        GameObject insPixmate = Instantiate(_prefabFox, insPos, insRot);
        FoxEcology foxEcology = insPixmate.GetComponent<FoxEcology>();
        foxEcology.PixmateForM = ForM;
        // 仮置き
        foxEcology.PixmatesManager = this;
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
                int foxCount = _pixmateFoxes.Length;
                for (int i = 0; i < foxCount; i++)
                {
                    if(foxCount != _pixmateFoxes.Length)
                    {
                        break;
                    }
                    string key = PIXMATE_KEY + (i + 1);
                    _pixmateIO.DoSavePixmatePos(_savePos[i], key);
                    _pixmateIO.DoSavePixmateScale(_saveScale[i], key);
                    _pixmateIO.DoSavePixmateRot(_saveRot[i], key);
                }
            });
            Debug.Log("Fin:" + Time.time);
        }
    }

    public void MaitingStart(Texture2D target1, Texture2D target2, Transform insTransform)
    {
        TextureFusioner textureFusioner = new TextureFusioner();
        // Textureの生成
        Texture2D matTexture = Texture2D.whiteTexture;

        // スプライトからテクスチャを作成
        Sprite sprite = null;
        if (target1 != null && target2 != null) sprite = textureFusioner.CombineTextures(target1, target2);

        if (sprite != null)
        {
            matTexture = new Texture2D(sprite.texture.width, sprite.texture.height, TextureFormat.RGBA32, false);
            matTexture.SetPixels(sprite.texture.GetPixels());
            matTexture.Apply();
        }
        else
        {
            Debug.LogError("Target sprite not found in croppedImages dictionary.");
            return;
        }

        // Pixmateの生成処理
        Vector3 insPos = new Vector3(insTransform.position.x, insTransform.position.y + 0.5f, insTransform.position.z - 1.0f);
        Quaternion insRot = Quaternion.Euler(insTransform.rotation.eulerAngles.x, insTransform.rotation.eulerAngles.y, insTransform.rotation.eulerAngles.z);
        float scale = INITIAL_SCALE_FOX;
        Vector3 insScale = new Vector3(scale, scale, scale);
        // 性別の決定
        int ForM = UnityEngine.Random.Range(0, 2);

        Array.Resize(ref _pixmateFoxes, _pixmateFoxes.Length + 1);
        FoxEcology foxEcology = InstantiatePixmate(insPos, insRot, insScale, matTexture , ForM);
        _pixmateFoxes[_pixmateFoxes.Length - 1] = foxEcology;
        // マテリアルの生成手続き

        SpawnPixmate(matTexture, ForM);
        ActivationPixmate(foxEcology);
    }
}
