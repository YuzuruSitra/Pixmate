using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// テクスチャとマテリアルの管理
public class MaterialBunker : MonoBehaviour
{
    // 他スクリプトでも呼べるようにインスタンス化
    public static MaterialBunker InstanceMatBunker;

    private const string KEY_NAME = "MaterialNo.";
    public string KeyName => KEY_NAME;
    public int MatCount = 0;
    public const int MATERIAL_AMOUNT = 500;
    public Material[] Materials = new Material[MATERIAL_AMOUNT];
    // 手持ちのSprite保持
    public Sprite NowHaveSprite;
    public Material NowHaveMaterial;
    // のちのちセーブと紐付け
    public Dictionary<string, Sprite> CroppedImages = new Dictionary<string, Sprite>();
    public Dictionary<string, Material> ImageMaterials = new Dictionary<string, Material>();


    void Awake()
    {
        if (InstanceMatBunker == null)
        {
            InstanceMatBunker = this;
        }
    }

    void Start()
    {
        setMaterials();
        SpritesAssignMat();
    }

    // マテリアルを辞書にセット
    void setMaterials()
    {
        for(int i=0; i < MATERIAL_AMOUNT; i++)
        {
            int nameCount = i + 1;
            string tmpKey = KeyName;
            tmpKey += nameCount;
            ImageMaterials.Add(tmpKey, Materials[i]);
        }
    }

    // スプライトの追加
    public void AddSprites(Sprite setSprite)
    {
        string tmpKey = KeyName;
        MatCount += 1;
        string tmp = tmpKey + MatCount;
        CroppedImages.Add(tmp, setSprite);
        SpritesAssignMat();
    }

    // マテリアルにスプライトをセット
    void SpritesAssignMat()
    {   
        int i = 0;
        while(i < MatCount)
        {
            int nameCount = i + 1;
            string tmpKey = KeyName;
            tmpKey += nameCount;

            // スプライトからテクスチャを作成
            Texture2D matTexture = new Texture2D(CroppedImages[tmpKey].texture.width, CroppedImages[tmpKey].texture.height, TextureFormat.RGBA32, false);
            matTexture.SetPixels(CroppedImages[tmpKey].texture.GetPixels());
            matTexture.Apply();
            // マテリアルにテクスチャを割り当てる
            ImageMaterials[tmpKey].SetTexture("_MainTex", matTexture);
                        
            i++; 
        }
    }

}
