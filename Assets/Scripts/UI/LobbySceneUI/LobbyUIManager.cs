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
        //btn.onClick.AddListener(SaveConfirm);     // ��ư ���� ��
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
        PanelEdit("�ʱ�ȭ", "���� �ʱ�ȭ �Ͻðڽ��ϱ�?");
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
        PanelEdit("����", "���� �Ͻðڽ��ϱ�?");
        ConfirmBtn.onClick.AddListener(SaveConfirm);
        cancelBtn.onClick.AddListener(PanelCancel);
    }
    public void SaveConfirm()
    {
        Debug.Log("����!");
        PanelCancel();
    }

    public void ExitContainer()
    {
        SamplePanel.SetActive(true);
        PanelEdit("������", "�����ðڽ��ϱ�?");
        ConfirmBtn.onClick.AddListener(ExitConfirm);
        cancelBtn.onClick.AddListener(PanelCancel);
    }
    public void ExitConfirm()
    {
        Debug.Log("������!");
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
