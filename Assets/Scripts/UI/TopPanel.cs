using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopPanel : MonoBehaviour
{
    public List<GameObject> enemiesList = new List<GameObject>();

    public TextMeshProUGUI[] timeText;
    public Image[] phase;
    public TextMeshProUGUI wave;

    [SerializeField]
    float time = 180, readyWaitingTime = 1.0f, battleWaitingTime = 1.0f;

    [SerializeField]
    int maxEnemyCount = 3, waveCount = 1;

    int min, sec, currentEnemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        maxEnemyCount = 3;
        currentEnemyCount = 0;
        timeText[0].text = "03";
        timeText[2].text = "00";
        timeText[3].text = currentEnemyCount.ToString();
        timeText[4].text = maxEnemyCount.ToString();
        enemiesList = GameManager.Instance.enemiesList;

        if (timeText[0].gameObject.activeSelf 
            && timeText[1].gameObject.activeSelf
            && timeText[2].gameObject.activeSelf)
        {
            timeText[0].gameObject.SetActive(false);
            timeText[1].gameObject.SetActive(false);
            timeText[2].gameObject.SetActive(false);
        }
        if (phase[0].gameObject.activeSelf && phase[1].gameObject.activeSelf)
        {
            phase[0].gameObject.SetActive(false);
            phase[1].gameObject.SetActive(false);
        }
        if (wave.gameObject.activeSelf)
        {
            wave.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.waitTimer <= 0 && GameManager.Instance.state == State.BATTLE)
        {
            battleActive();
            BattleTime();
            EnemeyCount();
            //UnitCount();
        }
        else if (GameManager.Instance.state == State.WAIT)
        {
            ReadyActive();
            WaitTime();
        }
    }

    void BattleTime()
    {
        timeText[0].gameObject.SetActive(false);
        timeText[1].gameObject.SetActive(false);
        timeText[2].gameObject.SetActive(false);

        wave.gameObject.SetActive(true);
        wave.text = "Wave ".ToString() + waveCount.ToString();
    }

    void WaitTime()
    {
        timeText[0].gameObject.SetActive(true);
        timeText[1].gameObject.SetActive(true);
        timeText[2].gameObject.SetActive(true);

        float time = GameManager.Instance.waitTimer;
        min = (int)time / 60;
        sec = ((int)time - min * 60) % 60;

        if (min <= 0 && sec <= 0)
        {
            timeText[0].text = 0.ToString();
            timeText[2].text = 0.ToString();
        }
        else
        {
            if (sec >= 60)
            {
                min += 1;
                sec -= 60;
            }
            else
            {
                timeText[0].text = min.ToString();
                timeText[2].text = sec.ToString();
            }
        }
    }
    
    void EnemeyCount()
    {
        currentEnemyCount = enemiesList.Count;
        if (currentEnemyCount >= 0)
        {
            timeText[2].text = currentEnemyCount.ToString();
        }
    }

    void UnitCount()
    {

    }

    void ReadyActive()
    {
        readyWaitingTime -= Time.deltaTime;

        if (readyWaitingTime >= 0)
        {
            phase[0].gameObject.SetActive(true);
        }
        else
        {
            phase[0].gameObject.SetActive(false);
        }
    }

    void battleActive()
    {
        battleWaitingTime -= Time.deltaTime;

        if (battleWaitingTime >= 0)
        {
            phase[1].gameObject.SetActive(true);
        }
        else
        {
            phase[1].gameObject.SetActive(false);
        }
    }
}
