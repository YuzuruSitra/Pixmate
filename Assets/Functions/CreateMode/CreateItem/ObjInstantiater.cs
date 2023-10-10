using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInstantiater : MonoBehaviour
{   
    // ワールドマネージャー
    WorldManager _worldManager;
    // 各種アイテムのクラス
    [SerializeField]
    private ItemCube _itemCube;
    [SerializeField]
    private PixmateGenerate _pixmateGenerate;

    void Start()
    {
        _worldManager = WorldManager.InstanceWorldManager;
    }

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
        GameObject insObj = null;
        switch(currentItem)
        {
            case "Cube":
                rootPos = _predictManager.AdjCubePos;
                insObj = _itemCube.GenerateCube(rootPos,targetObj);
                _worldManager.InsObjSaving(insObj);
                break;
            case "HalfCube":
                rootPos = _predictManager.AdjCubePos;
                insObj = _itemCube.GenerateCube(rootPos,targetObj);
                _worldManager.InsObjSaving(insObj);
                break;
            case "Step":
                rootPos = _predictManager.AdjCubePos;
                insObj = _itemCube.GenerateCube(rootPos,targetObj);
                _worldManager.InsObjSaving(insObj);
                break;
            case "SmallCube":
                rootPos = _predictManager.AdjCubePos;
                insObj = _itemCube.GenerateCube(rootPos,targetObj);
                _worldManager.InsObjSaving(insObj);
                break;
            case "Gene":
                rootPos = _predictManager.SameCubePos;
                _pixmateGenerate.GenerateMate(rootPos,targetObj,rayHitObj);
                _worldManager.DeleteObjSaving(rayHitObj);
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

        // アイテムが指定されていない且つ対象が射程外なら処理を終了
        if(targetObj == null || !targetInLange)return;

        string currentItem = _itemBunker.NowHaveItem;
        switch(currentItem)
        {
            case "Cube":
                _worldManager.DeleteObjSaving(rayHitObj);
                _itemCube.InventCube(rayHitObj);
                break;
            case "HalfCube":
                _worldManager.DeleteObjSaving(rayHitObj);
                _itemCube.InventCube(rayHitObj);
                break;
            case "Step":
                _worldManager.DeleteObjSaving(rayHitObj);
                _itemCube.InventCube(rayHitObj);
                break;
            case "SmallCube":
                _worldManager.DeleteObjSaving(rayHitObj);
                _itemCube.InventCube(rayHitObj);
                break;
            case "Gene":

                break;
            default:
                break;
        }
    }
}
