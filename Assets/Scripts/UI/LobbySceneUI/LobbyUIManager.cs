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
    public GameObject ClearPanel;
    public GameObject SavePanel;
    public GameObject ExitPanel;

    private void Start()
    {
        if(ClearPanel != null)
            ClearPanel.SetActive(false);
        if(SavePanel != null)
            SavePanel.SetActive(false);
        if (ExitPanel != null)
            ExitPanel.SetActive(false);

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
        //NotHaveFunc_Panel.GetComponent<Animator>().SetTrigger("");
    }

    public void ClearEditList()
    {
        ClearPanel.SetActive(true);
    }
    public void ClearConfirm()
    {
        PanelCancel();
        EditList.Instance.ListClear();
    }

    public void SaveEditList()
    {
        SavePanel.SetActive(true);
    }
    public void SaveConfirm()
    {
        Debug.Log("저장!");
        PanelCancel();
    }

    public void ExitContainer()
    {
        ExitPanel.SetActive(true);
    }
    public void ExitConfirm()
    {
        Debug.Log("나가기!");
        PanelCancel();
        LoadMainScene();
    }


    public void PanelCancel()
    {
        ClearPanel.SetActive(false);
        SavePanel.SetActive(false);
        ExitPanel.SetActive(false);
    }



    //public void Save_btn()
    //{
    //    SavePanel.SetActive(true);
    //}

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
