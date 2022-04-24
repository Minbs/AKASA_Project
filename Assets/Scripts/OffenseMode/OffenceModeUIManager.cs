using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System.Linq;
using TMPro;


/// <summary>
/// </summary>
public class OffenceModeUIManager : Singleton<OffenceModeUIManager>
{

    public GameObject worldCanvas;

    public GameObject settingCharacter;
    public SkeletonDataAsset skeletonDataAsset;

    public bool isSettingCharacterOn = true;

    //
    const int maxCost = 99;
    int[] maxMinionCount = { 3, 5 };
    List<GameObject> enemiesList = new List<GameObject>();

    ///text - 0:LimitTimeMin, 1:LimitTimeColon, 2:LimitTimeSec, 3:GameTargetCurrent, 4:GameTargetMax, 5:MinionAvailable
    public TextMeshProUGUI[] text;

    public TextMeshProUGUI costText;


    [SerializeField]
    int regenTime = 3;

    float time = 0, phaseWaitingTime;
    int min, sec, currentEnemyCount = 0;
    bool isPhaseCheck;

    public GameObject minionBtnTranslucentBG;
    public List<GameObject> tBG = new List<GameObject>();
    public List<TextMeshProUGUI> wTime = new List<TextMeshProUGUI>();
    public bool isCheck = false;
    bool isSoundCheck = true;
    public bool isCostCheck = false;

    public GameObject wBG;
    public List<OffenceModeMinionButton> mBtn;

    AudioSource audioSource;

    void Start()
    {

        Init();
    }

    void Update()
    {
        if (isSettingCharacterOn)
            SetSettingCharacterMousePosition();


        RegenCost();
    }

    public void SetSettingCharacterMousePosition()
    {
        settingCharacter.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    }



    //
    void Init()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        time = 0;
       // enemiesList = OffenseModeGameManager.Instance.enemiesList;
        //costText.text = OffenseModeGameManager.Instance.cost.ToString();

        /*
        for (int i = 0; i < wBG.transform.childCount; i++)
        {
            mBtn.Add(wBG.GetComponentsInChildren<OffenceModeMinionButton>()[i]);
            //MinionManager.Instance.heroPrefabs[mBtn[i].index].GetComponent<Minion>().minionStandbyTime = 1;
        }

        for (int i = 0; i < 12; i++)
        {
            tBG.Add(minionBtnTranslucentBG.transform.GetChild(i).gameObject);
            wTime.Add(tBG[i].GetComponentInChildren<TextMeshProUGUI>());

            if (tBG[i].activeSelf)
                tBG[i].SetActive(false);
        }
        */

    }




    void EnemeyCount()
    {
        currentEnemyCount = enemiesList.Count;
        if (currentEnemyCount >= 0)
            text[3].text = currentEnemyCount.ToString();
    }

    void UnitCount()
    {

    }


    /// <summary>
    /// 코스트 리젠 (regenTime마다 실행)
    /// </summary>
    void RegenCost()
    {
        time += Time.deltaTime;

        if (time >= regenTime)
        {
            if (OffenseModeGameManager.Instance.cost >= maxCost)
            {
                costText.text = OffenseModeGameManager.Instance.cost.ToString() + '+'.ToString();
                return;
            }

            OffenseModeGameManager.Instance.cost++;
            costText.text = OffenseModeGameManager.Instance.cost.ToString();

            time = 0;
        }
    }

    /// <summary>
    /// 캐릭터 배치후 코스트 소모
    /// </summary>
    public void UseCost(int index)
    {
        if (MinionManager.Instance.heroPrefabs.Count <= index)
            return;

        OffenseModeGameManager.Instance.cost -= MinionManager.Instance.heroPrefabs[index].GetComponent<DefenceMinion>().cost;
        costText.text = OffenseModeGameManager.Instance.cost.ToString();
    }

    public void DeploymentMinion(int index)
    {
        if (OffenseModeGameManager.Instance.cost >= 0)
        {
            if (OffenseModeGameManager.Instance.cost >= MinionManager.Instance.heroPrefabs[index].GetComponent<OffenceMinion>().cost)
            {
                UseCost(index);

                if (!tBG[index].activeSelf)
                {
                    mBtn[index].MBtnTBGPosition();
                    tBG[index].SetActive(true);
                    isCheck = true;
                }
            }
        }
        else
        {
            return;
        }
    }

    public void OnDoubleSpeedBtn()
    {
        if (Time.timeScale == 1)
            Time.timeScale = 2;       
        else       
            Time.timeScale = 0;       
    }

    public void OnPauseBtn()
    {
        if (Time.timeScale != 0)      
            Time.timeScale = 0;       
        else       
            Time.timeScale = 1;      
    }

}
