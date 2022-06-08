using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;
using System.Linq;
using TMPro;
using DG.Tweening;

public class SkillButton : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    public Image SkillIcon;
    public int index;
    public int UnitIndex;


    // Update is called once per frame
    void Update()
    {
        //  MBtnTBGPosition();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.state == State.BATTLE)
            return;



        if (GameManager.Instance.cost < MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().cost)
            return;

        GameManager.Instance.minionsListIndex = index;
        GameManager.Instance.ChangeMinionPositioningState();

        if (GameManager.Instance.settingCharacter)
            Destroy(GameManager.Instance.settingCharacter);

        GameManager.Instance.settingCharacter = Instantiate(MinionManager.Instance.minionPrefabs[index], MinionManager.Instance.transform);
    }

    private void OnMouseOver()
    {
        // this.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        this.transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.3f);
        SkillIcon.gameObject.SetActive(true);

}
    private void OnMouseExit()
    {
        this.transform.DOScale(new Vector3(1.0f, 1.0f, 1), 0.3f);
        SkillIcon.gameObject.SetActive(false);
        //this.transform.localScale = new Vector3(1.0f, 1.0f, 1);
    }
    //public void MBtnTBGPosition() => BattleUIManager.Instance.tBG[index].transform.position = transform.position;
}

