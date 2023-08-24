using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PixmateSpawn : MonoBehaviour
{
    private PixmatesManager _pixmateManager;
    private PredictManager _predictManager;
    private MaterialBunker _materialBunker;
    private SkinnedMeshRenderer[] _skinnedMeshRenderer = new SkinnedMeshRenderer[2];
    [SerializeField]
    private Material _material;

    // 生成するPixmate
    [SerializeField]
    private GameObject _pixmatePrefab;
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
        _pixmateManager = PixmatesManager.InstancePixmatesManager;
        _predictManager = PredictManager.InstancePredictManager;
        _materialBunker = MaterialBunker.InstanceMatBunker;
        _skinnedMeshRenderer[0] = GetComponent<SkinnedMeshRenderer>();
        _skinnedMeshRenderer[1] = transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();
        // 仮置き
        Invoke("DoAppear",5.0f);
    }

    // Pixmateを生成時に呼ぶ
    void DoAppear()
    {
        // 初期化
        _doProcess = true;
        _elapsedTime = 0.0f;
        UpdateBlendShapes(0f);
        gameObject.SetActive(true);

        // 対象のマテリアルからキーを抽出
        string targetName = _predictManager.HaveCubeMatKey;
        TryGetKey tryGetKey = new TryGetKey();
        string targetKey = tryGetKey.GetKey(targetName);
        if(targetKey == null) return;

        // スプライトからテクスチャを作成
        Dictionary<string, Sprite> croppedImages = _materialBunker.CroppedImages;
        if (croppedImages.TryGetValue(targetKey, out Sprite targetSprite))
        {
            Texture2D matTexture = new Texture2D(targetSprite.texture.width, targetSprite.texture.height, TextureFormat.RGBA32, false);
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
        Quaternion insRot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90f, transform.rotation.eulerAngles.z);
        _insPixmate = Instantiate(_pixmatePrefab, insPos, insRot);
        _insPixmate.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
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
        for (int i = 0; i < _skinnedMeshRenderer.Length; i++) _skinnedMeshRenderer[i].SetBlendShapeWeight(0, weight);
    }
}
