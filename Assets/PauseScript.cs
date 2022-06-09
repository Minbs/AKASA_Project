using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseScript : MonoBehaviour
{
    public GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {
        
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
        //창 나가기
        SceneManager.LoadScene("StageSelectScene");

    }

}
