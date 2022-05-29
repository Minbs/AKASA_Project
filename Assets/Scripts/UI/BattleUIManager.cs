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
   


    //phase - 0:Wait, 1:Start, 2:Wave1, 3:Wave2, 4:Wave3
    public Image[] phase;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI costEarnedText;

    [SerializeField]
    float[] WaitingTime;
    [SerializeField]
    int maxEnemyCount = 3, waveCount = 1;

    float time = 0, phaseWaitingTime;
    int min, sec, currentEnemyCount = 0;
    bool isPhaseCheck;

    public bool isCheck = false;
    bool isSoundCheck = true;

    bool isDeployBtnCheck = true;
    bool isSkillBtnCheck = true;

    AudioSource audioSource;

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

    public GameObject minionUpgradeUI;

    public GameObject SkillCutSceneObject;
    public GameObject SkillCutSceneImage;
    public GameObject SkillCutSceneIllust;

    Vector3 illustStartPos;
    void Start()
    {
        incomeUpgradeButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(IncomeUpgrade);
        illustStartPos = SkillCutSceneIllust.GetComponent<RectTransform>().position;
        Init();
    }

    private void FixedUpdate()
    {
      
        //
    }

    void Update()
    {
      
        FPS();

        //코스트 획득량당 코스트 획득
        RegenCost();

        if (GameManager.Instance.state == State.WAIT)
        {
            if (WaitingTime[(int)State.WAIT] >= 0) Active((int)State.WAIT);
        }
        if (GameManager.Instance.state == State.BATTLE)
        {
            //상단 패널에 타이머UI에서 웨이브UI로 변경
            BattleTime();
        }

    }

    void Init()
    {

    }

    void BattleTime()
    {
        wave.gameObject.SetActive(true);
        wave.text = "Wave ".ToString() + waveCount.ToString();
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

            }
        }
        else
        {
            return;
        }
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

    public void skill()
    {
        StartCoroutine(ActiveCutScene(1));
    }

    public void SetMinionUpgradeUI(GameObject minion)
    {
        minionUpgradeUI.SetActive(true);
        minionUpgradeUI.GetComponent<RectTransform>().anchoredPosition3D = minion.transform.position;
    }

    //스킬 컷신
    public IEnumerator ActiveCutScene(float duration)
    {
        SkillCutSceneObject.SetActive(true);
        float timer = 0;



        Vector3 startPos = new Vector3(-362, 81, 0);
        Vector3 startRot = new Vector3(0, 0, 35);

        Vector3 endPos = new Vector3(-362, 300, 0);
        Vector3 endRot = new Vector3(0, 0, 9);

        SkillCutSceneImage.GetComponent<RectTransform>().localPosition = startPos;
        SkillCutSceneImage.GetComponent<RectTransform>().localEulerAngles = startRot;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;

            SkillCutSceneImage.GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPos, endPos, timer / duration);
            SkillCutSceneImage.GetComponent<RectTransform>().localEulerAngles = Vector3.Lerp(startRot, endRot, timer / duration);

            SkillCutSceneIllust.GetComponent<RectTransform>().position = illustStartPos;
            SkillCutSceneIllust.GetComponent<RectTransform>().localEulerAngles = new Vector3(SkillCutSceneImage.GetComponent<RectTransform>().localEulerAngles.x, SkillCutSceneImage.GetComponent<RectTransform>().localEulerAngles.y, -SkillCutSceneImage.GetComponent<RectTransform>().localEulerAngles.z);
        }

        SkillCutSceneImage.GetComponent<RectTransform>().localPosition = endPos;
        SkillCutSceneImage.GetComponent<RectTransform>().localEulerAngles = endRot;

        float timer2 = 0;
        float duration2 = 1;
        while (timer2 < duration2)
        {
            timer2 += Time.deltaTime;
            yield return null;
        }

        SkillCutSceneObject.SetActive(false);
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
