using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Synergy_UI : MonoBehaviour
{
    public Image Backgrund;
    public Button CloseButton;
    public TextMeshProUGUI[] text;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            OnSynergyInfo();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OffSynergyInfo();
        }
    }

    public void OffSynergyInfo()
    {

        Backgrund.transform.DOMoveX(-300, 1f, false);
        CloseButton.transform.DOMoveX(-100, 1f, false);
        text[0].transform.DOMoveX(-300, 1f, false);
        text[1].transform.DOMoveX(-300, 1f, false);
        text[2].transform.DOMoveX(-300, 1f, false);
        text[3].transform.DOMoveX(-300, 1f, false);

        
    }

    public void OnSynergyInfo()
    {
        Backgrund.transform.DOMoveX(280, 1f, false);
        CloseButton.transform.DOMoveX(400, 1f, false);
        text[0].transform.DOMoveX(325, 1f, false);
        text[1].transform.DOMoveX(325, 1f, false);
        text[2].transform.DOMoveX(325, 1f, false);
        text[3].transform.DOMoveX(325, 1f, false);

    }
}
