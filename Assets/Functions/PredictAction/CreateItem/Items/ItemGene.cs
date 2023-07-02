using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGene : MonoBehaviour
{
    public void GenerateMate(Vector3 createPos, GameObject createObj, GameObject convertObj)
    {
        if(createPos == null || convertObj == null)return;
        Destroy(convertObj);
        Quaternion rotationQuaternion = Quaternion.Euler(0f, 180f, 0f);
        Instantiate(createObj, createPos, rotationQuaternion);
    }
}
