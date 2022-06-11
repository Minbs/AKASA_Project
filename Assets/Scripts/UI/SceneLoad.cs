using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum ProgressbarDirection
{
    Left,
    Right
}

public class SceneLoad : MonoBehaviour
{
    [Header("Progressbar")]
    public Slider[] progressbar;
    public Text[] loadingText;

    [Header("Illust")]
    public GameObject illust;
    [SerializeField]
    List<GameObject> illustObj;
    [SerializeField]
    List<Image> illustImage;

    static string nextSceneName;
    int random;

    // Start is called before the first frame update
    private void Start()
    {
        Init();

        for (int i = 0; i < illust.transform.childCount; i++)
            illustImage[i].gameObject.SetActive(false);
        illustImage[random].gameObject.SetActive(true);

        StartCoroutine(LoadSceneProcess());
    }

    /// <summary> 초기화 </summary>
    private void Init()
    {
        random = Random.Range(0, illust.transform.childCount);
        for (int i = (int)ProgressbarDirection.Left; i < (int)ProgressbarDirection.Right + 1; i++)
            progressbar[i].value = 0;

        for (int i = 0; i < illust.transform.childCount; i++)
        {
            illustObj.Add(illust.transform.GetChild(i).gameObject);
            illustImage.Add(illustObj[i].GetComponentInChildren<Image>());
        }
        Time.timeScale = 0.2f;
    }

    /// <summary> 이전 씬에서 로딩 씬을 거쳐 다음 씬으로 이동 </summary> <param name="sceneName"></param>
    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    /// <summary> 비동기 씬 전환(+ 로딩 퍼센티지) </summary> <returns></returns>
    IEnumerator LoadSceneProcess()
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
        operation.allowSceneActivation = false;

        int percentage;
        while (!operation.isDone)
        {
            yield return null;

            for (int i = (int)ProgressbarDirection.Left; i < (int)ProgressbarDirection.Right + 1; i++)
            {
                percentage = (int)(progressbar[i].value * 100);
                loadingText[i].text = percentage.ToString() + "%".ToString();

                if (progressbar[i].value < 0.9f)
                    progressbar[i].value = Mathf.MoveTowards(progressbar[i].value, 0.9f, Time.deltaTime);
                else if (operation.progress >= 0.9f)
                    progressbar[i].value = Mathf.MoveTowards(progressbar[i].value, 1f, Time.deltaTime);

                if (progressbar[i].value >= 1f)
                {
                    loadingText[i].text = 100.ToString() + "%".ToString();
                    operation.allowSceneActivation = true;
                    Time.timeScale = 1f;
                }
            }
        }
    }
}
