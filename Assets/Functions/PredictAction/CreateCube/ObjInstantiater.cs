using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInstantiater : MonoBehaviour
{   
    // オブジェクトの生成
    public void GenerateCube()
    {
        PredictManager _predictManager = PredictManager.InstancePredictManager;
        ItemBunker _itemBunker = ItemBunker.InstanceItemBunker;
        
        if(_itemBunker.NowHaveItemObject == null || _predictManager.AdjCubePos == null || !_predictManager.InLange)return;
        Instantiate(_itemBunker.NowHaveItemObject, _predictManager.AdjCubePos, Quaternion.identity);
    }

    
}
