using UnityEngine;
using System.IO;
using System.Diagnostics;

public class MaterialCreator : MonoBehaviour
{
    /*
    public Color materialColor = Color.white;


    [Conditional("UNITY_EDITOR")]
    void Start()
    {
        CreateAndSaveMaterial(materialColor);
    }

    void CreateAndSaveMaterial(Color color)
    {
        // Resourcesフォルダ内のMaterialsフォルダへのパスを取得
        string path = Path.Combine(Application.dataPath, "Resources/Materials");
        int i = 0;

        // ファイル名が見つかった場合、新しいマテリアルをアセットとして保存
        while (i < 500)
        {
            i++;
            string name = $"CroppedImageMat_{i}";
            // 新しいマテリアルを作成
            Material newMaterial = CreateMaterial(name, color);
            UnityEditor.AssetDatabase.CreateAsset(newMaterial, "Assets/Resources/Materials/" + name + ".mat");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }
    }

    Material CreateMaterial(string name, Color color)
    {
        // 新しいマテリアルを作成
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.color = color;
        newMaterial.name = name;

        return newMaterial;
    }
    */
}



