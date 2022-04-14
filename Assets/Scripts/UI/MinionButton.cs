using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;
using System.Linq;
using TMPro;

public class MinionButton : Singleton<MinionButton>, IPointerDownHandler
{
    // Start is called before the first frame update
    public int index;
    public Minion hero;
    public GameObject minionBtnTranslucentBG;
    List<GameObject> tBG = new List<GameObject>();
    List<TextMeshProUGUI> wTime = new List<TextMeshProUGUI>();
    public bool isCheck = false;
    public float time;

    void Start()
    {
        for (int i = 0; i < 12; i++)
        {
            tBG.Add(minionBtnTranslucentBG.transform.GetChild(i).gameObject);
            wTime.Add(tBG[i].GetComponentInChildren<TextMeshProUGUI>());

            if (tBG[i].activeSelf)
                tBG[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(MinionManager.Instance.heroQueue.Count != 0)
        hero = MinionManager.Instance.heroQueue[index];
        time = MinionManager.Instance.heroTime[index];

        if (isCheck == true)
            waitTimer(index);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.cost >= 0)
        {
            if (index == 0 && GameManager.Instance.cost < MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().cost)
            {
                return;
            }
            else if (index == 0 && GameManager.Instance.cost >= MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().cost)
            {
                BattleUIManager.Instance.UseCost(index);
                isCheck = true;
                //waitTimer(index);           
            }
            if (index == 1 && GameManager.Instance.cost < MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().cost)
            {
                return;
            }
            else if (index == 1 && GameManager.Instance.cost >= MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().cost)
            {
                BattleUIManager.Instance.UseCost(index);
                isCheck = true;
                //waitTimer(index);  
            }
        }
        else
        {
            return;
        }
        
        GameManager.Instance.heroesListIndex = index;
        GameManager.Instance.CanSetTile();

        BattleUIManager.Instance.attackRangeNodes = hero.attackRangeNodes.ToList();

        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().skeletonDataAsset = MinionManager.Instance.heroPrefabs[index].transform.GetChild(0).GetComponent<SkeletonAnimation>().skeletonDataAsset;
        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().initialSkinName = hero.gameObject.transform.GetChild(0).GetComponent<SkeletonAnimation>().initialSkinName;
        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().Initialize(true);

        BattleUIManager.Instance.settingCharacter.SetActive(true);

        // GameManager.Instance.hero = MinionManager.Instance.heroPrefabs[index];


    }

    public void waitTimer(int index)
    {
        Debug.Log("check");

        if (!tBG[index].activeSelf)
        {
            tBG[index].SetActive(true);
        }

        tBG[index].transform.position = transform.position;

        wTime[index].text = BattleUIManager.Instance.MinionWaitTime(index).ToString("F1") + "s".ToString();

        if (BattleUIManager.Instance.MinionWaitTime(index) <= 0 && isCheck == true)
        {
            tBG[index].SetActive(false);
            isCheck = false;
        }
    }
}

