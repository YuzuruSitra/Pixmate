using UnityEngine;

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

    // 対象のマテリアルを変更
    public void AssignMaterial(GameObject target, Material mat)
    {
        target.GetComponent<MeshRenderer>().material = mat;
    }
}
