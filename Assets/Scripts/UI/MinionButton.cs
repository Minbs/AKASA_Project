using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;
using System.Linq;
using TMPro;
using DG.Tweening;

public class MinionButton : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    public int index;
    public int UnitIndex;
    public int test;
    public GameObject SelectdImage;
    public GameObject GamedataManager;
    public GameObject StatUI;
    public TextMeshProUGUI HpText;
    public TextMeshProUGUI DefText;
    public TextMeshProUGUI AtkText;
    public TextMeshProUGUI AtkSpeedText;
    public TextMeshProUGUI CostText;


    private void Start()
    {
        GamedataManager.GetComponent<CSV_Player_Status>().StartParsing(this.name);
        HpText.text = GamedataManager.GetComponent<CSV_Player_Status>().Call_Stat_CSV(this.name, 1).HP.ToString();
        DefText.text = GamedataManager.GetComponent<CSV_Player_Status>().Call_Stat_CSV(this.name, 1).Def.ToString();
        AtkText.text = GamedataManager.GetComponent<CSV_Player_Status>().Call_Stat_CSV(this.name, 1).Atk.ToString();
        AtkSpeedText.text = GamedataManager.GetComponent<CSV_Player_Status>().Call_Stat_CSV(this.name, 1).AtkSpeed.ToString();
        CostText.text = GamedataManager.GetComponent<CSV_Player_Status>().Call_Stat_CSV(this.name, 1).BuyCost.ToString();
    }
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

    public void OnMouseOver()
    {
        // this.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        this.transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.3f);
        StatUI.SetActive(true);
        SelectdImage.SetActive(true);
    }
    public void OnMouseExit()
    {
        this.transform.DOScale(new Vector3(1.0f, 1.0f, 1), 0.3f);
        SelectdImage.SetActive(false);
        StatUI.SetActive(false);

    }

}

