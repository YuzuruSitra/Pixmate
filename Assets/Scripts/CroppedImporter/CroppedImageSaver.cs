using UnityEngine;

// クロップした画像を整えて保存するクラス
public class CroppedImageSaver
{
    const int CROP_SIZE = 512;
    private MaterialBunker _materialBunker;
    private SaveManager _saveManager;

    public CroppedImageSaver()
    {
        _materialBunker = GameObject.FindWithTag("MaterialBunker").GetComponent<MaterialBunker>();
        _saveManager = GameObject.FindWithTag("SaveManager").GetComponent<SaveManager>();
    }

    public void ResizeToSaveSprite(Texture2D croppedTexture)
    {
        if (croppedTexture == null) return;
        
        // 画像のサイズ変換処理
        Texture2D saveTexture = croppedTexture;
        saveTexture = ResizeTexture(saveTexture, CROP_SIZE, CROP_SIZE);

        Sprite newSprite = Sprite.Create(saveTexture, new Rect(0f, 0f, saveTexture.width, saveTexture.height), new Vector2(0.5f, 0.5f));
        //スプライトを保存
        SaveNewSprites(newSprite);
    }

    Texture2D ResizeTexture(Texture2D originalTexture, int width, int height)
    {
        // 新しいテクスチャの作成
        Texture2D resizedTexture = new Texture2D(width, height);

        // オリジナルのピクセルデータをリサイズされたテクスチャにコピー
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color newColor = originalTexture.GetPixelBilinear((float)x / width, (float)y / height);
                resizedTexture.SetPixel(x, y, newColor);
            }
        }
        // テクスチャの適用
        resizedTexture.Apply();

        return resizedTexture;
    }

    // スプライトの追加
    void SaveNewSprites(Sprite setSprite)
    {
        if (_materialBunker == null) return;
        if (_saveManager == null) return;
        
        // キーの発行
        string tmpKey = MaterialBunker.KEY_NAME;
        string key = tmpKey + _materialBunker.MatCount;
        _materialBunker.AddDictionarySprites(key, setSprite);
        
        // Spriteのセーブ
        _saveManager.DoSaveSprite(_materialBunker.MatCount, setSprite, key, key);
    }

}
