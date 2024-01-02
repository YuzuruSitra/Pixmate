using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイテムの名前と説明のオブジェクト
[CreateAssetMenu]
public class ItemInformation : ScriptableObject
{
    [SerializeField]
    string cubeName = "Cube";
    public string CubeName { get { return cubeName; } }
    [SerializeField]
    string cubeExp = "スタンダードなキューブ \nキューブを8使うことで生成可能";
    public string CubeExp { get { return cubeExp; } }

    [SerializeField]
    string halfCubeName = "HalfCube";
    public string HalfCubeName { get { return halfCubeName; } }
    [SerializeField]
    string halfCubeExp = "ハーフサイズのオブジェクト \nキューブを4使うことで生成可能";
    public string HalfCubeExp { get { return halfCubeExp; } }

    [SerializeField]
    string stepCubeName = "StepCube";
    public string StepCubeName { get { return stepCubeName; } }
    [SerializeField]
    string stepCubeExp = "階段型のオブジェクト \nキューブを6使うことで生成可能";
    public string StepCubeExp { get { return stepCubeExp; } }

    [SerializeField]
    string smallCubeName = "SmallCube";
    public string SmallCubeName { get { return smallCubeName; } }
    [SerializeField]
    string smallCubeExp = "棒状のオブジェクト \nキューブを2使うことで生成可能";
    public string SmallCubeExp { get { return smallCubeExp; } }

    [SerializeField]
    string pixGenName = "PixGen";
    public string PixGenName { get { return pixGenName; } }
    [SerializeField]
    string pixGenExp = "完全なキューブに\n生命を与える遺伝子";
    public string PixGenExp { get { return pixGenExp; } }
}
