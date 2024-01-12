using UnityEngine;

// CreateModeのボタン押下処理
public class ObjInstantiater
{   
    // ワールドマネージャー
    private WorldDataBunker _worldDataBunker;
    private ItemBunker _itemBunker;
    // 各種アイテムのクラス
    private ObjectManipulator _objectManipulator;
    private PixmateGenerate _pixmateGenerate;
    private PredictionAdjuster _predictionAdjuster;

    public ObjInstantiater()
    {
        _worldDataBunker = GameObject.FindWithTag("WorldManager").GetComponent<WorldDataBunker>();
        _itemBunker = GameObject.FindWithTag("Item").GetComponent<ItemBunker>();
        _objectManipulator = GameObject.FindWithTag("PredictFunctions").GetComponent<ObjectManipulator>();
        _pixmateGenerate = GameObject.FindWithTag("PredictFunctions").GetComponent<PixmateGenerate>();
        _predictionAdjuster = GameObject.FindWithTag("PredictFunctions").GetComponent<PredictionAdjuster>();
    }
    
    // オブジェクトの生成-ボタン1
    public void Generate1()
    {
        // 必要なデータのインスタンス化
        GameObject targetObj = _itemBunker.NowHaveItemObject;
        GameObject rayHitObj = _predictionAdjuster.NowHaveCube;
        bool targetInLange = _predictionAdjuster.InLange;
        Vector3 rootPos;

        // アイテムが指定されていない且つ対象が射程外なら処理を終了
        if(targetObj == null || !targetInLange)return;

        string currentItem = _itemBunker.SelectItem;
        GameObject insObj = null;
        switch(currentItem)
        {
            case "Cube":
                rootPos = _predictionAdjuster.AdjCubePos;
                insObj = _objectManipulator.GenerateCube(rootPos,targetObj);
                _worldDataBunker.InsObjSaving(insObj);
                break;
            case "HalfCube":
                rootPos = _predictionAdjuster.AdjCubePos;
                insObj = _objectManipulator.GenerateCube(rootPos,targetObj);
                _worldDataBunker.InsObjSaving(insObj);
                break;
            case "Step":
                rootPos = _predictionAdjuster.AdjCubePos;
                insObj = _objectManipulator.GenerateCube(rootPos,targetObj);
                _worldDataBunker.InsObjSaving(insObj);
                break;
            case "SmallCube":
                rootPos = _predictionAdjuster.AdjCubePos;
                insObj = _objectManipulator.GenerateCube(rootPos,targetObj);
                _worldDataBunker.InsObjSaving(insObj);
                break;
            case "Gene":
                rootPos = _predictionAdjuster.SameCubePos;
                _pixmateGenerate.GenerateMate(rootPos,targetObj,rayHitObj);
                _worldDataBunker.DeleteObjSaving(rayHitObj);
                break;
            default:
                break;
        }

    }

     // オブジェクトの生成-ボタン2
    public void Generate2()
    {
        // 必要なデータのインスタンス化
        GameObject targetObj = _itemBunker.NowHaveItemObject;
        GameObject rayHitObj = _predictionAdjuster.NowHaveCube;
        bool targetInLange = _predictionAdjuster.InLange;

        // アイテムが指定されていない且つ対象が射程外なら処理を終了
        if(targetObj == null || !targetInLange)return;

        string currentItem = _itemBunker.SelectItem;
        switch(currentItem)
        {
            case "Cube":
                _worldDataBunker.DeleteObjSaving(rayHitObj);
                _objectManipulator.InventCube(rayHitObj);
                break;
            case "HalfCube":
                _worldDataBunker.DeleteObjSaving(rayHitObj);
                _objectManipulator.InventCube(rayHitObj);
                break;
            case "Step":
                _worldDataBunker.DeleteObjSaving(rayHitObj);
                _objectManipulator.InventCube(rayHitObj);
                break;
            case "SmallCube":
                _worldDataBunker.DeleteObjSaving(rayHitObj);
                _objectManipulator.InventCube(rayHitObj);
                break;
            case "Gene":

                break;
            default:
                break;
        }
    }
}
