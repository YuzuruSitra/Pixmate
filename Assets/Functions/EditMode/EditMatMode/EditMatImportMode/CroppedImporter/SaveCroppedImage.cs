using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SaveCroppedImage : MonoBehaviour
{
    const int CROP_SIZE = 512;
    [SerializeField]
    private CropImage _cropImage;

    public void AddCroppedSprite(Texture2D croppedTexture)
    {

        if (croppedTexture == null) return;
        
        // 画像のサイズ変換処理
        Texture2D saveTexture = croppedTexture;
        saveTexture = ResizeTexture(saveTexture, CROP_SIZE, CROP_SIZE);


        MaterialBunker materialBunker = MaterialBunker.InstanceMatBunker;

        string path = Path.Combine(Application.dataPath, "Resources/CroppedImages");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        // 保存するマテリアルのパスを設定
        int imageIndex = materialBunker.MatCount + 1;
        string fileName = $"CroppedImage_{imageIndex}.png";
        string filePath = Path.Combine(path, fileName);

        byte[] pngData = saveTexture.EncodeToPNG();
        File.WriteAllBytes(filePath, pngData);

        Sprite newSprite = Sprite.Create(saveTexture, new Rect(0f, 0f, saveTexture.width, saveTexture.height), new Vector2(0.5f, 0.5f));
        //スプライトを保存
        materialBunker.AddSprites(newSprite);
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

}
