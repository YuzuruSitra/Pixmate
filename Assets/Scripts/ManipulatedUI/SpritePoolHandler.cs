using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 写真一覧のプール管理クラス
public class SpritePoolHandler : MonoBehaviour
{
    private Queue<GameObject> _imagePool = new Queue<GameObject>();

    public void ShowSprites(GameObject[] poolObjects)
    {
        MaterialBunker materialBunker = MaterialBunker.InstanceMatBunker;

        for (int i = 0; i < materialBunker.MatCount; i++)
        {
            int nameCount = i + 1;
            string tmpKey = materialBunker.KeyName + nameCount;

            poolObjects[i] = GetPooledImage();

            if (poolObjects[i] == null) return;

            poolObjects[i].name = tmpKey;

            Image pooledImage = poolObjects[i].GetComponent<Image>();
            if (materialBunker.CroppedImages.TryGetValue(tmpKey, out Sprite tmpSprite))
                pooledImage.sprite = tmpSprite;
        }
    }

    public GameObject GeneratePool(GameObject prefab, Transform poolParent)
    {
        GameObject newImage = Instantiate(prefab);
        InitializePoolObject(newImage, poolParent);
        return newImage;
    }

    GameObject GetPooledImage()
    {
        if (_imagePool.Count == 0) return null;

        GameObject pooledImage = _imagePool.Dequeue();
        pooledImage.SetActive(true);
        return pooledImage;
    }

    public void ReturnAllToPool(GameObject[] poolObjects)
    {
        foreach (var obj in poolObjects)
        {
            obj.SetActive(false);
            _imagePool.Enqueue(obj);
        }
    }

    public void ShowSelectedItem(GameObject itemFrame, InputField itemNameField, Sprite itemSprite, string itemName)
    {
        Image itemImage = itemFrame.GetComponent<Image>();
        itemImage.sprite = itemSprite;
        itemNameField.text = itemName;
    }

    private void InitializePoolObject(GameObject poolObject, Transform poolParent)
    {
        poolObject.SetActive(false);
        poolObject.transform.SetParent(poolParent);
        _imagePool.Enqueue(poolObject);
    }
}
