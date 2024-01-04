using System.Collections.Generic;
using UnityEngine;

// テクスチャとマテリアルのデータ保持
public class MaterialBunker : MonoBehaviour
{
    [SerializeField]
    private MaterialIO _materialIO;

    public const string KEY_NAME = "MaterialNo.";
    public int MatCount = 0;
    public const int MATERIAL_AMOUNT = 500;
    public Material[] Materials = new Material[MATERIAL_AMOUNT];
    // セーブと紐付け
    public Dictionary<string, Sprite> CroppedImages = new Dictionary<string, Sprite>();
    public Dictionary<string, Material> ImageMaterials = new Dictionary<string, Material>();
    public Dictionary<string, string> ImageNames = new Dictionary<string, string>();
    // 初期アセット
    public Material[] DefaultMat = new Material[5];

    // 今所有しているもの
    private string _nowHavePhoto = "MaterialNo.1";
    public string NowHavePhoto => _nowHavePhoto;
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

    void Start()
    {
        // ロード処理
        MatCount = _materialIO.LoadCountMat();
        for(int i = 0; i < MatCount; i++)
        {
            int nameCount = i + 1;
            string addTmpKey = KEY_NAME + nameCount;

            // CroppedImagesに追加。
            Sprite tmpPhotoSpriteValue = _materialIO.LoadMaterialSprite(addTmpKey);
            if(tmpPhotoSpriteValue != null) CroppedImages.Add(addTmpKey, tmpPhotoSpriteValue);

            // ImageNamesに追加。
            string tmpPhotoNamesValue = _materialIO.LoadMaterialSpriteName(addTmpKey);
            if(tmpPhotoNamesValue != null) ImageNames.Add(addTmpKey, tmpPhotoNamesValue);
        }

        SetMaterials();
        SpritesAssignMat();

        // ワールドのロード処理
        WorldManager worldManager = WorldManager.InstanceWorldManager; 
        worldManager.WorldLoad();
    }

    // スプライトの追加
    public void AddDictionarySprites(string key, Sprite setSprite)
    {
        MatCount += 1;
        CroppedImages.Add(key, setSprite);
        ImageNames.Add(key, key);
        SpritesAssignMat();
    }

    // 写真の名前を変更
    public void ChangePhotoName(string tmpName)
    {
        ImageNames[NowHavePhoto] = tmpName;
        // SpriteNameのセーブ
        _materialIO.DoSaveSprite(MatCount,CroppedImages[NowHavePhoto],ImageNames[NowHavePhoto],NowHavePhoto);
    }

    // マテリアルにスプライトをセット
    void SpritesAssignMat()
    {
        int i = 0;
        while (i < MatCount)
        {
            int nameCount = i + 1;
            string tmpKey = KEY_NAME;
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

    public void SetNowHavePhoto(string newPhoto)
    {
        _nowHavePhoto = newPhoto;
    }

    // マテリアルを辞書にセット
    void SetMaterials()
    {
        for(int i = 0; i < MATERIAL_AMOUNT; i++)
        {
            int nameCount = i + 1;
            string tmpKey = KEY_NAME;
            tmpKey += nameCount;
            ImageMaterials.Add(tmpKey, Materials[i]);
        }
    }

    // 特定スプライトの削除時の辞書要素並び替えとセーブ処理
    public void DeleteSortDictionary()
    {
        if(NowHavePhoto == null) return;
        // 削除したい画像のKeyを取得
        string tmp = System.Text.RegularExpressions.Regex.Replace(NowHavePhoto, @"[^0-9]", "");
        int tmpInt = int.Parse(tmp);

        // 画像を格納している辞書を削除したい画像から繰り上げる。
        while (tmpInt <= MatCount)
        {
            string addNewKey = KEY_NAME + tmpInt;
            string nextKey = KEY_NAME + (tmpInt + 1);

            if(tmpInt < MatCount)
            {
                // 辞書から現在のキーの要素を取得
                Sprite currentSprite = CroppedImages[nextKey];
                Material currentMaterial = ImageMaterials[nextKey];

                // 辞書に新しいキーで要素を追加
                CroppedImages[addNewKey] = currentSprite;
                ImageMaterials[addNewKey] = currentMaterial;
            }
            else
            {
                // 辞書の最後の要素を削除
                CroppedImages.Remove(addNewKey);
                ImageNames.Remove(addNewKey);
                MatCount -= 1;
            }
            tmpInt++;
        }
        
        // 選択中の画像を変更
        _nowHavePhoto = "MaterialNo.1";

        // 保存する処理
    
        // PhotoSpriteとPhotoNameの書き込み
        for(int i = 0; i < MatCount; i++)
        {
            int nameCount = i + 1;
            string pullTmpKey = KEY_NAME + nameCount;
            Sprite tmpPhotoSpriteValue = CroppedImages[pullTmpKey];
            string tmpPhotoNamesValue = ImageNames[pullTmpKey];
            _materialIO.DoSaveSprite(MatCount, tmpPhotoSpriteValue, tmpPhotoNamesValue, pullTmpKey);
        }
    }
}
