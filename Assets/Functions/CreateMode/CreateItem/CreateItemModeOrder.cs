using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreateItemModeOrder : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField] 
    private ShowItemList _showItemList;

    // ビュー用
    [SerializeField] 
    private GameObject _flamePrefab;

    [SerializeField] 
    private Image _setItemFlame;
    // アイテムのカウント数描画
    // [SerializeField] private Text _setItemCount;

    [SerializeField]
    private Transform _PoolParentObj;
    // アクティブなプールオブジェクトを保持
    private int _havingItemCount => ItemBunker.InstanceItemBunker.HavingItemCount;
    private GameObject[] _poolObj;
    
    // Start is called before the first frame update
    void Start()
    {

        // プールオブジェクトの配列を初期化
        _poolObj = new GameObject[_havingItemCount];

        _stateManager.OnStateChanged += OpenCreateItem;

        // プールの生成とリスナー登録
        SetPoolListener();
    }

    void OnDestroy()
    {
        _stateManager.OnStateChanged -= OpenCreateItem;
    }

    // CreateItemパネル展開時の処理
    void OpenCreateItem(StateManager.GameState newState)
    {
        if(newState != StateManager.GameState.CreateItemMode) return;
        // 重複ケア
        _showItemList.AllReturnPooled(_poolObj);
        // 描画処理
        _showItemList.ShowSprits(_poolObj);
        // 線tなくオブジェクトの描画
        ItemBunker itemBunker = ItemBunker.InstanceItemBunker;
        _setItemFlame.sprite = itemBunker.NowHaveItemSprite;
    }

    // プールオブジェクトを生成格納しリスナー登録
    
    void SetPoolListener()
    {
        for(int i = 0;  i < _poolObj.Length; i++)
        {
            // プールの生成格納
            _poolObj[i]  = _showItemList.GeneratePool(_flamePrefab, _PoolParentObj);
            //ボタンをリスナー登録
            Button activeButton =  _poolObj[i].GetComponent<Button>();

            activeButton.onClick.AddListener(SelectFlame);
        }
    }

    // 特定のスプライトをピック
    void SelectFlame()
    {
        ItemBunker itemBunker = ItemBunker.InstanceItemBunker;
        string getKey = PushFlame();
        // 選択しているアイテムによってCreateModeの行動が変わる
        itemBunker.NowHaveItem = getKey;
        // 選択中の画像表示
        _setItemFlame.sprite = itemBunker.NowHaveItemSprite;
    }

    public string PushFlame()
    {
        // 選択したスプライトのキーの取得
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        string tmpKey = clickedButton.gameObject.name;

        return tmpKey;
    }

    public void ReturnCreateMode()
    {
        //_assignMatImage.sprite = MaterialBunker.InstanceMatBunker.NowHaveSprite;
        _stateManager.ChangeState(StateManager.GameState.CreateMode);
    }

}
