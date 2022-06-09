using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Wave_UI_Script : MonoBehaviour
{

    public Image[] Logo;
    public TextMeshProUGUI StageText;

    // Start is called before the first frame update
    void Start()
    {
        Wave_Logo_ColorChange(0, "Yellow");
        Wave_Logo_ColorChange(1, "Red");
        Wave_Logo_ColorChange(2, "Blue");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("키입력");
            StageText_Next();
        }
    }

    public void Wave_Logo_ColorChange(int logoindex, string Color) //logoindex = 로고인텍스, Color = 변할색깔 {Yellow, Blue, Red}
    {   
        if(Color == "Yellow")
        {
            Logo[logoindex].color = UnityEngine.Color.yellow;

        }

        else if(Color == "Blue")
        {
            Logo[logoindex].color = UnityEngine.Color.blue ;

        }

        else if (Color == "Red")
        {
            Logo[logoindex].color = UnityEngine.Color.red;

        }
    }

    public void StageText_Next()
    {
      
        if(StageText.text == "1-1")
        {
            StageText.text = "1-2";
        }
        else if(StageText.text == "1-2")
        {
            StageText.text = "1-3";
        }

        else if (StageText.text == "1-3")
        {
            StageText.text = "1-4";
        }

        else if (StageText.text == "1-4")
        {
            StageText.text = "2-1";
        }

        else if (StageText.text == "2-1")
        {
            StageText.text = "2-2";
        }

        else if (StageText.text == "2-2")
        {
            StageText.text = "2-3";
        }

        else if (StageText.text == "2-3")
        {
            StageText.text = "2-4";
        }
    }

}

