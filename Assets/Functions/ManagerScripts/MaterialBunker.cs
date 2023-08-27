using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// テクスチャとマテリアルの管理
public class MaterialBunker : MonoBehaviour
{
    // 他スクリプトでも呼べるようにインスタンス化
    public static MaterialBunker InstanceMatBunker;
    SaveManager _saveManager;

    public const string KEY_NAME = "MaterialNo.";
    public string KeyName => KEY_NAME;
    public int MatCount = 0;
    public const int MATERIAL_AMOUNT = 500;
    public Material[] Materials = new Material[MATERIAL_AMOUNT];
    // セーブと紐付け
    public Dictionary<string, Sprite> CroppedImages = new Dictionary<string, Sprite>();
    public Dictionary<string, Material> ImageMaterials = new Dictionary<string, Material>();
    public Dictionary<string, string> ImageNames = new Dictionary<string, string>();

    // 今所有しているもの
    public string NowHavePhoto = "MaterialNo.1";
    public Sprite NowHavePhotoSprite
    {
        get { return MatCount != 0 && NowHavePhoto != null ? CroppedImages[NowHavePhoto] : null; }
    }
    public Material NowHavePhotoMaterial
    {
        get { return MatCount != 0 && NowHavePhoto != null ? ImageMaterials[NowHavePhoto] : null; }
    }
    public string NowHavePhotoNames
    {
        get { return MatCount != 0 && NowHavePhoto != null ? ImageNames[NowHavePhoto] : null; }
    }


    void Awake()
    {
        if (InstanceMatBunker == null)
        {
            InstanceMatBunker = this;
        }
    }

    void Start()
    {
        SaveManager _saveManager = SaveManager.InstanceSaveManager;
        // ロード処理
        MatCount = _saveManager.LoadCountMat();
        for(int i = 0; i < MatCount; i++)
        {
            int nameCount = i + 1;
            string addTmpKey = KEY_NAME + nameCount;

            // CroppedImagesに追加。
            Sprite tmpPhotoSpriteValue = _saveManager.LoadMaterialSprite(addTmpKey);
            CroppedImages.Add(addTmpKey, tmpPhotoSpriteValue);

            // ImageNamesに追加。
            string tmpPhotoNamesValue = _saveManager.LoadMaterialSpriteName(addTmpKey);
            ImageNames.Add(addTmpKey, tmpPhotoNamesValue);
        }

        SetMaterials();
        SpritesAssignMat();
    }

    // スプライトの追加
    public void AddSprites(Sprite setSprite)
    {
        string tmpKey = KeyName;
        MatCount += 1;
        string tmp = tmpKey + MatCount;
        CroppedImages.Add(tmp, setSprite);
        ImageNames.Add(tmp, tmp);
        SaveManager instanceSaveManager = SaveManager.InstanceSaveManager;
        instanceSaveManager.DoSaveSprite(MatCount,setSprite,tmp,tmp);
        SpritesAssignMat();
    }

    // 写真の名前を変更
    public void ChangePhotoName(string tmpName)
    {
        ImageNames[NowHavePhoto] = tmpName;
        SaveManager instanceSaveManager = SaveManager.InstanceSaveManager;
        instanceSaveManager.DoSaveSprite(MatCount,CroppedImages[NowHavePhoto],ImageNames[NowHavePhoto],NowHavePhoto);
    }

    // マテリアルにスプライトをセット
    void SpritesAssignMat()
    {
        int i = 0;
        while (i < MatCount)
        {
            int nameCount = i + 1;
            string tmpKey = KeyName;
            tmpKey += nameCount;

            // スプライトからテクスチャを作成
            Texture2D matTexture = new Texture2D(CroppedImages[tmpKey].texture.width, CroppedImages[tmpKey].texture.height, TextureFormat.RGBA32, false);
            matTexture.SetPixels(CroppedImages[tmpKey].texture.GetPixels());
            matTexture.Apply();

            // マテリアルにテクスチャを割り当てる
            ImageMaterials[tmpKey].SetTexture("_BaseMap", matTexture);

            i++;
        }
    }

    /*--------------------------------*/

    // マテリアルを辞書にセット
    void SetMaterials()
    {
        for(int i = 0; i < MATERIAL_AMOUNT; i++)
        {
            int nameCount = i + 1;
            string tmpKey = KeyName;
            tmpKey += nameCount;
            ImageMaterials.Add(tmpKey, Materials[i]);
        }
    }

    // 仮置き
    public void SaveMat()
    {
        SaveManager instanceSaveManager = SaveManager.InstanceSaveManager;
        instanceSaveManager.DoSaveMaterial();
    }

}
