using UnityEngine;
using System.Collections.Generic;

// ワールドデータのScriptableObject用クラス
[CreateAssetMenu(fileName = "WorldData", menuName = "ScriptableObjects/WorldData")]
public class DefaultWorldData : ScriptableObject
{
    public List<UnityEngine.Vector3> ObjPositions = new List<UnityEngine.Vector3>();
    public List<UnityEngine.Quaternion> ObjRotations = new List<UnityEngine.Quaternion>();
    public List<string> ObjTags = new List<string>();
    public List<int> ObjKinds = new List<int>();
    public int WorldObjCount;
}