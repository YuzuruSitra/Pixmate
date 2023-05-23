using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignMaterial : MonoBehaviour
{

    public void DoAssignMat()
    {
        GameObject _assignCube = CubeBunker.InstanceCubeBunker.NowHaveCube;
        if(MaterialBunker.InstanceMatBunker.NowHaveMaterial == null)return;
        if(CubeBunker.InstanceCubeBunker.NowHaveCube == null)return;
        _assignCube.GetComponent<MeshRenderer>().material = MaterialBunker.InstanceMatBunker.NowHaveMaterial;
    }
}
