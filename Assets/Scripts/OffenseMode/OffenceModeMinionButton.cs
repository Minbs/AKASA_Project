using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;
using System.Linq;
using TMPro;

public class OffenceModeMinionButton : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    public int index;
    public Minion minion;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(MinionManager.Instance.heroQueue.Count != 0)
            minion = MinionManager.Instance.heroQueue[index];

        if (OffenceModeUIManager.Instance.isCheck == true)
        {
            MinionStanbyTimer(index);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (OffenseModeGameManager.Instance.cost < MinionManager.Instance.heroPrefabs[index].GetComponent<OffenceMinion>().cost)
            return;

        OffenseModeGameManager.Instance.heroesListIndex = index;
        OffenseModeGameManager.Instance.CanSetTile();

        // BattleUIManager.Instance.attackRangeNodes = minion.attackRangeNodes.ToList();

        OffenceModeUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().
            skeletonDataAsset = MinionManager.Instance.heroPrefabs[index].transform.
            GetChild(0).GetComponent<SkeletonAnimation>().skeletonDataAsset;
        OffenceModeUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().
            initialSkinName = minion.gameObject.transform.GetChild(0).GetComponent<SkeletonAnimation>().initialSkinName;
        OffenceModeUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().Initialize(true);

        OffenceModeUIManager.Instance.settingCharacter.SetActive(true);

        // GameManager.Instance.hero = MinionManager.Instance.heroPrefabs[index];
        
    }

    public void MBtnTBGPosition()
    {
        BattleUIManager.Instance.tBG[index].transform.position = transform.position;
    }

    public void MinionStanbyTimer(int index)
    {
        MinionManager.Instance.heroPrefabs[index].
            GetComponent<OffenceMinion>().minionStandbyTime -= Time.deltaTime;

        if (MinionManager.Instance.heroPrefabs[index].
            GetComponent<OffenceMinion>().minionStandbyTime <= 0)
        {
            if (BattleUIManager.Instance.tBG[index].activeSelf)
            {
                BattleUIManager.Instance.isCheck = false;
                BattleUIManager.Instance.tBG[index].SetActive(false);
            }
        }

        BattleUIManager.Instance.wTime[index].text = 
            MinionManager.Instance.heroPrefabs[index].GetComponent<OffenceMinion>().
            minionStandbyTime.ToString("F1") + "s".ToString();
    }
}

