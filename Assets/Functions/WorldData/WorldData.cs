using UnityEngine;
using System.Collections.Generic;
using System.Numerics;

[CreateAssetMenu(fileName = "WorldData", menuName = "ScriptableObjects/WorldData")]
public class WorldData : ScriptableObject
{
    public List<UnityEngine.Vector3> objPositions = new List<UnityEngine.Vector3>();
    public List<UnityEngine.Quaternion> objRotations = new List<UnityEngine.Quaternion>();
    public List<string> objtags = new List<string>();
    public List<int> objKinds = new List<int>();
}