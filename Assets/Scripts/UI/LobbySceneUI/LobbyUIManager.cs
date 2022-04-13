using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using UnityEngine.EventSystems;

public class LobbyUIManager : Singleton<LobbyUIManager>
{
    public GameObject showPanel;
    bool m_bPanelOn = false;
    //public GameObject ClearPanel;
    //public GameObject SavePanel;
    //public GameObject ExitPanel;
    public GameObject SamplePanel;
    public Text Title_text;
    public Text Content_text;
    public Button ConfirmBtn;
    public Button cancelBtn;
    delegate void FunctionPointer();

    private void Start()
    {
        //if(ClearPanel != null)
        //    ClearPanel.SetActive(false);
        //if(SavePanel != null)
        //    SavePanel.SetActive(false);
        //if (ExitPanel != null)
        //    ExitPanel.SetActive(false);
        if (SamplePanel != null)
            SamplePanel.SetActive(false);
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
        PanelEdit("초기화", "정말 초기화 하시겠습니까?");
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
        PanelEdit("저장", "저장 하시겠습니까?");
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
        PanelEdit("나가기", "나가시겠습니까?");
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

}
