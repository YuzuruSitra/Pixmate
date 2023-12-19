using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignMaterial
{
    ObjectManipulator _objectManipulator;
    PredictionAdjuster _predictionAdjuster;
    MaterialBunker _materialBunker;
    // ワールドデータの保存
    WorldManager _worldManager;
    
    public AssignMaterial(ObjectManipulator objectManipulator, PredictionAdjuster predictionAdjuster)
    {
        _objectManipulator = objectManipulator;
        _predictionAdjuster = predictionAdjuster;
        _materialBunker = MaterialBunker.InstanceMatBunker;
        _worldManager = WorldManager.InstanceWorldManager;
    }

    public void DoAssignMat()
    {
        GameObject targetObj = _predictionAdjuster.NowHaveCube;
        if(_materialBunker.NowHavePhotoMaterial == null || targetObj == null || !_predictionAdjuster.InLange) return;
        _objectManipulator.AssignMaterial(targetObj, _materialBunker.NowHavePhotoMaterial);
        
        // ワールドデータの保存
        _worldManager.ChangeObjSaving(targetObj);
    }
}
