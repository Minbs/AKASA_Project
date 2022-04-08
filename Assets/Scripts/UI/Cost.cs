using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cost : Singleton<Cost>
{
    const int maxCost = 99;

    public int cost = 21;
    public int verityCost = 7, isabellaCost = 5;

    float time = 0;

    [SerializeField]
    float waitingTime = 3;
    [SerializeField]
    int regenTime = 3;

    public TextMeshProUGUI costText;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        costText.text = cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        regenCost();
    }

    /// <summary>
    /// 코스트 리젠
    /// </summary>
    void regenCost()
    {
        time += Time.deltaTime;

        //waitingTime마다 실행
        if (time >= waitingTime)
        {
            if (cost >= maxCost)
            {
                costText.text = cost.ToString() + '+'.ToString();
                return;
            }

            cost++;
            costText.text = cost.ToString();

            time = 0;
        }
    }

    /// <summary>
    /// 캐릭터 배치후 코스트 소모
    /// </summary>
    public void useCost()
    {
        if (GameManager.Instance.heroesListIndex == 0)
        {
            cost -= verityCost;
            costText.text = cost.ToString();
        }
        else if (GameManager.Instance.heroesListIndex == 1)
        {
            cost -= isabellaCost;
            costText.text = cost.ToString();
        }
        else
        {
            Debug.Log("check");
        }
    }
}
