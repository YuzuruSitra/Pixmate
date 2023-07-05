using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditMatModeOrder : MonoBehaviour
{
    [SerializeField]
    MaterialBunker _materialBunker;

    [SerializeField] 
    private StateManager _stateManager;

    [SerializeField] 
    private ImportModeOrder _importModeOrder;

    [SerializeField] 
    private GalleryShow _galleryShow;

    // ビュー用
    [SerializeField] 
    private GameObject _flamePrefab;

    [SerializeField] 
    private GameObject _setItemFlame;

    [SerializeField]
    private Transform _PoolParentObj;

    // ボタン類
    [SerializeField]
    private Button _importButton;
    [SerializeField]
    private Button _returnButton;
    [SerializeField]
    private Button _deleteButton;

    // アクティブなプールオブジェクトを保持
    private int _materialAmount => MaterialBunker.MATERIAL_AMOUNT;
    private GameObject[] _poolObj;

    void Start()
    {
        // マテリアル管理のインスタンス
        _materialBunker = MaterialBunker.InstanceMatBunker;

        // プールオブジェクトの配列を初期化
        _poolObj = new GameObject[_materialAmount];

        _stateManager.OnStateChanged += OpenEditMat;

        // ボタンのリスナーに登録
        _importButton.onClick.AddListener(GoImportMode);
        _returnButton.onClick.AddListener(ReturnEditMode);
        _deleteButton.onClick.AddListener(DeleteSprite);

        // プールの生成とリスナー登録
        SetPoolListener();
    }

    void OnDestroy()
    {
        _stateManager.OnStateChanged -= OpenEditMat;
    }

    // OpenEditMatパネル展開時の処理
    void OpenEditMat(StateManager.GameState newState)
    {
        if(newState != StateManager.GameState.EditMatMode) return;
        // 重複ケア
        _galleryShow.AllReturnPooled(_poolObj);
        // 描画処理
        _galleryShow.ShowSprits(_poolObj);
        // 選択フレームに画像をセット
        _galleryShow.ShowSelectItem(_setItemFlame, _materialBunker.NowHavePhotoSprite);
    }

    void ReturnEditMode()
    {
        _stateManager.ChangeState(StateManager.GameState.EditMode);
    }

    void GoImportMode()
    {
        _stateManager.ChangeState(StateManager.GameState.EditMatImportMode);
        // インポートしなかった場合EditMatModeに戻す
        if(!_importModeOrder.ActiveImportMode)_stateManager.ChangeState(StateManager.GameState.EditMatMode);
    }

    /*---------------------------------------------*/

    // プールオブジェクトを生成格納しリスナー登録
    void SetPoolListener()
    {
        for(int i = 0;  i < _poolObj.Length; i++)
        {
            // プールの生成格納
            _poolObj[i]  = _galleryShow.GeneratePool(_flamePrefab, _PoolParentObj);
            //ボタンをリスナー登録
            Button activeButton =  _poolObj[i].GetComponent<Button>();

            activeButton.onClick.AddListener(SelectFlame);
        }
    }

    // 特定のスプライトをピック
    void SelectFlame()
    {
        _materialBunker.NowHavePhoto = PushFlame();
        _galleryShow.ShowSelectItem(_setItemFlame, _materialBunker.NowHavePhotoSprite);
    }

    public string PushFlame()
    {
        // 選択したスプライトのキーの取得
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        string tmpKey = clickedButton.gameObject.name;

        return tmpKey;
    }

    // 特定スプライトの削除
    void DeleteSprite()
    {
        if(_materialBunker.NowHavePhoto == null) return;
        // 削除したい画像のKeyを取得
        string tmp = System.Text.RegularExpressions.Regex.Replace(_materialBunker.NowHavePhoto, @"[^0-9]", "");
        int tmpInt = int.Parse(tmp);
        string baseKeyName = MaterialBunker.KEY_NAME;

        // 画像を格納している辞書を削除したい画像から繰り上げる。
        while (tmpInt <= _materialBunker.MatCount)
        {
            string addNewKey = baseKeyName + tmpInt;
            string nextKey = baseKeyName + (tmpInt + 1);

            if(tmpInt < _materialBunker.MatCount)
            {
                // 辞書から現在のキーの要素を取得
                Sprite currentSprite = _materialBunker.CroppedImages[nextKey];
                Material currentMaterial = _materialBunker.ImageMaterials[nextKey];

                // 辞書に新しいキーで要素を追加
                _materialBunker.CroppedImages[addNewKey] = currentSprite;
                _materialBunker.ImageMaterials[addNewKey] = currentMaterial;
            }
            else
            {
                // 辞書の最後の要素を削除
                _materialBunker.CroppedImages.Remove(addNewKey);
                _materialBunker.ImageMaterials.Remove(addNewKey);
                _materialBunker.MatCount -= 1;
            }
            tmpInt++;
        }
        // 選択中の画像をnullにし再描画
        _materialBunker.NowHavePhoto = null;
        _stateManager.ChangeState(StateManager.GameState.EditMatMode);
        //オブジェクトの再描画


        // 保存する処理
        SaveManager instanceSaveManager = SaveManager.InstanceSaveManager;
        instanceSaveManager.Dosave();
    }
}
