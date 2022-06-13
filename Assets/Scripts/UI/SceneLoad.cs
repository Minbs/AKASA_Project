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

    [Header("LoadingObj")]
    public GameObject LoadingObj;
    [SerializeField]
    List<GameObject> LoadingImage;

    static string nextSceneName;
    int random1, random2;

    // Start is called before the first frame update
    private void Start()
    {
        Init();

        for (int i = 0; i < LoadingObj.transform.childCount; i++)
            LoadingImage[i].gameObject.SetActive(false);
        LoadingImage[random1].gameObject.SetActive(true);
        LoadingImage[random2].gameObject.SetActive(true);

        StartCoroutine(LoadSceneProcess());
    }

    private void Update()
    {
        OnLoadingSceneClick();
    }

    /// <summary> 초기화 </summary>
    private void Init()
    {
        random1 = Random.Range(0, LoadingObj.transform.childCount - 5);
        random2 = Random.Range(LoadingObj.transform.childCount - 5, LoadingObj.transform.childCount);
        for (int i = (int)ProgressbarDirection.Left; i <= (int)ProgressbarDirection.Right; i++)
            progressbar[i].value = 0;

        for (int i = 0; i < LoadingObj.transform.childCount; i++)
        {
            LoadingImage.Add(LoadingObj.transform.GetChild(i).gameObject);
        }
        Time.timeScale = 0.2f;
    }

    /// <summary> 이전 씬에서 로딩 씬을 거쳐 다음 씬으로 이동 </summary> <param name="sceneName"></param>
    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public void OnLoadingSceneClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            random1 = Random.Range(0, LoadingObj.transform.childCount - 5);
            random2 = Random.Range(LoadingObj.transform.childCount - 5, LoadingObj.transform.childCount);
            for (int i = 0; i < LoadingObj.transform.childCount; i++)
                LoadingImage[i].gameObject.SetActive(false);
            LoadingImage[random1].gameObject.SetActive(true);
            LoadingImage[random2].gameObject.SetActive(true);
        }
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
