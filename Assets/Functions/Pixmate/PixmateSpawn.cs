using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PixmateSpawn : MonoBehaviour
{
    private PixmatesManager _pixmateManager;
    private MaterialBunker _materialBunker;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField]
    private Material _material;

    // 生成したprefab
    private GameObject _insPixmate;
    // 生成に必要な時間
    private const float REQUIRE_TIME = 5.0f;
    // 経過時間
    private float _elapsedTime = 0.0f;
    // 処理の開始
    private bool _doProcess = false;
    // アニメーションの継続時間
    private const float DURATION = 2.5f;
    private const float MAX_WEIGHT = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        _pixmateManager = PixmatesManager.InstancePixmatesManager;
        _materialBunker = MaterialBunker.InstanceMatBunker;
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    // Pixmateを生成時に呼ぶ
    public void LaunchSpawn(GameObject catalystObj)
    {
        // 初期化
        _doProcess = true;
        _elapsedTime = 0.0f;
        UpdateBlendShapes(0f);
        gameObject.SetActive(true);

        // 対象のマテリアルからキーを抽出
        string targetName = catalystObj.GetComponent<MeshRenderer>().material.name;
        TryGetKey tryGetKey = new TryGetKey();
        string targetKey = tryGetKey.GetKey(targetName);
        if(targetKey == null) return;

        Texture2D matTexture = Texture2D.whiteTexture;
        // スプライトからテクスチャを作成
        Dictionary<string, Sprite> croppedImages = _materialBunker.CroppedImages;
        if (croppedImages.TryGetValue(targetKey, out Sprite targetSprite))
        {
            matTexture = new Texture2D(targetSprite.texture.width, targetSprite.texture.height, TextureFormat.RGBA32, false);
            matTexture.SetPixels(targetSprite.texture.GetPixels());
            matTexture.Apply();

            _material.SetTexture("_BaseMap", matTexture);
        }
        else
        {
            Debug.LogError("Target sprite not found in croppedImages dictionary.");
        }
    
        // Pixmateの生成処理
        Vector3 insPos = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        Quaternion insRot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180f, transform.rotation.eulerAngles.z);
        float scale = PixmatesManager.INITIAL_SCALE_FOX;
        Vector3 insScale = new Vector3(scale, scale, scale);
        _insPixmate = _pixmateManager.InstantiatePixmate(insPos, insRot, insScale, matTexture);
        // マテリアルの生成手続き
        _pixmateManager.SpawnPixmate(matTexture);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_doProcess) return;
        _elapsedTime += Time.deltaTime;
        if(REQUIRE_TIME < _elapsedTime)
        {
            _doProcess = false;
            StartCoroutine("OpenEggAnim");
        }
    }

    // 卵を展開するアニメーション
    IEnumerator OpenEggAnim()
    {
        float startTime = Time.time;

        while (Time.time - startTime < DURATION)
        {
            float animProgress = (Time.time - startTime) / DURATION;
            float blendShapeWeight = Mathf.Lerp(0, MAX_WEIGHT, animProgress);

            UpdateBlendShapes(blendShapeWeight);

            yield return null;
        }

        UpdateBlendShapes(MAX_WEIGHT);
        
        // Pixmateの動き出し
        _pixmateManager.ActivationPixmate(_insPixmate.GetComponent<FoxEcology>());
        // 処理の終了
        yield return new WaitForSeconds(10.0f);
        gameObject.SetActive(false);
    }

    void UpdateBlendShapes(float weight)
    {
        _skinnedMeshRenderer.SetBlendShapeWeight(0, weight);
    }
}
