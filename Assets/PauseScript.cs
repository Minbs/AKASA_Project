using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class PauseScript : MonoBehaviour
{
    public GameObject Canvas;
    public Image[] Victory;
    public Image[] Fail;
    public Slider masterSlider;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.ChangeMasterVolume(masterSlider.value);
        masterSlider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterVolume(val));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            Canvas.SetActive(true);
        }
    }

    public void CanvasOff()
    {
        Time.timeScale = 1f;
        Canvas.SetActive(false);
    }

    public void Diffence_Scenes_Exit()
    {
        //����â���� ������ ��ư
        SceneManager.LoadScene("StageSelectScene_2");

    }

    public void RestartButton()
    {
  
        SceneManager.LoadScene("DefenceStageScene");
    }

    public void OkButton()
    {

        SceneManager.LoadScene("StageSelectScene_2");
    }
    public void UIFadeOutRestartButton()
    {
        for (int i = 0; i < 4; i++)
        {
            Victory[i].GetComponent<Image>().DOFade(0, 1f);
        }
        for (int i = 0; i < 4; i++)
        {
            Fail[i].GetComponent<Image>().DOFade(0, 1f);
        }
        Invoke("RestartButton", 1f);
    }
    public void UIFadeOutOKButton()
    {
        for (int i = 0; i < 4; i++)
        {
            Victory[i].GetComponent<Image>().DOFade(0, 1f);
        }
        for (int i = 0; i < 4; i++)
        {
            Fail[i].GetComponent<Image>().DOFade(0, 1f);
        }
        Invoke("OkButton",1f);
    }

}
