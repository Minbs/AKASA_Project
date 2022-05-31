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

    /// <summary> �ʱ�ȭ </summary>
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

    /// <summary> ���� ������ �ε� ���� ���� ���� ������ �̵� </summary> <param name="sceneName"></param>
    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

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
                }
            }
        }
    }
}
