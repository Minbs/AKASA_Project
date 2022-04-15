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
    [Header("잠깐 보여주는 Panel")]
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



    //public GameObject MinionStand;

    delegate void FunctionPointer();

    private void Start()
    {
        if (SamplePanel != null)
            SamplePanel.SetActive(false);
        if (StageInfo_Screen != null)
            StageInfo_Screen.SetActive(false);  

        //btn.onClick.AddListener(SaveConfirm);     // 버튼 적용 법
    }

    private void Update()
    {

    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
        Debug.Log("메인 씬으로 이동");
    }
    public void LoadGachaScene()
    {
        Debug.Log("뽑기 씬 출력");
    }

    public void LoadUnitContainerScene()
    {
        SceneManager.LoadScene("UnitContainerScene");
        Debug.Log("유닛 관리 씬 출력");

    }

    public void ShowSettingScene()
    {
        Debug.Log("설정 씬 보여주기");

    }

    public void LoadStageSelectScene()
    {
        SceneManager.LoadScene("StageSelectScene");
        Debug.Log("스테이지 선택 씬 보여주기");

    }

    public void PreparingButton()
    {
        if (m_bPanelOn == false)
            StartCoroutine(PanelPlay());
    }

    public void ClearEditList()
    {
        SamplePanel.SetActive(true);
        PanelEdit("Reset", "Are you sure you want to reset it?");
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
        PanelEdit("Save", "Are you sure you want to save?");
        ConfirmBtn.onClick.AddListener(SaveConfirm);
        cancelBtn.onClick.AddListener(PanelCancel);
    }
    public void SaveConfirm()
    {
        Debug.Log("저장!");
        PanelCancel();
    }

    public void ExitContainer()
    {
        SamplePanel.SetActive(true);
        PanelEdit("Exit", "Are you sure you want to get out of UnitContainer?");
        ConfirmBtn.onClick.AddListener(ExitConfirm);
        cancelBtn.onClick.AddListener(PanelCancel);
    }
    public void ExitConfirm()
    {
        Debug.Log("나가기!");
        PanelCancel();
        LoadMainScene();
    }


    public void PanelCancel()
    {
        SamplePanel.SetActive(false);
    }

    void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");
        filePath += "/Levels";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resource", "Levels");
        AssetDatabase.Refresh();
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



    public void ShowStageInfo(string stageName)
    {
        StageInfo_Screen.SetActive(true);
        StageName_Text.text = stageName;
    }
    public void HideStageInfo()
    {
        StageInfo_Screen.SetActive(false);
    }

}
