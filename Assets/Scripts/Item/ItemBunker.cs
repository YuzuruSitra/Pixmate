using System;
using System.Collections.Generic;
using UnityEngine;

// アイテムを保持するクラス
public class ItemBunker : MonoBehaviour
{
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
    private string _selectItem = "Cube";
    public string SelectItem => _selectItem;
    public Sprite NowHaveItemSprite => _itemSpriteDictionary[_selectItem];
    public GameObject NowHaveItemObject => _itemObjectDictionary[_selectItem];

    public event Action<String> OnItemChanged;
    void Awake()
    {
        for(int i = 0; i < _havingItemCount; i++)
        {
            _itemSpriteDictionary.Add(_itemName[i], _itemSprite[i]);
            _itemObjectDictionary.Add(_itemName[i], _itemObject[i]);
        }
    }

    public void ChangeItem(string newItem)
    {
        if(_selectItem == newItem) return;
        _selectItem = newItem;
        OnItemChanged?.Invoke(_selectItem);
    }
}
