using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LobbyUIManager : Singleton<LobbyUIManager>
{
    public GameObject showPanel;
    bool m_bPanelOn = false;
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
        //NotHaveFunc_Panel.GetComponent<Animator>().SetTrigger("");
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
