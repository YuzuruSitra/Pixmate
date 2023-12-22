using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// アイテム選択モードのリスナー登録
public class SelectItemModeAddListener : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField] 
    private ItemPoolHandler _itemPoolHandler;

    // ビュー用
    [SerializeField] 
    private GameObject _flamePrefab;

    // CreateModeUIs
    [SerializeField] 
    private Image _setItemFlame;
    // アイテムのカウント数描画
    // [SerializeField] private Text _setItemCount;

    [SerializeField]
    private Transform _PoolParentObj;
    // アクティブなプールオブジェクトを保持
    private int _havingItemCount => ItemBunker.InstanceItemBunker.HavingItemCount;
    private GameObject[] _poolObj;
    
    // ItemListでの説明用UI
    [SerializeField]
    private Text _itemName;
    //[SerializeField]
    //private Text ItemCount;
    [SerializeField]
    private Text _itemExp;

    // 名前と説明のScriptableObject。
    [SerializeField]
    private ItemInformation _itemInformation;
    [SerializeField]
    private Button _returnButton;


    // Start is called before the first frame update
    void Start()
    {
        // プールオブジェクトの配列を初期化
        _poolObj = new GameObject[_havingItemCount];

        _stateManager.OnStateChanged += OpenCreateItem;

        _returnButton.onClick.AddListener(ReturnCreateMode);

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
        if(newState != StateManager.GameState.SelectItemMode) return;
        // 重複ケア
        _itemPoolHandler.ReturnAllToPool(_poolObj);
        // 描画処理
        _itemPoolHandler.ShowItems(_poolObj);
        // オブジェクトの描画
        ItemBunker itemBunker = ItemBunker.InstanceItemBunker;
        _setItemFlame.sprite = itemBunker.NowHaveItemSprite;
        UpdateItemUIs();
    }

    // プールオブジェクトを生成格納しリスナー登録
    
    void SetPoolListener()
    {
        for(int i = 0;  i < _poolObj.Length; i++)
        {
            // プールの生成格納
            _poolObj[i]  = _itemPoolHandler.GenerateitemPool(_flamePrefab, _PoolParentObj);
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
        itemBunker.ChangeItem(getKey);
        // 選択中の画像表示
        _setItemFlame.sprite = itemBunker.NowHaveItemSprite;
        UpdateItemUIs();
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

    // アイテムの名前と説明の更新
    private void UpdateItemUIs()
    {
        ItemBunker itemBunker = ItemBunker.InstanceItemBunker;
        string nowHaveItem = itemBunker.SelectItem;
        string itemName = "";
        string itemExp = "";

        // 名前と説明の変更処理
        switch (nowHaveItem)
        {
            case "Cube":
                itemName = _itemInformation.CubeName;
                itemExp = _itemInformation.CubeExp;
                break;
            case "HalfCube":
                itemName = _itemInformation.HalfCubeName;
                itemExp = _itemInformation.HalfCubeExp;
                break;
            case "Step":
                itemName = _itemInformation.StepCubeName;
                itemExp = _itemInformation.StepCubeExp;
                break;
            case "SmallCube":
                itemName = _itemInformation.SmallCubeName;
                itemExp = _itemInformation.SmallCubeExp;
                break;
            case "Gene":
                itemName = _itemInformation.PixGenName;
                itemExp = _itemInformation.PixGenExp;
                break;
        }

        //改行コード変換
        if (itemName.Contains("\\n")) itemName = itemName.Replace(@"\n", Environment.NewLine);
        //改行コード変換
        if (itemExp.Contains("\\n")) itemExp = itemExp.Replace(@"\n", Environment.NewLine);

        _itemName.text = itemName;
        _itemExp.text = itemExp;
    }


}
