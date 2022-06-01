using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System.Linq;
using TMPro;
using DG.Tweening;

//public enum Phase //Phase 사용 X GameManager State 사용하기
//{
//    Wait,  //전투 대비
//    Start,  //전투 시작
//    Wave1,  //웨이브 1
//    Wave2,  //웨이브 2
//    Wave3,   //웨이브 3
//    Between //웨이브 사이간격
//}

/// <summary>
/// </summary>
public class BattleUIManager : Singleton<BattleUIManager>
{
    public GameObject worldCanvas;

    public GameObject DeployableTileImage;
    public Sprite NotDeployableTileSprite;
    public Sprite DeployableTileSprite;
    
    public SkeletonDataAsset skeletonDataAsset;

    //
    int[] maxMinionCount = { 3, 5 };
    List<GameObject> enemiesList = new List<GameObject>();

    [Space(10)]

    [Header("TopPanel-Text")]
    [Tooltip("(필요) 0:LimitTimeMin, 1:LimitTimeColon, 2:LimitTimeSec, 3:GameTargetCurrent, 4:GameTargetMax, 5:MinionAvailable")]
    public TextMeshProUGUI[] text;
    public TextMeshProUGUI wave;
    [SerializeField]
    int waveCount = 1;

    [Space(10)]

    [Header("Phase-Image")]
    [Tooltip("(필요) 0:Wait, 1:Battle, 2:Wave1, 3:Wave2, 4:Wave3")]
    public Image[] phase;

    [Space(10)]

    [Header("Cost-Text")]
    [Tooltip("(필요) CostValue")]
    public TextMeshProUGUI costText;
    [Tooltip("(필요) CostTimeText")]
    public TextMeshProUGUI costEarnedText;

    [Space(10)]

    [Header("WaitingTime-Text")]
    [Tooltip("(필요) 0:Wait, 1:Start, 2:Wave1, 3:Wave2, 4:Wave3, 5:Between")]
    [SerializeField]
    float[] WaitingTime;

    float time = 0, phaseWaitingTime;
    int maxEnemyCount = 20, min, sec, currentEnemyCount = 0;
    //bool isPhaseCheck;

    [Space(10)]

    // 변수 이름 기능이나 용도를 알 수 있게 바꾸기
    [Header("BottominionDeployPanelel-Object")]
    [Tooltip("(필요) DisableMinionObj")]
    public GameObject disableMinionObj;
    [HideInInspector]
    public List<GameObject> translucentTexture;
    [HideInInspector]
    public List<GameObject> edge;

    private float skillTime;

    [Tooltip("(필요) DisableSkillObj")]
    public GameObject disableSkillObj;
    [HideInInspector]
    public List<GameObject> skillTranslucentTexture = new List<GameObject>();
    [HideInInspector]
    public List<TextMeshProUGUI> SkillTime = new List<TextMeshProUGUI>();

    [Space(10)]

    //public bool isCheck = false;
    bool isSoundCheck = true;

    [Header("BottomPanel-Object")]
    [Space(5)]

    [Header("WaitPanel-Object")]
    [Tooltip("(필요) WaitPanelObj")]
    public GameObject waitPanelObj;
    [Tooltip("(필요) MinionDeployPanel")]
    public GameObject minionDeployPanel;
    [Tooltip("(필요) ObjectDeployPanel")]
    public GameObject objectDeployPanel;
    [Tooltip("(필요) MinionDeployContent")]
    public GameObject minionDeployContent;
    [Tooltip("(필요) ObjectDeployContent")]
    public GameObject objectDeployContent;

    [HideInInspector]
    public List<MinionButton> minionDeployButton;
    [HideInInspector]
    public List<Button> objectDeployButton;

    [Header("BattlePanel-Object")]
    [Tooltip("(필요) BattlePanelObj")]
    public GameObject battlePanelObj;
    [Tooltip("(필요) MinionSkillPanel")]
    public GameObject minionSkillPanel;
    [Tooltip("(필요) PlayerSkillPanel")]
    public GameObject playerSkillPanel;
    [Tooltip("(필요) MinionSkillContent")]
    public GameObject minionSkillContent;
    [Tooltip("(필요) PlayerSkillContent")]
    public GameObject playerSkillContent;

    [HideInInspector]
    public List<Button> minionSkillButton;
    [HideInInspector]
    public List<Button> playerSkillButton;

    bool isDeployBtnCheck = true;
    bool isSkillBtnCheck = true;

    AudioSource audioSource;

    [Space(10)]

    [Header("BottomButton-Object")]
    [Tooltip("(필요) BottomButton")]
    public GameObject bottomButton;
    private GameObject waitButtonObj;
    private GameObject battleButtonObj;
    [HideInInspector]
    public List<GameObject> waitButton;
    [HideInInspector]
    public List<GameObject> battleButton;

    [Space(10)]

    //카메라 관련 변수
    Vector3 startingPoint;
    Vector3 endingPoint;
    [Header("Camera-MoveTime")]
    [Tooltip("(필요) end 이동 진행시간")]
    public int endDuration = 3;
    [Tooltip("(필요) end 이동 딜레이")]
    public int endDelay = 1;
    [Tooltip("(필요) start 이동 진행시간")]
    public int startDuration = 4;
    [Tooltip("(필요) start 이동 딜레이")]
    public int startDelay = 5;

    [Space(10)]

    //fps 관련 변수
    private float fpsDeltaTime = 0;
    [Header("FPS")]
    [SerializeField, Range(1, 100)]
    private int fpsFontSize = 25;
    [SerializeField]
    private Color fpsColor = Color.green;
    private int fpsIndex = 0;
    public bool isFpsShow;

    [Header("Upgrade-Sell")]
    public GameObject sellPanel;
    public GameObject incomeUpgradeButton;
    public TextMeshProUGUI incomeText;
    public GameObject minionUpgradeUI;

    void Start()
    {
        incomeUpgradeButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(IncomeUpgrade);
        Init();
    }

    void Update()
    {

        FPS();
        //OnDeployButton();

        //코스트 획득량당 코스트 획득
        RegenCost();

        if (GameManager.Instance.state == State.WAIT)
        {
            if (WaitingTime[(int)State.WAIT] >= 0) Active((int)State.WAIT);
            WaitTime();
            ////disableMinionObj.SetActive(true);
            //waitButtonObj.SetActive(true);
            //battleButtonObj.SetActive(false);
        }
        if (GameManager.Instance.state == State.BATTLE)
        {

            //상단 패널에 타이머UI에서 웨이브UI로 변경
            BattleTime();
            //에너미 카운트수 체크
            EnemeyCount();
            //배경음악 재생
            if (isSoundCheck) audioSource.Play(); isSoundCheck = false;
            //일시정지시 배경음악 일시중지, 일시정지 해제시 배경음악 재생
            if (GameManager.Instance.gameSpeed == 0) audioSource.Pause(); else audioSource.UnPause();
            //스타트 페이즈 대기시간만큼 팝업UI 출력 후 해제
            if (WaitingTime[(int)State.BATTLE] >= 0) Active((int)State.BATTLE);
            //웨이브1 페이즈 대기시간만큼 팝업UI 출력 후 해제
          //  if (WaitingTime[(int)Phase.Wave1] >= 0) Active((int)Phase.Wave1);

            //disableMinionObj.SetActive(false);
            //waitButtonObj.SetActive(false);
            //battleButtonObj.SetActive(true);
        }
        if (GameManager.Instance.state == State.WAVE_END)
        {
            battlePanelObj.SetActive(false);
            waitPanelObj.SetActive(true);
            //minionDeployPanel.SetActive(true);
        }
        else
        {

        }





    }

    void Init()
    {
        time = 0;
        audioSource = gameObject.GetComponent<AudioSource>();
        enemiesList = GameManager.Instance.enemiesList;
        costText.text = GameManager.Instance.cost.ToString();
        costEarnedText.text = GameManager.Instance.costTime.ToString();
        startingPoint = Camera.main.transform.localPosition;
        endingPoint = new Vector3(8.55f, 16.52f, -16.04f);

        //
        for (int i = 0; i < disableMinionObj.transform.childCount; i++)
        {
            translucentTexture.Add(disableMinionObj.transform.GetChild(i).gameObject);
            edge.Add(translucentTexture[i].transform.GetChild(0).gameObject);
            if (translucentTexture[i].activeSelf) translucentTexture[i].SetActive(false);
            if (edge[i].activeSelf) edge[i].SetActive(false);
        }

        for (int i = 0; i < disableSkillObj.transform.childCount; i++)
        {
            skillTranslucentTexture.Add(disableSkillObj.transform.GetChild(i).gameObject);
            SkillTime.Add(skillTranslucentTexture[i].GetComponentInChildren<TextMeshProUGUI>());
            if (skillTranslucentTexture[i].activeSelf) skillTranslucentTexture[i].SetActive(false);
        }

        //미니언 버튼
        for (int i = 0; i < minionDeployContent.transform.childCount; i++)
            minionDeployButton.Add(minionDeployContent.GetComponentsInChildren<MinionButton>()[i]);

        //오브젝트 버튼
        for (int i = 0; i < objectDeployContent.transform.childCount; i++)
            objectDeployButton.Add(objectDeployContent.GetComponentsInChildren<Button>()[i]);

        //미니언 스킬 버튼
        for (int i = 0; i < minionSkillContent.transform.childCount; i++)
            minionSkillButton.Add(minionSkillContent.GetComponentsInChildren<Button>()[i]);

        //플레이어 스킬 버튼
        for (int i = 0; i < playerSkillContent.transform.childCount; i++)
            playerSkillButton.Add(playerSkillContent.GetComponentsInChildren<Button>()[i]);

        //전투대비 배치
        waitButtonObj = bottomButton.transform.GetChild(0).gameObject;
        //전투시작 배치
        battleButtonObj = bottomButton.transform.GetChild(1).gameObject;

        waitButtonObj.SetActive(true);

        //전투대비 버튼
        for (int i = 0; i < waitButtonObj.transform.childCount; i++)
            waitButton.Add(waitButtonObj.transform.GetChild(i).gameObject);

        //전투시작 버튼
        for (int i = 0; i < battleButtonObj.transform.childCount; i++)
            battleButton.Add(battleButtonObj.transform.GetChild(i).gameObject);

        for (int i = 0; i < 3; i++)
            if (text[i].gameObject.activeSelf) text[i].gameObject.SetActive(false);
        text[3].text = currentEnemyCount.ToString();
        text[4].text = maxEnemyCount.ToString();
        text[5].text = maxMinionCount[0].ToString();

        for (int i = 0; i < 5; i++)
            if (phase[i].gameObject.activeSelf) phase[i].gameObject.SetActive(false);

        if (wave.gameObject.activeSelf) wave.gameObject.SetActive(false);

        if (minionDeployPanel.activeSelf) 
        { 
            objectDeployPanel.SetActive(false);
            battlePanelObj.SetActive(false);
        } 
        else
            minionDeployPanel.SetActive(true);

        if (waitButtonObj.activeSelf) battleButtonObj.SetActive(false);

        Camera.main.transform.DOMove(endingPoint, endDuration).SetDelay(endDelay);
        Camera.main.transform.DOMove(startingPoint, startDuration).SetDelay(startDelay);
    }

    void BattleTime()
    {
        for (int i = 0; i < 3; i++)
            text[i].gameObject.SetActive(false);

        waitPanelObj.SetActive(false);

        wave.gameObject.SetActive(true);
        wave.text = "Wave ".ToString() + waveCount.ToString();
    }

    void WaitTime()
    {
        wave.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++) text[i].gameObject.SetActive(true);

        float time = GameManager.Instance.currentWaitTimer;
        min = (int)time / 60;
        sec = ((int)time - min * 60) % 60;

        if (min <= 0 && sec <= 0)
        {
            text[0].text = 0.ToString();
            text[2].text = 0.ToString();

            waitPanelObj.SetActive(false);
            battlePanelObj.SetActive(true);

            //disableMinionObj.SetActive(false);
            waitButtonObj.SetActive(false);
            battleButtonObj.SetActive(true);
        }
        else
        {
            if (sec >= 60)
            {
                min += 1;
                sec -= 60;
            }
            else
            {
                text[0].text = min.ToString();
                text[2].text = sec.ToString();

                //disableMinionObj.SetActive(true);

                waitPanelObj.SetActive(true);
                battlePanelObj.SetActive(false);

                waitButtonObj.SetActive(true);
                battleButtonObj.SetActive(false);
            }
        }
    }

    void EnemeyCount()
    {
        currentEnemyCount = enemiesList.Count;
        text[3].text = currentEnemyCount >= 0 ? currentEnemyCount.ToString() : 0.ToString();
    }

    /// <summary> 페이즈 팝업UI 출력, 지정된 시간 후 해제 </summary> <param name="index"></param>
    public void Active(int index) // Phase State로 바꾸기
    {
        switch (index)
        {
            case (int)State.WAIT:
                WaitingTime[(int)State.WAIT] -= Time.deltaTime;
                phaseWaitingTime = WaitingTime[(int)State.WAIT];
                //isPhaseCheck = false;
                break;
            case (int)State.BATTLE:
                WaitingTime[(int)State.BATTLE] -= Time.deltaTime;
                phaseWaitingTime = WaitingTime[(int)State.BATTLE];
                //isPhaseCheck = false;
                break;
            //case (int)Phase.Wave1:
            //    StartCoroutine("PhaseDelay");
            //    if (isPhaseCheck)
            //    {
            //        WaitingTime[(int)Phase.Wave1] -= Time.deltaTime;
            //        phaseWaitingTime = WaitingTime[(int)Phase.Wave1];
            //        isPhaseCheck = false;
            //    }
            //    else
            //        return;
            //    break;
            //case (int)Phase.Wave2:
            //    StartCoroutine("PhaseDelay");
            //    if (isPhaseCheck)
            //    {
            //        WaitingTime[(int)Phase.Wave2] -= Time.deltaTime;
            //        phaseWaitingTime = WaitingTime[(int)Phase.Wave2];
            //        isPhaseCheck = false;
            //    }
            //    else
            //        return;
            //    break;
            //case (int)Phase.Wave3:
            //    StartCoroutine("PhaseDelay");
            //    if (isPhaseCheck)
            //    {
            //        WaitingTime[(int)Phase.Wave3] -= Time.deltaTime;
            //        phaseWaitingTime = WaitingTime[(int)Phase.Wave3];
            //        isPhaseCheck = false;
            //    }
            //    else
            //        return;
            //    break;
            default:
                break;
        }

        if (phaseWaitingTime >= 0) phase[index].gameObject.SetActive(true);
        else phase[index].gameObject.SetActive(false);
    }

    /// <summary> 코스트 리젠 (GameManager.Instance.earnedCost마다 실행) </summary>
    void RegenCost()
    {
        time += Time.deltaTime;

        if (time >= GameManager.Instance.costTime)
        {
            GameManager.Instance.cost += GameManager.Instance.totalIncome;

            costText.text = GameManager.Instance.cost.ToString();
            time = 0;
        }
    }

    /// <summary> 캐릭터 배치후 코스트 소모 </summary>
    public void UseCost(int index)
    {
        if (MinionManager.Instance.minionPrefabs.Count <= index
            || GameManager.Instance.cost < MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().cost) return;
        GameManager.Instance.cost -= MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().cost;

        Debug.Log(MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().cost);
        costText.text = GameManager.Instance.cost.ToString();
    }

    /// <summary> 미니언 배치 </summary> <param name="index"></param>
    public void DeploymentMinion(int index)
    {
        if (GameManager.Instance.cost >= 0)
        {
            if (GameManager.Instance.cost >= MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().cost)
            {
                if (!translucentTexture[index].activeSelf)
                {
                    //minionDeployButton[index].MBtntranslucentTexturePosition();
                    translucentTexture[index].SetActive(true);
                }
            }
        }
        else
        {
            return;
        }
    }

    public void SkillWaitingTimer(int index)
    {
        skillTime -= Time.deltaTime;

        if (skillTime <= 0)
            if (skillTranslucentTexture[index].activeSelf) skillTranslucentTexture[index].SetActive(false);

        SkillTime[index].text = skillTime.ToString("F1") + "s".ToString();
    }

    //GameManager SetGameSpeed 함수 사용
    //   public void OnDoubleSpeedButton() => GameManager.Instance.gameSpeed =

    //  SetGameSpeed
    /* 
            public void OnDoubleSpeedButton() => GameManager.Instance.gameSpeed =

            GameManager.Instance.gameSpeed == 1 || GameManager.Instance.gameSpeed == 0 ?
            GameManager.Instance.gameSpeed = 2 : GameManager.Instance.gameSpeed = 1;

    public void OnPauseButton() => GameManager.Instance.gameSpeed =
        GameManager.Instance.gameSpeed == 0 ? GameManager.Instance.gameSpeed = 1 : GameManager.Instance.gameSpeed = 0;
    */

    private void OnDeployButtonCheck() => isDeployBtnCheck = minionDeployPanel.activeSelf == true ? false : true;
    private void OnSkillButtonCheck() => isSkillBtnCheck = minionSkillPanel.activeSelf == true ? false : true;

    public void OnMinionDeployButtonCheck()
    {
        OnDeployButtonCheck();

        if (isDeployBtnCheck && GameManager.Instance.state == State.WAIT)
        {
            waitButton[0].transform.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            waitButton[1].transform.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(49, 49, 55, 255);
            waitButton[0].transform.GetChild(1).gameObject.SetActive(true);
            waitButton[1].transform.GetChild(1).gameObject.SetActive(false);

            minionDeployPanel.SetActive(true);
            objectDeployPanel.SetActive(false);
            //disableMinionObj.SetActive(true);
        }
    }

    public void OnObjectDeployButtonCheck()
    {
        OnDeployButtonCheck();

        if (!isDeployBtnCheck && GameManager.Instance.state == State.WAIT)
        {
            waitButton[0].transform.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(49, 49, 55, 255);
            waitButton[1].transform.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            waitButton[0].transform.GetChild(1).gameObject.SetActive(false);
            waitButton[1].transform.GetChild(1).gameObject.SetActive(true);

            minionDeployPanel.SetActive(false);
            objectDeployPanel.SetActive(true);
            //disableMinionObj.SetActive(false);
        }
    }

    public void OnMinionSkillButtonCheck()
    {
        OnSkillButtonCheck();

        if (isSkillBtnCheck && GameManager.Instance.state == State.BATTLE)
        {
            battleButton[0].transform.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            battleButton[1].transform.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(49, 49, 55, 255);
            battleButton[0].transform.GetChild(1).gameObject.SetActive(true);
            battleButton[1].transform.GetChild(1).gameObject.SetActive(false);

            minionSkillPanel.SetActive(true);
            playerSkillPanel.SetActive(false);
            //disableMinionObj.SetActive(true);
        }
    }

    public void OnPlayerSkillButtonCheck()
    {
        OnSkillButtonCheck();

        if (!isSkillBtnCheck && GameManager.Instance.state == State.BATTLE)
        {
            battleButton[0].transform.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(49, 49, 55, 255);
            battleButton[1].transform.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            battleButton[0].transform.GetChild(1).gameObject.SetActive(false);
            battleButton[1].transform.GetChild(1).gameObject.SetActive(true);

            minionSkillPanel.SetActive(false);
            playerSkillPanel.SetActive(true);
            //disableMinionObj.SetActive(false);
        }
    }

 //   IEnumerator PhaseDelay()
  //  {
     //   yield return new WaitForSeconds(WaitingTime[5]);
   //     isPhaseCheck = true;
//    }

    public void SkillButton()
    {
        GameObject wraith = GameObject.Find("wraith(Clone)");
        wraith.GetComponent<UnitStateMachine>().ChangeState(wraith.GetComponent<UnitStateMachine>().SkillPerformState);
    }

    private void FPS()
    {
        fpsDeltaTime += (Time.unscaledDeltaTime - fpsDeltaTime) * 0.1f;

        if (Input.GetKeyDown(KeyCode.F3))
        {
            isFpsShow = !isFpsShow;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            switch (fpsIndex)
            {
                case 0:
                    Application.targetFrameRate = 25;
                    fpsIndex++;
                    break;
                case 1:
                    Application.targetFrameRate = 30;
                    fpsIndex++;
                    break;
                case 2:
                    Application.targetFrameRate = 60;
                    fpsIndex++;
                    break;
                case 3:
                    Application.targetFrameRate = 80;
                    fpsIndex++;
                    break;
                case 4:
                    Application.targetFrameRate = 120;
                    fpsIndex++;
                    break;
                case 5:
                    Application.targetFrameRate = 144;
                    fpsIndex++;
                    break;
                case 6:
                    Application.targetFrameRate = 200;
                    fpsIndex++;
                    break;
                case 7:
                    Application.targetFrameRate = 240;
                    fpsIndex++;
                    break;
                case 8:
                    Application.targetFrameRate = -1;
                    fpsIndex++;
                    break;
                case 9:
                    fpsIndex = 0;
                    break;
            }
        }
    }

    public void SetSellCostText(float cost)
    {
        sellPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "판매 : " + cost;
    }

    public void SetIncomeUpgradeButtonActive(bool active)
    {
        IncomeUpgradeData data;

        if (GameManager.Instance.incomeUpgradeCount + 1>= GameManager.Instance.incomeUpgradeDatas.Count)
        {
            incomeUpgradeButton.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "UpgradeComplete";
        }
        else
        {
            data = GameManager.Instance.incomeUpgradeDatas[GameManager.Instance.incomeUpgradeCount + 1];
            incomeUpgradeButton.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Cost: " + data.upgradeCost + " / " + "Income: " + data.income;
        }

        incomeUpgradeButton.SetActive(active);
    }

    public void IncomeUpgrade()
    {
        if (GameManager.Instance.incomeUpgradeCount + 1 >= GameManager.Instance.incomeUpgradeDatas.Count)
            return;

        IncomeUpgradeData data;
        data = GameManager.Instance.incomeUpgradeDatas[GameManager.Instance.incomeUpgradeCount + 1];

        if (GameManager.Instance.cost <data.upgradeCost)
        {
            Debug.Log("코스트 부족");
            return;
        }

        GameManager.Instance.totalIncome = data.income;
        incomeText.text = GameManager.Instance.totalIncome.ToString();
        GameManager.Instance.cost -= data.upgradeCost;
        GameManager.Instance.incomeUpgradeCount++;
        SetIncomeUpgradeButtonActive(true);

        costText.text = GameManager.Instance.cost.ToString();
    }

    private GameObject upgradeMinion;
    private Stat currentStat = null;
    private Stat nextLevelStat = null;
    public void SetMinionUpgradeUI(GameObject minion)
    {
        minionUpgradeUI.SetActive(true);
        minionUpgradeUI.GetComponent<RectTransform>().anchoredPosition3D = minion.transform.position;
        upgradeMinion = minion;

        // 미니언 이름으로 데이터 쉽게 가져올 수 있게 바꾸기
        if(minion.GetComponent<Minion>().Unitname == "Verity")
        {
            currentStat = CSV_Player_Status.Instance.VeriyStat_Array[minion.GetComponent<Unit>().Level - 1];
            nextLevelStat = CSV_Player_Status.Instance.VeriyStat_Array[minion.GetComponent<Unit>().Level];
        }
        else if (minion.GetComponent<Minion>().Unitname == "Isabella")
        {
            currentStat = CSV_Player_Status.Instance.IsabellaStat_Array[minion.GetComponent<Unit>().Level - 1];
            nextLevelStat = CSV_Player_Status.Instance.IsabellaStat_Array[minion.GetComponent<Unit>().Level];
        }
        else if (minion.GetComponent<Minion>().Unitname == "Wraith")
        {
            currentStat = CSV_Player_Status.Instance.WraithStat_Array[minion.GetComponent<Unit>().Level - 1];
            nextLevelStat = CSV_Player_Status.Instance.WraithStat_Array[minion.GetComponent<Unit>().Level];
        }
        else if (minion.GetComponent<Minion>().Unitname == "Zippo")
        {
            currentStat = CSV_Player_Status.Instance.ZippoStat_Array[minion.GetComponent<Unit>().Level - 1];
            nextLevelStat = CSV_Player_Status.Instance.ZippoStat_Array[minion.GetComponent<Unit>().Level];
        }
        
        minionUpgradeUI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = currentStat.UpgradeCost.ToString();
        // 체력 텍스트
        minionUpgradeUI.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = currentStat.HP.ToString() + " → " + nextLevelStat.HP.ToString();
        // 방어력 텍스트
        minionUpgradeUI.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = currentStat.Def.ToString() + " → " + nextLevelStat.Def.ToString();
        // 공격력 텍스트
        minionUpgradeUI.transform.GetChild(1).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = currentStat.Atk.ToString() + " → " + nextLevelStat.Atk.ToString();
        // 공격속도 텍스트
        minionUpgradeUI.transform.GetChild(1).GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = currentStat.AtkSpeed.ToString() + " → " + nextLevelStat.AtkSpeed.ToString();


    }

    public void MinionUpgradeButton()
    {
       upgradeMinion.GetComponent<Unit>().Level++;
       upgradeMinion.GetComponent<Unit>().SetUnitStat(nextLevelStat);
       SetMinionUpgradeUI(upgradeMinion);
    }



    private void OnGUI()
    {
        if (isFpsShow)
        {
            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(30, 30, Screen.width, Screen.height);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = fpsFontSize;
            style.normal.textColor = fpsColor;

            float ms = fpsDeltaTime * 1000f;
            float fps = 1.0f / fpsDeltaTime;
            string text = string.Format("{0:0.} FPS ({1:0.0} ms)", fps, ms);

            GUI.Label(rect, text, style);
        }
    }
}
