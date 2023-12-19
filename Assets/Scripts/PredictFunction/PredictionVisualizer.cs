using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionVisualizer : MonoBehaviour
{
    [SerializeField]
    private ItemBunker _itemBunker;
    [SerializeField]
    private PredictionAdjuster _predictionAdjuster;
    [SerializeField]
    private RotateObject _rotateObject;
    private bool _currentAdjVisible;
    private bool _isAdjVisible;
    [SerializeField]
    private GameObject _predictAdjCube;
    private bool _currentSameVisible;
    private bool _isSameVisible;
    [SerializeField]
    private GameObject _predictSameCube;

    [SerializeField] 
    private StateManager _stateManager;

    void Start()
    {
        _stateManager.OnStateChanged += SwitchStateVisualize;
        _itemBunker.OnItemChanged += SwitchItemVisualize;
    }

    private void OnDestroy()
    {
        _stateManager.OnStateChanged -= SwitchStateVisualize;
        _itemBunker.OnItemChanged -= SwitchItemVisualize;
    }

    void Update()
    {
        CtrlPredictObj();
    }

    private void SwitchStateVisualize(StateManager.GameState newState)
    {
        switch (newState)
        {
            case StateManager.GameState.DefaultMode:
                _isAdjVisible = false;
                _isSameVisible = false;
                _predictAdjCube.SetActive(false);
                _predictSameCube.SetActive(false);
                break;
            case StateManager.GameState.CreateMode:
                SwitchItemVisualize(_itemBunker.SelectItem);
                break;
            case StateManager.GameState.EditMode:
                _isAdjVisible = false;
                _isSameVisible = true;
                break;
        }
    }

    void SwitchItemVisualize(string currentItem)
    {   
        switch(currentItem)
        {
            case "Cube":
                _isAdjVisible = true;
                _isSameVisible = false;
                break;
            case "HalfCube":
                _isAdjVisible = true;
                _isSameVisible = false;
                break;
            case "Step":
                _isAdjVisible = true;
                _isSameVisible = false;
                break;
            case "SmallCube":
                _isAdjVisible = true;
                _isSameVisible = false;
                break;
            case "Gene":
                _isAdjVisible = false;
                _isSameVisible = true;
                break;
        }
    }

    private void CtrlPredictObj()
    {
        if (!_predictionAdjuster.InLange || _rotateObject.IsRotate)
        {
            _isAdjVisible = false;
            _isSameVisible = false;
        }
        if (_currentAdjVisible == _isAdjVisible && _currentSameVisible == _isSameVisible) return;
        _predictAdjCube.SetActive(_isAdjVisible);
        _predictSameCube.SetActive(_isSameVisible);
        _currentAdjVisible = _isAdjVisible;
        _currentSameVisible = _isSameVisible;
    }

}
