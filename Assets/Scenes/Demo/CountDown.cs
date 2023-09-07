using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField]
    private Text _outText;
    [SerializeField]
    private float _setTime = 5f;
    private float _currentTime;

    // Start is called before the first frame update
    void Start()
    {
        _currentTime = _setTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentTime < 0)
        {
            _currentTime = 0;
            return;
        }
        _currentTime -= Time.deltaTime;
        _outText.text = "Count : " + _currentTime.ToString("n2") + "s";
    }

}
