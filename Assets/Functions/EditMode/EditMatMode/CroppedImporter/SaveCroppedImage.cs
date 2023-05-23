using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SaveCroppedImage : MonoBehaviour
{

    [SerializeField]
    private CropImage _cropImage;

    public void AddCroppedSprite()
    {

        if (_cropImage.CroppedTexture == null) return;
        
        MaterialBunker materialBunker = MaterialBunker.InstanceMatBunker;

        string path = Path.Combine(Application.dataPath, "Resources/CroppedImages");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // 保存するマテリアルのパスを設定
        int imageIndex = materialBunker.CroppedImages.Count + 1;
        string fileName = $"CroppedImage_{imageIndex}.png";
        string filePath = Path.Combine(path, fileName);


        byte[] pngData = _cropImage.CroppedTexture.EncodeToPNG();
        File.WriteAllBytes(filePath, pngData);

        // Debug.Log($"Cropped image saved at: {filePath}");

        Sprite newSprite = Sprite.Create(_cropImage.CroppedTexture, new Rect(0f, 0f, _cropImage.CroppedTexture.width, _cropImage.CroppedTexture.height), new Vector2(0.5f, 0.5f));
        //スプライトを保存
        materialBunker.AddSprites(newSprite);
        
    }
}
