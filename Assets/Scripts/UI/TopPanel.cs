using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopPanel : MonoBehaviour
{
    public List<GameObject> enemiesList = new List<GameObject>();

    public TextMeshProUGUI[] timeText;

    [SerializeField]
    float time = 180;

    [SerializeField]
    int maxEnemyCount = 3;

    int min, sec, currentEnemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        maxEnemyCount = 3;
        currentEnemyCount = 0;
        timeText[0].text = "03";
        timeText[1].text = "00";
        timeText[2].text = currentEnemyCount.ToString();
        timeText[3].text = maxEnemyCount.ToString();
        enemiesList = GameManager.Instance.enemiesList;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.waitTimer <= 0 && GameManager.Instance.state == State.BATTLE)
        {
            Timer();
            EnemeyCount();
            //UnitCount();
        }
        else if (GameManager.Instance.state == State.WAIT)
        {
            float time = GameManager.Instance.waitTimer;
            min = (int)time / 60;
            sec = ((int)time - min * 60) % 60;

            if (min <= 0 && sec <= 0)
            {
                timeText[0].text = 0.ToString();
                timeText[1].text = 0.ToString();
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
                    timeText[1].text = sec.ToString();
                }
            }
        }

        void Timer()
        {
            time -= Time.deltaTime;

            min = (int)time / 60;
            sec = ((int)time - min * 60) % 60;

            if (min <= 0 && sec <= 0)
            {
                timeText[0].text = 0.ToString();
                timeText[1].text = 0.ToString();
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
                    timeText[1].text = sec.ToString();
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
    }
}
