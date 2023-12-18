using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBunker : MonoBehaviour
{
    // 他スクリプトでも呼べるようにインスタンス化
    public static ItemBunker InstanceItemBunker;
    // 今保有しているアイテムの数
    private int _havingItemCount = 5;
    public int HavingItemCount => _havingItemCount;
    private string[] _itemName = new string[]
    {
        "Cube",
        "HalfCube",
        "Step",
        "SmallCube",
        "Gene"
    };
    public string[] ItemName => _itemName;
    [SerializeField]
    private Sprite[] _itemSprite = new Sprite[5];
    [SerializeField]
    private GameObject[] _itemObject = new GameObject[5];

    public GameObject[] ItemObject => _itemObject; 
    Dictionary<string, Sprite> _itemSpriteDictionary = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> ItemSpriteDictionary => _itemSpriteDictionary;
    Dictionary<string, GameObject> _itemObjectDictionary = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> ItemObjectDictionary => _itemObjectDictionary;
    // 今所有しているもの
    public string NowHaveItem = "Cube";
    public Sprite NowHaveItemSprite => _itemSpriteDictionary[NowHaveItem];
    public GameObject NowHaveItemObject => _itemObjectDictionary[NowHaveItem];

    void Awake()
    {
        if (InstanceItemBunker == null)
        {
            InstanceItemBunker = this;
        }

        for(int i = 0; i < _havingItemCount; i++)
        {
            _itemSpriteDictionary.Add(_itemName[i], _itemSprite[i]);
            _itemObjectDictionary.Add(_itemName[i], _itemObject[i]);
        }
    }
}
