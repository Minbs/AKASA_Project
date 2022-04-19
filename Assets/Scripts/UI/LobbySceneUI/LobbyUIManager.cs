using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using UnityEngine.EventSystems;
using TMPro;

public class LobbyUIManager : Singleton<LobbyUIManager>
{
    [Space(10f)]
    [Header("title")]
    public TextMeshProUGUI BlinkTextUI;
    public Image titleLogo;
    public Image TitleImage;
    public List<Sprite> TitleSprite;
    public float BG_Change_Time;

    [Space(10f)]
    [Header("��� �����ִ� Panel")]
    public GameObject showPanel;
    bool m_bPanelOn = false;

    //public GameObject ClearPanel;
    //public GameObject SavePanel;
    //public GameObject ExitPanel;
    [Space(10f)]
    [Header("Popup Panel")]
    public GameObject SamplePanel;
    public TextMeshProUGUI Title_text;
    public TextMeshProUGUI Content_text;
    public Button ConfirmBtn;
    public Button cancelBtn;

    [Space(10f)]
    [Header("Stage_Info")]
    public GameObject StageInfo_Screen;
    public TextMeshProUGUI StageName_Text;
    public GameObject EditPanel;
    public TextMeshProUGUI BestLvText;


    //public GameObject MinionStand;

    delegate void FunctionPointer();

    private void Start()
    {
        //DontDestroyOnLoad(gameObject);
        if (BlinkTextUI != null && TitleImage != null)
        {
            BG_Change_Time = 10f;
            StartCoroutine(BlinkText());
            StartCoroutine(ChangeBG());
        }

        if (SamplePanel != null)
            SamplePanel.SetActive(false);
        if (StageInfo_Screen != null)
            StageInfo_Screen.SetActive(false);
        if (EditPanel != null)
            EditPanel.SetActive(false); 

        //btn.onClick.AddListener(SaveConfirm);     // ��ư ���� ��
    }

    #region Titlefunc

    IEnumerator ChangeBG()
    {
        int BgCount = 0;
        float bgAlpha = TitleImage.color.a;
        while (true)
        {
            BgCount = Random.Range(0, TitleSprite.Count);
            for (; bgAlpha > 0.0f; bgAlpha -= .01f)
            {
                BGAlphaChange(bgAlpha);
                
                yield return new WaitForSeconds(0.01f);
            }
            TitleImage.sprite = TitleSprite[BgCount];

            for (; bgAlpha < 1.0f; bgAlpha += .01f)
            {
                BGAlphaChange(bgAlpha);
                yield return new WaitForSeconds(.01f);
            }
            yield return new WaitForSeconds(BG_Change_Time);
        }
    }


    IEnumerator BlinkText()
    {
        float TextAlpha = BlinkTextUI.color.a;

        while (BlinkTextUI == true)
        {
            for (; TextAlpha > 0.0f; TextAlpha -= .1f)
            {
                ColorAlphaChange(TextAlpha);
                yield return new WaitForSeconds(0.1f);
            }

            for (; TextAlpha < 1.0f; TextAlpha += .1f)
            {
                ColorAlphaChange(TextAlpha);
                yield return new WaitForSeconds(.1f);
            }
        }
    }

    void ColorAlphaChange(float TextAlpha)
    {
        Color c_Dummy = BlinkTextUI.color;
        c_Dummy.a = (float)TextAlpha;
        BlinkTextUI.color = c_Dummy;
    }

    void BGAlphaChange(float Alpha )
    {
        Color c_Dummy = TitleImage.color;
        c_Dummy.a = (float)Alpha;
        TitleImage.color = c_Dummy;
        titleLogo.color = c_Dummy;
    }


    #endregion


    public void LoadScene(string SceneName)
    {
        switch (SceneName)
        {
            case "MainScene":
                DontDestroyable.Instance.AudioPlay(1);
                break;
            case "StageSelectScene":
                DontDestroyable.Instance.AudioPlay(2);
                break;
            default:
                break;
        }
        SceneManager.LoadScene(SceneName);
        Debug.Log(SceneName + "������ �̵�");
    }

    private void Update()
    {

    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
        Debug.Log("���� ������ �̵�");
    }
    public void LoadGachaScene()
    {
        Debug.Log("�̱� �� ���");
    }

    public void LoadUnitContainerScene()
    {
        SceneManager.LoadScene("UnitContainerScene");
        Debug.Log("���� ���� �� ���");

    }

    public void ShowSettingScene()
    {
        Debug.Log("���� �� �����ֱ�");

    }

    public void LoadStageSelectScene()
    {
        SceneManager.LoadScene("StageSelectScene");
        Debug.Log("�������� ���� �� �����ֱ�");

    }

    public void PreparingButton()
    {
        if (m_bPanelOn == false)
            StartCoroutine(PanelPlay());
    }

    public void ClearEditList()
    {
        SamplePanel.SetActive(true);
        PanelEdit("�ʱ�ȭ", "���� �ʱ�ȭ�Ͻðڽ��ϱ�?");
        ConfirmBtn.onClick.AddListener(ClearConfirm);
        cancelBtn.onClick.AddListener(PanelCancel);
    }


    void PanelEdit(string m_title, string m_contents)
    {
        Title_text.text = m_title;
        Content_text.text = m_contents;
    }
    public void ClearConfirm()
    {
        PanelCancel();
        EditList.Instance.ListClear();
    }

    public void SaveEditList()
    {
        SamplePanel.SetActive(true);
        PanelEdit("����", "������ �����Ͻðڽ��ϱ�?");


        ConfirmBtn.onClick.AddListener(SaveConfirm);
        cancelBtn.onClick.AddListener(PanelCancel);
    }
    public void SaveConfirm()
    {
        Debug.Log("����!");
        EditList.Instance.SaveJsonFile();
        PanelCancel();
    }

    public void ExitContainer()
    {
        SamplePanel.SetActive(true);
        PanelEdit("������", "������ �����ðڽ��ϱ�?");
        ConfirmBtn.onClick.AddListener(ExitConfirm);
        cancelBtn.onClick.AddListener(PanelCancel);
    }
    public void ExitConfirm()
    {
        Debug.Log("������!");
        PanelCancel();
        LoadMainScene();
    }
    public void ShowPanel(GameObject obj)
    {
        obj.SetActive(true);
    }
    public void HidePanel(GameObject obj)
    {
        obj.SetActive(false);
    }
    public void PanelCancel()
    {
        ConfirmBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();
        SamplePanel.SetActive(false);
    }

    void CreateSaveDirectory()
    {
        //string filePath = Application.dataPath + "/Resources";
        //if (!Directory.Exists(filePath))
        //    AssetDatabase.CreateFolder("Assets", "Resources");
        //filePath += "/Levels";
        //if (!Directory.Exists(filePath))
        //    AssetDatabase.CreateFolder("Assets/Resource", "Levels");
        //AssetDatabase.Refresh();
    }



    public IEnumerator PanelPlay()
    {
        m_bPanelOn = true;
        showPanel.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        showPanel.GetComponent<Animator>().SetTrigger("Hide");
        yield return new WaitForSeconds(1.0f);
        showPanel.SetActive(false);
        m_bPanelOn = false;
    }

    public void ShowStageInfo(GameObject obj)
    {
        StageInfo_Screen.SetActive(true);
        StageManager.Instance.SetTargetStage(obj);
        StageName_Text.text = obj.GetComponent<StageInfo>().pro_stagename;
        BestLvText.text = "Best Lv: " + obj.GetComponent<StageInfo>().pro_BestLv.ToString();
    }
    public void HideStageInfo()
    {
        StageManager.Instance.PopTargetStage();
        StageInfo_Screen.SetActive(false);
    }

}
