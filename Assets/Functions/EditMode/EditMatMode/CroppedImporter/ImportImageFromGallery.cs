using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NativeGalleryNamespace;
using System.IO;

public class ImportImageFromGallery : MonoBehaviour
{
    private bool _importSuccess;
    public bool ImportSuccess => _importSuccess;

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
                // Load the image as a texture
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(File.ReadAllBytes(imagePath));

                // Convert the texture to JPEG format
                Texture2D jpegTexture = ConvertToJPEG(texture);

                // Create a sprite from the JPEG texture
                Sprite sprite = Sprite.Create(jpegTexture, new Rect(0f, 0f, jpegTexture.width, jpegTexture.height), new Vector2(0.5f, 0.5f));

                // Set the sprite to the imported image
                Image flameImage = importImageFlame.GetComponent<Image>();
                flameImage.sprite = sprite;
            }
        }, "Select an Image", "image/*");

        if(imagePath != null)
        {
            // 画像を選択した場合クロップモードへ
            _importSuccess = true;
        }
        else
        {
            // 画像を選択しなかった場合通常モードに戻る
            _importSuccess = false;
        }
        yield return null;
    }

    private Texture2D ConvertToJPEG(Texture2D inputTexture, int quality = 75)
    {
        byte[] jpegBytes = ImageConversion.EncodeToJPG(inputTexture, quality);

        Texture2D jpegTexture = new Texture2D(inputTexture.width, inputTexture.height, TextureFormat.RGB24, false);
        jpegTexture.LoadImage(jpegBytes);

        return jpegTexture;
    }
}
