using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System.Linq;
using TMPro;

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
    //const int maxCost = 99;
    int[] maxMinionCount = { 3, 5 };
    List<GameObject> enemiesList = new List<GameObject>();

    //text - 0:LimitTimeMin, 1:LimitTimeColon, 2:LimitTimeSec, 3:GameTargetCurrent, 4:GameTargetMax, 5:MinionAvailable
    public TextMeshProUGUI[] text;
    //phase - 0:Wait, 1:Start, 2:Wave1, 3:Wave2, 4:Wave3
    public Image[] phase;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI costEarnedText;

    //WaitingTime - 0:Wait, 1:Start, 2:Wave1, 3:Wave2, 4:Wave3, 5:Between
    [SerializeField]
    float[] WaitingTime;
    [SerializeField]
    int maxEnemyCount = 3, waveCount = 1;

    float time = 0, phaseWaitingTime;
    int min, sec, currentEnemyCount = 0;
    bool isPhaseCheck;
    int[] enemyRewardCost = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

    // 변수 이름 기능이나 용도를 알 수 있게 바꾸기
    public GameObject mBG;
    public List<GameObject> tBG;
    public List<GameObject> edge;

    private float skillTime;

    public GameObject sBG;
    public List<GameObject> stBG = new List<GameObject>();
    public List<TextMeshProUGUI> wTime = new List<TextMeshProUGUI>();

    public bool isCheck = false;
    bool isSoundCheck = true;

    public GameObject mPan;
    public GameObject oPan;
    public GameObject mCnt;
    public GameObject oCnt;
    public List<MinionButton> mBtn;
    public List<Button> oBtn;
    bool isDeployBtnCheck = true;

    AudioSource audioSource;

    public GameObject bBObj;
    private GameObject rObj;
    private GameObject bObj;
    public List<GameObject> rBtn;
    public List<GameObject> bBtn;

    [SerializeField]
    private Tooltip tooltip;

    //fps 관련 변수
    private float fpsDeltaTime = 0;
    [SerializeField, Range(1, 100)]
    private int fpsFontSize = 25;
    [SerializeField]
    private Color fpsColor = Color.green;
    private int fpsIndex = 0;
    public bool isFpsShow;

    public GameObject sellPanel;
    public GameObject incomeUpgradeButton;
    public TextMeshProUGUI incomeText;
    void Start()
    {
        incomeUpgradeButton.GetComponent<Button>().onClick.AddListener(IncomeUpgrade);
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
            ////mBG.SetActive(true);
            //rObj.SetActive(true);
            //bObj.SetActive(false);
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
            //if (WaitingTime[(int)Phase.Wave1] >= 0) Active((int)Phase.Wave1);

            //mBG.SetActive(false);
            //rObj.SetActive(false);
            //bObj.SetActive(true);
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

        //
        for (int i = 0; i < mBG.transform.childCount; i++)
        {
            tBG.Add(mBG.transform.GetChild(i).gameObject);
            edge.Add(tBG[i].transform.GetChild(0).gameObject);
            if (tBG[i].activeSelf) tBG[i].SetActive(false);
            if (edge[i].activeSelf) edge[i].SetActive(false);
        }

        for (int i = 0; i < sBG.transform.childCount; i++)
        {
            stBG.Add(sBG.transform.GetChild(i).gameObject);
            wTime.Add(stBG[i].GetComponentInChildren<TextMeshProUGUI>());
            if (stBG[i].activeSelf) stBG[i].SetActive(false);
        }

        //미니언 버튼
        for (int i = 0; i < mCnt.transform.childCount; i++)
            mBtn.Add(mCnt.GetComponentsInChildren<MinionButton>()[i]);

        //오브젝트 버튼
        for (int i = 0; i < oCnt.transform.childCount; i++)
            oBtn.Add(oCnt.GetComponentsInChildren<Button>()[i]);

        //전투대비 배치
        rObj = bBObj.transform.GetChild(0).gameObject;
        //전투시작 배치
        bObj = bBObj.transform.GetChild(1).gameObject;

        rObj.SetActive(true);

        //전투대비 버튼
        for (int i = 0; i < rObj.transform.childCount; i++)
            rBtn.Add(rObj.transform.GetChild(i).gameObject);

        //전투시작 버튼
        for (int i = 0; i < bObj.transform.childCount; i++)
            bBtn.Add(bObj.transform.GetChild(i).gameObject);

        for (int i = 0; i < 3; i++)
            if (text[i].gameObject.activeSelf) text[i].gameObject.SetActive(false);
        text[3].text = currentEnemyCount.ToString();
        text[4].text = maxEnemyCount.ToString();
        text[5].text = maxMinionCount[0].ToString();

        for (int i = 0; i < 5; i++)
            if (phase[i].gameObject.activeSelf) phase[i].gameObject.SetActive(false);

        if (wave.gameObject.activeSelf) wave.gameObject.SetActive(false);

        if (mPan.activeSelf) oPan.SetActive(false);

        if (rObj.activeSelf) bObj.SetActive(false);
    }

    void BattleTime()
    {
        for (int i = 0; i < 3; i++)
            text[i].gameObject.SetActive(false);

        wave.gameObject.SetActive(true);
        wave.text = "Wave ".ToString() + waveCount.ToString();
    }

    void WaitTime()
    {
        //mBG.SetActive(true);
        rObj.SetActive(true);
        bObj.SetActive(false);
        wave.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++) text[i].gameObject.SetActive(true);

        float time = GameManager.Instance.currentWaitTimer;
        min = (int)time / 60;
        sec = ((int)time - min * 60) % 60;

        if (min <= 0 && sec <= 0)
        {
            text[0].text = 0.ToString();
            text[2].text = 0.ToString();

            mBG.SetActive(false);
            rObj.SetActive(false);
            bObj.SetActive(true);
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
                isPhaseCheck = false;
                break;
            case (int)State.BATTLE:
                WaitingTime[(int)State.BATTLE] -= Time.deltaTime;
                phaseWaitingTime = WaitingTime[(int)State.BATTLE];
                isPhaseCheck = false;
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
        if (MinionManager.Instance.minionPrefabs.Count <= index) return;
        GameManager.Instance.cost -= MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().cost;
        costText.text = GameManager.Instance.cost.ToString();
    }

    /// <summary> 미니언 배치 </summary> <param name="index"></param>
    public void DeploymentMinion(int index)
    {
        if (GameManager.Instance.cost >= 0)
        {
            if (GameManager.Instance.cost >= MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().cost)
            {
                if (!tBG[index].activeSelf)
                {
             //       mBtn[index].MBtnTBGPosition();
                    tBG[index].SetActive(true);
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
            if (stBG[index].activeSelf) stBG[index].SetActive(false);

        wTime[index].text = skillTime.ToString("F1") + "s".ToString();
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

    private void OnDeployButtonCheck() => isDeployBtnCheck = mPan.activeSelf == true ? false : true;

    public void OnMinionDeployButtonCheck()
    {
        OnDeployButtonCheck();

        if (isDeployBtnCheck && GameManager.Instance.state == State.WAIT)
        {
            rBtn[0].transform.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            rBtn[1].transform.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(49, 49, 55, 255);
            rBtn[0].transform.GetChild(1).gameObject.SetActive(true);
            rBtn[1].transform.GetChild(1).gameObject.SetActive(false);

            mPan.SetActive(true);
            oPan.SetActive(false);
            mBG.SetActive(true);
        }
    }

    public void OnObjectDeployButtonCheck()
    {
        OnDeployButtonCheck();

        if (!isDeployBtnCheck && GameManager.Instance.state == State.WAIT)
        {
            rBtn[0].transform.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(49, 49, 55, 255);
            rBtn[1].transform.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            rBtn[0].transform.GetChild(1).gameObject.SetActive(false);
            rBtn[1].transform.GetChild(1).gameObject.SetActive(true);

            mPan.SetActive(false);
            oPan.SetActive(true);
            mBG.SetActive(false);
        }
    }

    IEnumerator PhaseDelay()
    {
        yield return new WaitForSeconds(WaitingTime[5]);
        isPhaseCheck = true;
    }

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

    public void SetSellCostText(int cost)
    {
        sellPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "판매 : " + cost;
    }

    public void SetIncomeUpgradeButtonActive(bool active)
    {
        IncomeUpgradeData data;

        if (GameManager.Instance.incomeUpgradeCount + 1>= GameManager.Instance.incomeUpgradeDatas.Count)
        {
            incomeUpgradeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "UpgradeComplete";
        }
        else
        {
            data = GameManager.Instance.incomeUpgradeDatas[GameManager.Instance.incomeUpgradeCount + 1];
            incomeUpgradeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Cost: " + data.upgradeCost + " / " + "Income: " + data.income;
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
