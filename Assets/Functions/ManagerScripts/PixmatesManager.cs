using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixmatesManager : MonoBehaviour
{
    public static PixmatesManager InstancePixmatesManager;
    // 存在できるPixmateの最大値
    private const int MAX_PIXMATES = 10;
    // 現在いるPixmateの数
    private int _pixmatesCount = 0;
    private GameObject[] _pixmateFoxes;

    void Awake()
    {
        if (InstancePixmatesManager == null)
        {
            InstancePixmatesManager = this;
        }
    }

    void Start()
    {
        // ゲーム起動時に存在しているPixmate達を起動する処理
        _pixmateFoxes = GameObject.FindGameObjectsWithTag("PixmateFox");
        _pixmatesCount = _pixmateFoxes.Length;

        // Pixmateにマテリアルを割り当てる処理
        
        ////////////////////

        // 配列に格納されたPixmateゲームオブジェクトを処理
        foreach (GameObject pixmateObject in _pixmateFoxes)
        {
            ActivationPixmate(pixmateObject.GetComponent<FoxEcology>());
        }
    }

    // Pixmateの動き出し
    public void ActivationPixmate(FoxEcology pixmateEcology)
    {
        pixmateEcology.ComeAlive();
    }

    // Pixmateへのマテリアル割り当て
    // ※管理周りの処理を追加予定
    public void AssignMaterial(GameObject pixmate, Texture2D texture)
    {
        GameObject childObj = pixmate.transform.GetChild(0).gameObject;
        childObj.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_BaseMap", texture);
    }
}
