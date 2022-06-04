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
    public int UnitIndex;

    // Update is called once per frame
    void Update()
    {
      //  MBtnTBGPosition();
    }
    void start()
    {
        UnitIndex = index;
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

    //public void MBtnTBGPosition() => BattleUIManager.Instance.tBG[index].transform.position = transform.position;
}

