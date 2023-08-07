using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public int WorldEdgeXMax = 5;
    public int WorldEdgeXMin = -5;
    public int WorldEdgeZMax = 5;
    public int WorldEdgeZMin = -5;

    public static WorldManager InstanceWorldManager;

    void Awake()
    {
        if (InstanceWorldManager == null)
        {
            InstanceWorldManager = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
