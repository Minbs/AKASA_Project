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
    public Image WaveFill; 
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            StageText_Next();
        }
       
    }

    public void CostUpUI(int cost, string textColor)
    {
        if(textColor == "Blue")
        {
            CostUpText.text = cost.ToString();

            CostUpText.color = new Color32(92, 151, 255, 255);
            CostUpText.GetComponent<TextMeshProUGUI>().DOFade(0, 2f);
            CostUpText2.color = new Color32(92, 151, 255, 255);
            CostUpText2.GetComponent<TextMeshProUGUI>().DOFade(0, 2f);
        }

        else
        {
            CostUpText.text = cost.ToString();
            CostUpText.color = new Color32(255, 255, 255, 255);
            CostUpText.GetComponent<TextMeshProUGUI>().DOFade(0, 2f);

            CostUpText2.color = new Color32(255, 255, 255, 255);
            CostUpText2.GetComponent<TextMeshProUGUI>().DOFade(0, 2f);
        }

    }

    public void TimerText(float time,float maxtime)
    {
        int inttime = (int)time;
        TimeText.text = inttime.ToString();
        WaveFill.GetComponent<Image>().fillAmount = (30-time) / maxtime;
    }

    public void Wave_Logo_ColorChange(int logoindex, string Color) //logoindex = 로고인텍스, Color = 변할색깔 {Yellow, Blue, Red}
    {   
        if(Color == "Yellow")
        {
            Logo[logoindex].color = new Color32(255,224,106,255);

        }

        else if(Color == "Blue")
        {
            Logo[logoindex].color = new Color32(92,151,255,255);

        }

        else if (Color == "Red")
        {
            Logo[logoindex].color = new Color32(255,117,92,255);

        }
    }

    public void StageText_Next()
    {

        if (StageText.text == "1-0")
        {
            StageText.text = "1-1";
        }
        else if (StageText.text == "1-1")
        {
            StageText.text = "1-2";
        }
        else if (StageText.text == "1-2")
        {
            StageText.text = "1-3";
        }

        else if (StageText.text == "1-3")
        {
            StageText.text = "1-4";
        }

        else if (StageText.text == "1-4")
        {
            StageText.text = "1-5";
        }

        else if (StageText.text == "1-5")
        {
            StageText.text = "2-1";
            for(int i = 0; i < 5; i++)
            {
                Logo[i].gameObject.SetActive(false);
                Logo[i + 5].gameObject.SetActive(true);
            }
            
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
        else if (StageText.text == "2-4")
        {
            StageText.text = "2-5";
        }

    }

}

