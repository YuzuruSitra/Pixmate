using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItemList : MonoBehaviour
{
    private Queue<GameObject> itemPool = new Queue<GameObject>();

    public void ShowSprits(GameObject[] poolObj)
    {
        ItemBunker itemBunker = ItemBunker.InstanceItemBunker;
        for(int i = 0;  i < itemBunker.HavingItemCount; i++)
        {
            // キーの設定
            string tmpKey = itemBunker.ItemName[i];

            // プールから出力されたオブジェクトの取得
            poolObj[i] = GetPooledImage();

            if(poolObj[i] == null)return;

            //オブジェクトの名前を変更
            poolObj[i].name = tmpKey;

            // 表示画像の制御
            Image outPooldImage = poolObj[i].transform.GetChild(0).GetComponent<Image>();
            if (itemBunker.ItemSpriteDictionary.TryGetValue(tmpKey, out Sprite tmpSprite))outPooldImage.sprite = tmpSprite;
        }
    }

    /*---------------------------------------------*/

    // プールの生成
    public GameObject GeneratePool(GameObject flamePrefab ,Transform poolParentObj)
    {
        GameObject newImage = Instantiate(flamePrefab);

        // オブジェクト座標周りの初期設定
        newImage.SetActive(false);
        newImage.transform.SetParent(poolParentObj);
        itemPool.Enqueue(newImage);
        return newImage;
    }

    // プールの呼び出し
    GameObject GetPooledImage()
    {
        if (itemPool.Count <= 0)return null;
        GameObject pooledImage = itemPool.Dequeue();
        pooledImage.SetActive(true);
        return pooledImage;
    }

    // プール返却用
    public void AllReturnPooled(GameObject[] poolObj)
    {
        for(int i = 0;  i < poolObj.Length; i++)
        {
            bool activeObj = poolObj[i].activeInHierarchy;
            if(activeObj)poolObj[i].SetActive(false);
            itemPool.Enqueue(poolObj[i]);
        }
    }

    public void ShowSelectItem(GameObject setItemFlame, Sprite tmpItem)
    {
        Image setItemImage = setItemFlame.GetComponent<Image>();
        setItemImage.sprite = tmpItem;
    }
}
