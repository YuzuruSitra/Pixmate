using UnityEngine;

// ブロックのマテリアル変更を担当するクラス
public class MaterialAssigner
{
    ObjectManipulator _objectManipulator;
    PredictionAdjuster _predictionAdjuster;
    MaterialBunker _materialBunker;
    
    public MaterialAssigner(ObjectManipulator objectManipulator, PredictionAdjuster predictionAdjuster)
    {
        _objectManipulator = objectManipulator;
        _predictionAdjuster = predictionAdjuster;
        _materialBunker = MaterialBunker.InstanceMatBunker;
    }

    public GameObject DoAssignMat()
    {
        GameObject targetObj = _predictionAdjuster.NowHaveCube;
        if(_materialBunker.NowHavePhotoMaterial == null || targetObj == null || !_predictionAdjuster.InLange) return null;
        targetObj.GetComponent<MeshRenderer>().material = _materialBunker.NowHavePhotoMaterial;        
        return targetObj;
    }
}
