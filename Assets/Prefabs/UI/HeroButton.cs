using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;
using System.Linq;

public class HeroButton : Singleton<HeroButton>, IPointerDownHandler
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
        GameManager.Instance.CanSetTile();

        BattleUIManager.Instance.attackRangeNodes = hero.attackRangeNodes.ToList();

        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().skeletonDataAsset = hero.skeletonData;
        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().initialSkinName = hero.gameObject.transform.GetChild(0).GetComponent<SkeletonAnimation>().initialSkinName;
        BattleUIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().Initialize(true);

        BattleUIManager.Instance.settingCharacter.SetActive(true);
        GameManager.Instance.heroesListIndex = index;
       // GameManager.Instance.hero = HeroManager.Instance.heroPrefabs[index];
    }
}
