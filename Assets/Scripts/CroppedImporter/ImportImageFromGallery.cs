using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

// データフォルダから画像を読み込むクラス
public class ImportImageFromGallery : MonoBehaviour
{
    // インポート成否イベント
    public event Action<bool> IsImportedSuccess;

    // 画像インポート処理
    public void ImportImage(GameObject importImageFlame)
    {
        NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

        if (permission == NativeGallery.Permission.Granted)
        {
            StartCoroutine(ImportImageCoroutine(importImageFlame));
        }
        else
        {
            NativeGallery.RequestPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);
        }

    }

    private IEnumerator ImportImageCoroutine(GameObject importImageFlame)
    {
        string imagePath = null;
        
        NativeGallery.GetImageFromGallery((path) =>
        {
            imagePath = path;
            
            // 画像を選択した場合
            if (imagePath != null)
            {
 
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(File.ReadAllBytes(imagePath));

                Texture2D jpegTexture = ConvertToJPEG(texture);

                Sprite sprite = Sprite.Create(jpegTexture, new Rect(0f, 0f, jpegTexture.width, jpegTexture.height), new Vector2(0.5f, 0.5f));

                Image flameImage = importImageFlame.GetComponent<Image>();
                flameImage.sprite = sprite;
                
                // 画像を選択した場合クロップモードへ
                IsSuccess(true);
            }
            else
            {
                // 画像を選択しなかった場合通常モードに戻る
                IsSuccess(false);
            }
        }, "Select an Image", "image/*");

        // コルーチンの完了を示すために必要
        yield return null;
    }

    public void IsSuccess(bool b)
    {
        // 同じステートを弾く
        IsImportedSuccess?.Invoke(b);
    }

    private Texture2D ConvertToJPEG(Texture2D inputTexture, int quality = 75)
    {
        byte[] jpegBytes = ImageConversion.EncodeToJPG(inputTexture, quality);

        Texture2D jpegTexture = new Texture2D(inputTexture.width, inputTexture.height, TextureFormat.RGB24, false);
        jpegTexture.LoadImage(jpegBytes);

        return jpegTexture;
    }
}
