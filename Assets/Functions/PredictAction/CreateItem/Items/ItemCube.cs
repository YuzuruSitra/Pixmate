using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCube : MonoBehaviour
{
    public void GenerateCube(Vector3 createPos, GameObject createObj)
    {
        if(createPos == null)return;
        Instantiate(createObj, createPos, Quaternion.identity);
    }

    public void InventCube(GameObject targetObj)
    {
        Destroy(targetObj);
    }
}
