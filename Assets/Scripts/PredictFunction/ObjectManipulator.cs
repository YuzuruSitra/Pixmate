using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    public GameObject GenerateCube(Vector3 createPos, GameObject createObj)
    {
        if(createPos == null) return null;
        return Instantiate(createObj, createPos, Quaternion.identity);
    }

    public void InventCube(GameObject targetObj)
    {
        Destroy(targetObj);
    }
}
