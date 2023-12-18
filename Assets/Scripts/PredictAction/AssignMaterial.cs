using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignMaterial
{
    public void DoAssignMat()
    {
        PredictManager _predictManager = PredictManager.InstancePredictManager;
        MaterialBunker _materialBunker = MaterialBunker.InstanceMatBunker;
        GameObject targetObj = _predictManager.NowHaveCube;
        if(_materialBunker.NowHavePhotoMaterial == null || targetObj == null || !_predictManager.InLange) return;
        _predictManager.AssignMaterial(_materialBunker.NowHavePhotoMaterial);
        
        // ワールドデータの保存
        WorldManager _worldManager = WorldManager.InstanceWorldManager;
        _worldManager.ChangeObjSaving(targetObj);
    }
}
