using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;
using System.Linq;

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
        if (BattleUIManager.Instance.cost >= 0)
        {
            if (index == 0 && BattleUIManager.Instance.cost < BattleUIManager.Instance.verityCost)
            {
                return;
            }
            else if (index == 0 && BattleUIManager.Instance.cost >= BattleUIManager.Instance.verityCost)
            {
                BattleUIManager.Instance.useCost(index);
            }
            if (index == 1 && BattleUIManager.Instance.cost < BattleUIManager.Instance.isabellaCost)
            {
                return;
            }
            else if (index == 1 && BattleUIManager.Instance.cost >= BattleUIManager.Instance.isabellaCost)
            {
                BattleUIManager.Instance.useCost(index);
            }
        }
        else
        {
            return;
        }


        GameManager.Instance.CanSetTile();

        BattleUIManager.Instance.attackRangeNodes = hero.attackRangeNodes.ToList();

        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().skeletonDataAsset = hero.skeletonData;
        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().initialSkinName = hero.gameObject.transform.GetChild(0).GetComponent<SkeletonAnimation>().initialSkinName;
        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().Initialize(true);

        BattleUIManager.Instance.settingCharacter.SetActive(true);
        GameManager.Instance.heroesListIndex = index;
       // GameManager.Instance.hero = MinionManager.Instance.heroPrefabs[index];
    }
}
