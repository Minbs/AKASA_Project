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
        if(MinionManager.Instance.minionQueue.Count != 0)
            minion = MinionManager.Instance.minionQueue[index];

        if (OffenceModeUIManager.Instance.isCheck == true)
        {
            MinionStanbyTimer(index);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (OffenceModeGameManager.Instance.cost < MinionManager.Instance.minionPrefabs[index].GetComponent<OffenceMinion>().cost)
            return;

        OffenceModeGameManager.Instance.minionListIndex = index;
        OffenceModeGameManager.Instance.ChangeMinionPositioningState(); // 미니언 배치 상태로 전환

        // MinionManager의 index 위치에 있는 미니언 스파인 데이터 대입
        OffenceModeUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().skeletonDataAsset = MinionManager.Instance.minionPrefabs[index].transform.GetChild(0).GetComponent<SkeletonAnimation>().skeletonDataAsset; 
        OffenceModeUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().initialSkinName = minion.gameObject.transform.GetChild(0).GetComponent<SkeletonAnimation>().initialSkinName;
        OffenceModeUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().Initialize(true);

        OffenceModeUIManager.Instance.settingCharacter.SetActive(true);
    }

    public void MBtnTBGPosition()
    {
        BattleUIManager.Instance.tBG[index].transform.position = transform.position;
    }

    public void MinionStanbyTimer(int index)
    {
        MinionManager.Instance.minionPrefabs[index].
            GetComponent<OffenceMinion>().minionStandbyTime -= Time.deltaTime;

        if (MinionManager.Instance.minionPrefabs[index].
            GetComponent<OffenceMinion>().minionStandbyTime <= 0)
        {
            if (BattleUIManager.Instance.tBG[index].activeSelf)
            {
                BattleUIManager.Instance.tBG[index].SetActive(false);
            }
        }

        BattleUIManager.Instance.wTime[index].text = 
            MinionManager.Instance.minionPrefabs[index].GetComponent<OffenceMinion>().
            minionStandbyTime.ToString("F1") + "s".ToString();
    }
}

