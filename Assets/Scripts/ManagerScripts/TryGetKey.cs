using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

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
        return targetKey;
    }

    // 何番目のマテリアルか取得
    public int GetMatNumber(string targetMatName)
    {
        int outNum;
        string numericPart = Regex.Match(targetMatName, @"\d+").Value;
        int.TryParse(numericPart, out outNum);
        outNum -= 1;
        if(outNum < 0) outNum = 0;
        return outNum;
    }
}
