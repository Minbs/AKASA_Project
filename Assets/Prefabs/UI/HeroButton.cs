using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;
using System.Linq;

public class HeroButton : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    public int index;
    public Hero hero;

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if(HeroManager.Instance.heroQueue.Count != 0)
        hero = HeroManager.Instance.heroQueue[index];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.CanSetTile();

        UIManager.Instance.attackRangeNodes = hero.attackRangeNodes.ToList();

        UIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().skeletonDataAsset = hero.skeletonData;
        UIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().Initialize(true);
        UIManager.Instance.settingCharacter.SetActive(true);
    }
}
