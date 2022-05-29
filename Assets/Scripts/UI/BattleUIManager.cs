using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System.Linq;
using TMPro;

//public enum Phase //Phase ��� X GameManager State ����ϱ�
//{
//    Wait,  //���� ���
//    Start,  //���� ����
//    Wave1,  //���̺� 1
//    Wave2,  //���̺� 2
//    Wave3,   //���̺� 3
//    Between //���̺� ���̰���
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

    //fps ���� ����
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

        //�ڽ�Ʈ ȹ�淮�� �ڽ�Ʈ ȹ��
        RegenCost();

        if (GameManager.Instance.state == State.WAIT)
        {
            if (WaitingTime[(int)State.WAIT] >= 0) Active((int)State.WAIT);
        }
        if (GameManager.Instance.state == State.BATTLE)
        {
            //��� �гο� Ÿ�̸�UI���� ���̺�UI�� ����
            BattleTime();
            //������� ���
      //      if (isSoundCheck) audioSource.Play(); isSoundCheck = false;
            //�Ͻ������� ������� �Ͻ�����, �Ͻ����� ������ ������� ���
           // if (GameManager.Instance.gameSpeed == 0) audioSource.Pause(); else audioSource.UnPause();
            //��ŸƮ ������ ���ð���ŭ �˾�UI ��� �� ����
          //  if (WaitingTime[(int)State.BATTLE] >= 0) Active((int)State.BATTLE);
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

    /// <summary> ������ �˾�UI ���, ������ �ð� �� ���� </summary> <param name="index"></param>
    public void Active(int index) // Phase State�� �ٲٱ�
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

    /// <summary> �ڽ�Ʈ ���� (GameManager.Instance.earnedCost���� ����) </summary>
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

    /// <summary> ĳ���� ��ġ�� �ڽ�Ʈ �Ҹ� </summary>
    public void UseCost(int index)
    {
        if (MinionManager.Instance.minionPrefabs.Count <= index
            || GameManager.Instance.cost < MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().cost) return;
        GameManager.Instance.cost -= MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().cost;

        Debug.Log(MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().cost);
        costText.text = GameManager.Instance.cost.ToString();
    }

    /// <summary> �̴Ͼ� ��ġ </summary> <param name="index"></param>
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

    //GameManager SetGameSpeed �Լ� ���
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
        sellPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "�Ǹ� : " + cost;
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
            Debug.Log("�ڽ�Ʈ ����");
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

    //��ų �ƽ�
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
