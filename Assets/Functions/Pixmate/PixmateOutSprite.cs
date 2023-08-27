using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PixmateOutSprite
{
    const int CROP_SIZE = 512;

    public Sprite AddPixmateSprite(int num, Texture2D croppedTexture)
    {

        if (croppedTexture == null) return null;
        
        // 画像のサイズ変換処理
        Texture2D saveTexture = croppedTexture;
        saveTexture = ResizeTexture(saveTexture, CROP_SIZE, CROP_SIZE);

        string path = Path.Combine(Application.dataPath, "Resources/PixmatesTextures");
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        // 保存するマテリアルのパスを設定
        //int imageIndex = materialBunker.MatCount + 1;
        string fileName = $"PixmatesColor_{num}.png";
        string filePath = Path.Combine(path, fileName);

        byte[] pngData = saveTexture.EncodeToPNG();
        File.WriteAllBytes(filePath, pngData);

        Sprite newSprite = Sprite.Create(saveTexture, new Rect(0f, 0f, saveTexture.width, saveTexture.height), new Vector2(0.5f, 0.5f));
        //スプライトを返す
        return newSprite;
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
