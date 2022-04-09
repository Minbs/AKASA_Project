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
