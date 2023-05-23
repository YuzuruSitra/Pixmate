using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBunker : MonoBehaviour
{
    // 他スクリプトでも呼べるようにインスタンス化
    public static CubeBunker InstanceCubeBunker;
    // 手持ちのSprite保持
    public GameObject NowHaveCube;

    void Awake()
    {
        if (InstanceCubeBunker == null)
        {
            InstanceCubeBunker = this;
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
