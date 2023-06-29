using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInstantiater : MonoBehaviour
{   
    // オブジェクトの生成
    public void GenerateDone()
    {
        // 必要なデータのインスタンス化
        PredictManager _predictManager = PredictManager.InstancePredictManager;
        ItemBunker _itemBunker = ItemBunker.InstanceItemBunker;
        GameObject targetObj = _itemBunker.NowHaveItemObject;
        GameObject rayHitObj = _predictManager.NowHaveCube;
        bool targetInLange = _predictManager.InLange;
        Vector3 rootPos;

        // アイテムが指定されていない且つ対象が射程外なら処理を終了
        if(targetObj == null || !targetInLange)return;

        string currentItem = _itemBunker.NowHaveItem;
        switch(currentItem)
        {
            case "Cube":
                rootPos = _predictManager.AdjCubePos;
                GenerateCube(rootPos,targetObj);
                break;
            case "Gene":
                rootPos = _predictManager.SameCubePos;
                GenerateMate(rootPos,targetObj,rayHitObj);
                break;
            default:
                break;
        }
    }

    void GenerateCube(Vector3 createPos, GameObject createObj)
    {
        if(createPos == null)return;
        Instantiate(createObj, createPos, Quaternion.identity);
    }

    void GenerateMate(Vector3 createPos, GameObject createObj, GameObject convertObj)
    {
        if(createPos == null || convertObj == null)return;
        Destroy(convertObj);
        Quaternion rotationQuaternion = Quaternion.Euler(0f, 180f, 0f);
        Instantiate(createObj, createPos, rotationQuaternion);
    }
}
