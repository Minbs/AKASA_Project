using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public Slider progressbar;
    static string nextSceneName;

    int random;

    public GameObject illust;
    [SerializeField]
    List<GameObject> illustObj;
    [SerializeField]
    List<Image> illustImage;

    // Start is called before the first frame update
    void Start()
    {
        random = Random.Range(0, 4);

        for (int i = 0; i < illust.transform.childCount; i++)
        {
            illustObj.Add(illust.transform.GetChild(i).gameObject);
            illustImage.Add(illustObj[i].GetComponentInChildren<Image>());
        }

        switch (random)
        {
            case 0:
                illustImage[1].gameObject.SetActive(false);
                illustImage[2].gameObject.SetActive(false);
                illustImage[3].gameObject.SetActive(false);
                break;
            case 1:
                illustImage[0].gameObject.SetActive(false);
                illustImage[2].gameObject.SetActive(false);
                illustImage[3].gameObject.SetActive(false);
                break;
            case 2:
                illustImage[0].gameObject.SetActive(false);
                illustImage[1].gameObject.SetActive(false);
                illustImage[3].gameObject.SetActive(false);
                break;
            case 3:
                illustImage[0].gameObject.SetActive(false);
                illustImage[1].gameObject.SetActive(false);
                illustImage[2].gameObject.SetActive(false);
                break;
            default:
                break;
        }

        StartCoroutine(LoadSceneProcess());
    }

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
