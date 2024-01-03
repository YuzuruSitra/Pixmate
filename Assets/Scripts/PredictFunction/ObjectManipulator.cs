using UnityEngine;

// ブロックの生成、回収を担当するクラス
public class ObjectManipulator : MonoBehaviour
{
    // 対象を生成
    public GameObject GenerateCube(Vector3 createPos, GameObject createObj)
    {
        if(createPos == null) return null;
        return Instantiate(createObj, createPos, Quaternion.identity);
    }

    // 対象を回収
    public void InventCube(GameObject targetObj)
    {
        Destroy(targetObj);
    }
}
