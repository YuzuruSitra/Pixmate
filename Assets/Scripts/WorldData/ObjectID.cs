using UnityEngine;

// オブジェクトに固有のIDを付与するクラス
public class ObjectID : MonoBehaviour
{
    [SerializeField]
    private int _thisObjID;
    public int ThisObjID => _thisObjID;
    
    public void SetObjID(int id)
    {
        _thisObjID = id;
    }

}
