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
    public Minion hero;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(MinionManager.Instance.heroQueue.Count != 0)
        hero = MinionManager.Instance.heroQueue[index];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.cost < MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().cost)
            return;

        GameManager.Instance.heroesListIndex = index;
        GameManager.Instance.CanSetTile();

        BattleUIManager.Instance.attackRangeNodes = hero.attackRangeNodes.ToList();

        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().skeletonDataAsset = MinionManager.Instance.heroPrefabs[index].transform.GetChild(0).GetComponent<SkeletonAnimation>().skeletonDataAsset;
        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().initialSkinName = hero.gameObject.transform.GetChild(0).GetComponent<SkeletonAnimation>().initialSkinName;
        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().Initialize(true);

        BattleUIManager.Instance.settingCharacter.SetActive(true);

        // GameManager.Instance.hero = MinionManager.Instance.heroPrefabs[index];
    }

    public void MBtnTBGPosition()
    {
        BattleUIManager.Instance.tBG[index].transform.position = transform.position;
    }
}

