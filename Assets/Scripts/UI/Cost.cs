using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cost : MonoBehaviour
{
    const int maxCost = 99;

    [SerializeField]
    int cost = 21, regenTime = 3, raceCost = 5, latelyCost = 7, ereMediumCost = 9;

    float time = 0;

    [SerializeField]
    float waitingTime = 3;

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

    void regenCost()
    {
        time += Time.deltaTime;

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
}
