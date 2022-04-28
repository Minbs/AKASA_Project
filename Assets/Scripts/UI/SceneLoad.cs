using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public Slider progressbar;
    static string nextSceneName;

    public GameObject illust;
    [SerializeField]
    List<GameObject> illustObj;
    [SerializeField]
    List<Image> illustImage;

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
        progressbar.value = 0;

        for (int i = 0; i < illust.transform.childCount; i++)
        {
            illustObj.Add(illust.transform.GetChild(i).gameObject);
            illustImage.Add(illustObj[i].GetComponentInChildren<Image>());
        }
    }

    /// <summary> 이전 씬에서 로딩 씬을 거쳐 다음 씬으로 이동 </summary> <param name="sceneName"></param>
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

        while (!operation.isDone)
        {
            yield return null;

            if (progressbar.value < 0.9f)
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            else if (operation.progress >= 0.9f)
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);

            if (progressbar.value >= 1f)
                operation.allowSceneActivation = true;
        }
    }
}
