using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryGetKey
{
    // マテリアルの名前からキーを抽出
    public string GetKey(string targetMatName)
    {
        MaterialBunker materialBunker = MaterialBunker.InstanceMatBunker;
        if (string.IsNullOrEmpty(targetMatName) || !targetMatName.Contains("CroppedImageMat_")) return null;

        // "(Instance)" を空文字列に置き換える
        targetMatName = targetMatName.Replace(" (Instance)", "");

        string targetKey = targetMatName.Replace("CroppedImageMat_", materialBunker.KeyName);
        Debug.Log(targetKey);
        return targetKey;
    }
}
