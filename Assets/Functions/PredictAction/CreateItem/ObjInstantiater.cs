using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInstantiater : MonoBehaviour
{   
    // 各種アイテムのクラス
    [SerializeField]
    private ItemCube _itemCube;
    [SerializeField]
    private ItemGene _itemGene;

    // オブジェクトの生成-ボタン1
    public void Generate1()
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
                _itemCube.GenerateCube(rootPos,targetObj);
                break;
            case "Gene":
                rootPos = _predictManager.SameCubePos;
                _itemGene.GenerateMate(rootPos,targetObj,rayHitObj);
                break;
            default:
                break;
        }
    }

     // オブジェクトの生成-ボタン2
    public void Generate2()
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
                _itemCube.InventCube(rayHitObj);
                break;
            case "Gene":

                break;
            default:
                break;
        }
    }
}
