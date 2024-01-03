using UnityEngine;

// テクスチャを合成するクラス
public class TextureFusioner
{
    const int TARGET_SIZE = 64;
    const int SUBDIVISIONS = 4;
    int[] _square;

    public Sprite CombineTextures(Texture2D target1, Texture2D target2)
    {
        InitializingArray();

        Texture2D tmpTexture1;
        tmpTexture1 = ResizeTexture(target1, TARGET_SIZE, TARGET_SIZE);
        Color32[] pixelColors1 = tmpTexture1.GetPixels32();

        Texture2D tmpTexture2;
        tmpTexture2 = ResizeTexture(target2, TARGET_SIZE, TARGET_SIZE);
        Color32[] pixelColors2 = tmpTexture2.GetPixels32();
        
        Texture2D completeImage = CombineMaterials(_square ,TARGET_SIZE , pixelColors1, pixelColors2);
        
        // ImageオブジェクトのTextureを設定
        Sprite sprite = Sprite.Create(completeImage, new Rect(0, 0, completeImage.width, completeImage.height), Vector2.one * 0.5f);
        return sprite;
    }


    /*----------------------------------------------------------------*/

    void InitializingArray()
    {
        _square = new int[TARGET_SIZE * TARGET_SIZE];

        for (int x = 0; x < TARGET_SIZE; x++)
        {
            for (int y = 0; y < TARGET_SIZE; y++)
            {
                int index = x + y * TARGET_SIZE;

                int subdivisionX = x / SUBDIVISIONS;
                int subdivisionY = y / SUBDIVISIONS;
                int number = subdivisionY * SUBDIVISIONS + subdivisionX + 1;
                _square[index] = number;
            }
        }
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

    Texture2D CombineMaterials(int[] square ,int maxSize , Color32[] pixPaint1, Color32[] pixPaint2)
    {
        int length = square.Length;
        int[] selectKey = new int[length]; 

        for (int i = 0; i < length; i++)
        {
            selectKey[i] = Random.Range(0, 2);
        }

        Texture2D texture = new Texture2D(maxSize, maxSize);
        for (int x = 0; x < maxSize; x++)
        {
            for (int y = 0; y < maxSize; y++)
            {
                int index = x + y * maxSize;   

                Color color = pixPaint1[index];
                int intsq = square[index] - 1;
                intsq = selectKey[intsq];
                if(intsq == 1) color = pixPaint2[index];

                texture.SetPixel(x, y, color);
            }
        }
        // テクスチャを適用
        texture.Apply();

        return texture;
    }

}
