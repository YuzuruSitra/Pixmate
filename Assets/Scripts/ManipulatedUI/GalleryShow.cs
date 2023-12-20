using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryShow : MonoBehaviour
{
    private Queue<GameObject> imagePool = new Queue<GameObject>();

    public void ShowSprits(GameObject[] poolObj )
    {
        MaterialBunker materialBunker = MaterialBunker.InstanceMatBunker;
        for(int i = 0;  i < materialBunker.MatCount; i++)
        {
            // キーの設定
            int nameCount = i + 1;
            string tmpKey = materialBunker.KeyName;
            tmpKey += nameCount;

            // プールから出力されたオブジェクトの取得
            poolObj[i] = GetPooledImage();

            if(poolObj[i] == null)return;

            //オブジェクトの名前を変更
            poolObj[i].name = tmpKey;

            // 表示画像の制御
            Image outPooldImage = poolObj[i].GetComponent<Image>();
            if (materialBunker.CroppedImages.TryGetValue(tmpKey, out Sprite tmpSprite))outPooldImage.sprite = tmpSprite;
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
        imagePool.Enqueue(newImage);
        return newImage;
    }

    // プールの呼び出し
    GameObject GetPooledImage()
    {
        if (imagePool.Count <= 0)return null;
        GameObject pooledImage = imagePool.Dequeue();
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
            imagePool.Enqueue(poolObj[i]);
        }
    }
    
    // アイテムの選択処理
    public void ShowSelectItem(GameObject setItemFlame,InputField setItemName, Sprite tmpItem, string tmpName)
    {
        Image setItemImage = setItemFlame.GetComponent<Image>();
        setItemImage.sprite = tmpItem;
        setItemName.text = tmpName;
    }

}
