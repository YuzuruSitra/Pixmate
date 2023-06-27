using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class tmp : MonoBehaviour
{
    [SerializeField]
    private TMP_Text cardNameText;
    // Start is called before the first frame update
    void Start()
    {
        cardNameText.text = "表示させたい文字列";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
