using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PixmateSpawn : MonoBehaviour
{
    private PredictManager _predictManager;
    private MaterialBunker _materialBunker;
    private SkinnedMeshRenderer[] _skinnedMeshRenderer = new SkinnedMeshRenderer[2];
    [SerializeField]
    private Material _material;
    
    // Start is called before the first frame update
    void Start()
    {
        _predictManager = PredictManager.InstancePredictManager;
        _materialBunker = MaterialBunker.InstanceMatBunker;
        _skinnedMeshRenderer[0] = GetComponent<SkinnedMeshRenderer>();
        _skinnedMeshRenderer[1] = transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();
        //_material = GetComponent<Renderer>().material;

        for(int i = 0; i < 2; i++) _skinnedMeshRenderer[i].SetBlendShapeWeight(0, 0f);
    }

    // Pixmateを生成時に呼ぶ
    void DoAppear()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        float blinkCount = Mathf.Clamp01(Mathf.Sin(Time.time)) * 100f;
        for(int i = 0; i < 2; i++) _skinnedMeshRenderer[i].SetBlendShapeWeight(0, blinkCount);
        DoAppear();
    }
}
