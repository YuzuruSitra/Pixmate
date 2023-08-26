using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGene : MonoBehaviour
{
    // SpawnObjを対象位置に移動させ起動する処理
    public void GenerateMate(Vector3 targetPos, GameObject spawnObj, GameObject convertObj)
    {
        if(targetPos == null || convertObj == null)return;
    
        spawnObj.transform.position = new Vector3(targetPos.x , targetPos.y - 0.5f, targetPos.z);
        PixmateSpawn pixmateSpawn = spawnObj.GetComponent<PixmateSpawn>();
        pixmateSpawn.LaunchSpawn(convertObj);
        Destroy(convertObj);
    }
}
