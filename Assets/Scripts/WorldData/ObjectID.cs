using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
