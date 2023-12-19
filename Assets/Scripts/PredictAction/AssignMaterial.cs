using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignMaterial
{
    PredictManager _predictManager;
    MaterialBunker _materialBunker;
    // ワールドデータの保存
    WorldManager _worldManager;
    
    public AssignMaterial()
    {
        _predictManager = PredictManager.InstancePredictManager;
        _materialBunker = MaterialBunker.InstanceMatBunker;
        _worldManager = WorldManager.InstanceWorldManager;
    }

    public void DoAssignMat()
    {
        GameObject targetObj = _predictManager.NowHaveCube;
        if(_materialBunker.NowHavePhotoMaterial == null || targetObj == null || !_predictManager.InLange) return;
        _predictManager.AssignMaterial(_materialBunker.NowHavePhotoMaterial);
        
        // ワールドデータの保存
        _worldManager.ChangeObjSaving(targetObj);
    }
}
