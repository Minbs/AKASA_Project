using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;
using System.Linq;
using TMPro;

public class MinionButton : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    public int index;
    public DefenceMinion minion;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(MinionManager.Instance.minionQueue.Count != 0)
            minion = MinionManager.Instance.minionQueue[index].GetComponent<DefenceMinion>();

        if (BattleUIManager.Instance.isCheck == true)
        {
            MinionStanbyTimer(index);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.cost < MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().cost)
            return;

        GameManager.Instance.minionsListIndex = index;
        GameManager.Instance.ChangeMinionPositioningState();

        BattleUIManager.Instance.attackRangeNodes = minion.attackRangeNodes.ToList();

        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().
            skeletonDataAsset = MinionManager.Instance.minionPrefabs[index].transform.GetChild(0).GetComponent<SkeletonAnimation>().skeletonDataAsset;
        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().
            initialSkinName = MinionManager.Instance.minionPrefabs[index].transform.GetChild(0).GetComponent<SkeletonAnimation>().initialSkinName;
        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().Initialize(true);

        BattleUIManager.Instance.settingCharacter.SetActive(true);
    }

    public void MBtnTBGPosition()
    {
        BattleUIManager.Instance.tBG[index].transform.position = transform.position;
    }

    public void MinionStanbyTimer(int index)
    {
        MinionManager.Instance.minionPrefabs[index].
            GetComponent<DefenceMinion>().minionStandbyTime -= Time.deltaTime;

        if (MinionManager.Instance.minionPrefabs[index].
            GetComponent<DefenceMinion>().minionStandbyTime <= 0)
        {
            if (BattleUIManager.Instance.tBG[index].activeSelf)
            {
                BattleUIManager.Instance.isCheck = false;
                BattleUIManager.Instance.tBG[index].SetActive(false);
            }
        }

        BattleUIManager.Instance.wTime[index].text = 
            MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().
            minionStandbyTime.ToString("F1") + "s".ToString();
    }
}

