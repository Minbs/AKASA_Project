using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Wave_UI_Script : MonoBehaviour
{

    public Image[] Logo;
    public TextMeshProUGUI StageText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI CostUpText;
    public TextMeshProUGUI CostUpText2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CostUpUI(int cost)
    {
        CostUpText.text = cost.ToString();
        CostUpText.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
        CostUpText.GetComponent<TextMeshProUGUI>().DOFade(0, 2f);

        CostUpText2.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
        CostUpText2.GetComponent<TextMeshProUGUI>().DOFade(0, 2f);
    }
    public void TimerText(float time)
    {
        int inttime = (int)time;
        TimeText.text = inttime.ToString();
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

        if(StageText.text == "1-0")
        {
            StageText.text = "1-1";
        }
        else if(StageText.text == "1-1")
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

