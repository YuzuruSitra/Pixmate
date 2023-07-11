using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    [SerializeField] 
    private GameObject _movePads;
    [SerializeField] 
    private GameObject _defaultPanel;
    [SerializeField] 
    private GameObject _createPanel;
    [SerializeField]
    private GameObject[] _createPanelButton = new GameObject[2];
    [SerializeField]
    private Sprite[] _button1Sprites = new Sprite[2];
    [SerializeField]
    private Sprite[] _button2Sprites = new Sprite[1];
    [SerializeField]
    private GameObject _createItemPanel;
    [SerializeField] 
    private GameObject _editPanel;
    [SerializeField] 
    private GameObject _editMatPanel;
    [SerializeField] 
    private GameObject _editCropPanel;

    private void Start()
    {
        _stateManager.OnStateChanged += UpdateUI;
        _stateManager.ChangeState(StateManager.GameState.DefaultMode);
    }

    private void OnDestroy()
    {
        _stateManager.OnStateChanged -= UpdateUI;
    }

    private void UpdateUI(StateManager.GameState newState)
    {
        switch (newState)
        {
            case StateManager.GameState.DefaultMode:
                OnDefaultUI();
                break;
            case StateManager.GameState.CreateMode:
                OnCreateUI();
                break;
            case StateManager.GameState.CreateItemMode:
                OnCreateItemUI();
                break;
            case StateManager.GameState.EditMode:
                OnEditUI();
                break;
            case StateManager.GameState.EditMatMode:
                OnEditMatUI();
                break;
            case StateManager.GameState.EditMatImportMode:
                OnEditCropUI();
                break;
            case StateManager.GameState.SettingsMode:
                OnSettingsUI();
                break;
        }
    }

    /*---------------------------------------------*/

    void OnDefaultUI()
    {
        AllInactiveUI();
        _movePads.SetActive(true);
        _defaultPanel.SetActive(true);
    }

    void OnCreateUI()
    {
        AllInactiveUI();
        _movePads.SetActive(true);
        _createPanel.SetActive(true);
        string setItem = ItemBunker.InstanceItemBunker.NowHaveItem;
        Image[] buttonImage = new Image[2];
        for(int i = 0; i < _createPanelButton.Length; i++)
        {
            buttonImage[i] = _createPanelButton[i].GetComponent<Image>();
        }

        // アイテムによてボタンの切り替え
        switch(setItem)
        {
            case "Cube":
                _createPanelButton[0].SetActive(true);
                _createPanelButton[1].SetActive(true);
                buttonImage[0].sprite = _button1Sprites[0];
                buttonImage[1].sprite = _button2Sprites[0];
                break;
            case "HalfCube":
                _createPanelButton[0].SetActive(true);
                _createPanelButton[1].SetActive(true);
                buttonImage[0].sprite = _button1Sprites[0];
                buttonImage[1].sprite = _button2Sprites[0];
                break;
            case "Step":
                _createPanelButton[0].SetActive(true);
                _createPanelButton[1].SetActive(true);
                buttonImage[0].sprite = _button1Sprites[0];
                buttonImage[1].sprite = _button2Sprites[0];
                break;
            case "SmallCube":
                _createPanelButton[0].SetActive(true);
                _createPanelButton[1].SetActive(true);
                buttonImage[0].sprite = _button1Sprites[0];
                buttonImage[1].sprite = _button2Sprites[0];
                break;
            case "Gene":
                _createPanelButton[0].SetActive(true);
                buttonImage[0].sprite = _button1Sprites[1];
                break;
        }
    }

    void OnCreateItemUI()
    {
        AllInactiveUI();
        _createItemPanel.SetActive(true);
    }

    void OnEditUI()
    {
        AllInactiveUI();
        _movePads.SetActive(true);
        _editPanel.SetActive(true);
    }

    void OnEditMatUI()
    {
        AllInactiveUI();
        _editMatPanel.SetActive(true);
    }

    void OnEditCropUI()
    {
        AllInactiveUI();
        _editCropPanel.SetActive(true);
    }

    void OnSettingsUI()
    {
        AllInactiveUI();
    }

    // 全てのUIを非アクティブ化
    void AllInactiveUI()
    {
        _movePads.SetActive(false);
        _defaultPanel.SetActive(false);
        _createPanel.SetActive(false);
        _createItemPanel.SetActive(false);
        _editPanel.SetActive(false);
        _editMatPanel.SetActive(false);
        _editCropPanel.SetActive(false);
        for(int i=0; i < _createPanelButton.Length; i++)
        {
            _createPanelButton[i].SetActive(false);
        }
    }


}
