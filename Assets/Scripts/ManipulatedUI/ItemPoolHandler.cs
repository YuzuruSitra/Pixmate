using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// アイテム一覧のプール管理クラス
public class ItemPoolHandler : MonoBehaviour
{
    private Queue<GameObject> _itemPool = new Queue<GameObject>();

    public void ShowItems(GameObject[] itemObjects)
    {
        ItemBunker itemBunker = ItemBunker.InstanceItemBunker;

        for (int i = 0; i < itemBunker.HavingItemCount; i++)
        {
            string itemName = itemBunker.ItemName[i];

            itemObjects[i] = GetPooledItem();

            if (itemObjects[i] == null) return;

            itemObjects[i].name = itemName;

            Image itemImage = itemObjects[i].transform.GetChild(0).GetComponent<Image>();
            if (itemBunker.ItemSpriteDictionary.TryGetValue(itemName, out Sprite itemSprite))
                itemImage.sprite = itemSprite;
        }
    }

    // プールの生成
    public GameObject GenerateitemPool(GameObject prefab, Transform poolParent)
    {
        GameObject newItem = Instantiate(prefab);
        InitializePoolItem(newItem, poolParent);
        return newItem;
    }

    // プールの呼び出し
    GameObject GetPooledItem()
    {
        if (_itemPool.Count == 0) return null;

        GameObject pooledItem = _itemPool.Dequeue();
        pooledItem.SetActive(true);
        return pooledItem;
    }

    // プール返却用
    public void ReturnAllToPool(GameObject[] itemObjects)
    {
        foreach (var obj in itemObjects)
        {
            obj.SetActive(false);
            _itemPool.Enqueue(obj);
        }
    }

    public void ShowSelectedItem(GameObject setItemFrame, Sprite itemSprite)
    {
        Image setItemImage = setItemFrame.GetComponent<Image>();
        setItemImage.sprite = itemSprite;
    }

    private void InitializePoolItem(GameObject poolItem, Transform poolParent)
    {
        poolItem.SetActive(false);
        poolItem.transform.SetParent(poolParent);
        _itemPool.Enqueue(poolItem);
    }
}
