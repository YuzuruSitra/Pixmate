using UnityEngine;
using UnityEngine.UI;

public class CreateModeUIChanger : MonoBehaviour
{
    [SerializeField] 
    private StateManager _stateManager;
    
    [SerializeField] 
    private GameObject _createPanel;
    [SerializeField]
    private GameObject[] _createPanelButton = new GameObject[2];
    [SerializeField]
    private Sprite[] _button1Sprites = new Sprite[2];
    [SerializeField]
    private Sprite[] _button2Sprites = new Sprite[1];
    private Image[] _buttonImage = new Image[2];

    void Start()
    {
        _stateManager.OnStateChanged += ChangePanel;
        for(int i = 0; i < _createPanelButton.Length; i++) 
            _buttonImage[i] = _createPanelButton[i].GetComponent<Image>();
    }

    void OnDestroy()
    {
        _stateManager.OnStateChanged -= ChangePanel;
    }

    void ChangePanel(StateManager.GameState newState)
    {
        if (newState == StateManager.GameState.CreateMode)
            OpenCreatePanel();
        else
            CloseCreatePanel();
    }

    void OpenCreatePanel()
    {
        _createPanel.SetActive(true);
        string setItem = ItemBunker.InstanceItemBunker.SelectItem;

        // アイテム毎にボタン画像の切り替え
        switch(setItem)
        {
            case "Cube":
                _createPanelButton[0].SetActive(true);
                _createPanelButton[1].SetActive(true);
                _buttonImage[0].sprite = _button1Sprites[0];
                _buttonImage[1].sprite = _button2Sprites[0];
                break;
            case "HalfCube":
                _createPanelButton[0].SetActive(true);
                _createPanelButton[1].SetActive(true);
                _buttonImage[0].sprite = _button1Sprites[0];
                _buttonImage[1].sprite = _button2Sprites[0];
                break;
            case "Step":
                _createPanelButton[0].SetActive(true);
                _createPanelButton[1].SetActive(true);
                _buttonImage[0].sprite = _button1Sprites[0];
                _buttonImage[1].sprite = _button2Sprites[0];
                break;
            case "SmallCube":
                _createPanelButton[0].SetActive(true);
                _createPanelButton[1].SetActive(true);
                _buttonImage[0].sprite = _button1Sprites[0];
                _buttonImage[1].sprite = _button2Sprites[0];
                break;
            case "Gene":
                _createPanelButton[0].SetActive(true);
                _buttonImage[0].sprite = _button1Sprites[1];
                break;
        }
    }

    void CloseCreatePanel()
    {
        _createPanel.SetActive(false);
        for(int i = 0; i < _createPanelButton.Length; i++)
            _createPanelButton[i].SetActive(false);
    }
}
