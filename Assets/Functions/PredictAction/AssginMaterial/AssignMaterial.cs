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
        if(_materialBunker.NowHavePhotoMaterial == null || _predictManager.NowHaveCube == null || !_predictManager.InLange) return;
        _predictManager.AssignMaterial(_materialBunker.NowHavePhotoMaterial);
    }
}
