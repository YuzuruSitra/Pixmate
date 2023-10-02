using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PixmatesManager : MonoBehaviour
{
    public static PixmatesManager InstancePixmatesManager;
    SaveManager _saveManager;

    public const string PIXMATE_KEY = "PixmateNo.";
    public string PixmateKey => PIXMATE_KEY;

    // 存在できるPixmateの最大値
    private const int MAX_PIXMATES = 10;
    // 現在いるPixmateの数
    private int _pixmatesCount = 0;
    public int PixmatesCount => _pixmatesCount;
    private GameObject[] _pixmateFoxes = new GameObject[0];

    // 生成Prefab
    [SerializeField]
    private GameObject _prefabFox;

    // セーブと紐づけ
    public Dictionary<string, Sprite> _textureImages = new Dictionary<string, Sprite>();

    void Awake()
    {
        if (InstancePixmatesManager == null)
        {
            InstancePixmatesManager = this;
        }
    }

    void Start()
    {
        _saveManager = SaveManager.InstanceSaveManager;
        
        // ロード処理
        // _pixmatesCount = _saveManager.LoadPixmateCount();
        for(int i = 0; i < _pixmatesCount; i++)
        {
            // CroppedImagesに追加。
            int nameCount = i + 1;
            string addTmpKey = PIXMATE_KEY + nameCount;
            Sprite sprite = _saveManager.LoadPixmateSprite(addTmpKey);
            if(sprite != null) _textureImages.Add(addTmpKey, sprite);
        }

        // Pixmateの生成
        for(int i = 0; i < _pixmatesCount; i++)
        {
            string textureKey = PIXMATE_KEY + (i + 1);
            Texture2D matTexture = Texture2D.whiteTexture;
            // テクスチャの取得
            if (_textureImages.TryGetValue(textureKey, out Sprite targetSprite))
            {
                matTexture = new Texture2D(targetSprite.texture.width, targetSprite.texture.height, TextureFormat.RGBA32, false);
                matTexture.SetPixels(targetSprite.texture.GetPixels());
                matTexture.Apply();
            }
            else
            {
                continue;
            }

            // ※ロードした値に変更予定
            Vector3 randomPosition = new Vector3( UnityEngine.Random.Range(-3f, 4f), 1f, UnityEngine.Random.Range(-3f, 4f));
            Vector3 scale = new Vector3(0.15f, 0.15f, 0.15f);
            Quaternion rot = Quaternion.Euler(0f, UnityEngine.Random.Range(-180f, 181f), 0f); 
            Array.Resize(ref _pixmateFoxes, _pixmateFoxes.Length + 1);
            _pixmateFoxes[_pixmateFoxes.Length - 1] = InstantiatePixmate(randomPosition, rot, scale, matTexture);
            ActivationPixmate(_pixmateFoxes[i].GetComponent<FoxEcology>());
        }
    }

    // Pixmateの動き出し
    public void ActivationPixmate(FoxEcology pixmateEcology)
    {
        pixmateEcology.ComeAlive();
    }

    // Pixmateの追加処理
    public void SpawnPixmate(Texture2D texture)
    {
        _pixmatesCount ++;
        
        //Textureをスプライトに変換し辞書へ追加
        PixmateOutSprite pixmateOutSprite = new PixmateOutSprite();
        Sprite pixmateSprite = pixmateOutSprite.AddPixmateSprite(_pixmatesCount, texture);
        if(pixmateSprite == null) return;
        string key = PIXMATE_KEY + _pixmatesCount;
        _textureImages.Add(key, pixmateSprite);

        // セーブ処理
        _saveManager.DoSavePixmates(_pixmatesCount, pixmateSprite, key);
    }

    public GameObject InstantiatePixmate(Vector3 insPos, Quaternion insRot,Vector3 scale, Texture2D texture)
    {
        // Pixmateの生成処理
        GameObject insPixmate = Instantiate(_prefabFox, insPos, insRot);
        insPixmate.transform.localScale = scale;
        // テクスチャの割り当て
        GameObject childObj = insPixmate.transform.GetChild(0).gameObject;
        childObj.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_BaseMap", texture);
        return insPixmate;
    }

}
