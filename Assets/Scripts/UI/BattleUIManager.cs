using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System.Linq;
using TMPro;

public enum Phase
{
    Ready,  //���� ���
    Start,  //���� ����
    Wave1,  //���̺� 1
    Wave2,  //���̺� 2
    Wave3,   //���̺� 3
    Between //���̺� ���̰���
}

/// <summary>
/// </summary>
public class BattleUIManager : Singleton<BattleUIManager>
{
    public GameObject worldCanvas;

    public GameObject DeployableTileImage;
    public Sprite NotDeployableTileSprite;

    public GameObject settingCharacter;
    public SkeletonDataAsset skeletonDataAsset;

    public bool isSettingCharacterOn = true;

    //
    const int maxCost = 99;
    int[] maxMinionCount = { 3, 5 };
    List<GameObject> enemiesList = new List<GameObject>();

    ///text - 0:LimitTimeMin, 1:LimitTimeColon, 2:LimitTimeSec, 3:GameTargetCurrent, 4:GameTargetMax, 5:MinionAvailable
    public TextMeshProUGUI[] text;
    ///phase - 0:Ready, 1:Start, 2:Wave1, 3:Wave2, 4:Wave3
    public Image[] phase;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI costText;

    ///WaitingTime - 0:Ready, 1:Start, 2:Wave1, 3:Wave2, 4:Wave3, 5:Bett
    [SerializeField]
    float[] WaitingTime;
    [SerializeField]
    int maxEnemyCount = 3, waveCount = 1, regenTime = 3;

    float time = 0, phaseWaitingTime;
    int min, sec, currentEnemyCount = 0;
    bool isPhaseCheck;

    public GameObject mBG;
    public List<GameObject> tBG;
    public List<GameObject> edge;
    public bool isCheck = false;
    bool isSoundCheck = true;

    public GameObject wBG;
    public List<MinionButton> mBtn;

    AudioSource audioSource;

    private float fpsDeltaTime = 0;

    [SerializeField, Range(1, 100)]
    private int fpsFontSize = 25;

    [SerializeField]
    private Color fpsColor = Color.green;
    private int fpsIndex = 0;
    public bool isFpsShow;


    void Start()
    {
        Init();
    }

    void Update()
    {
       if (settingCharacter.activeSelf)
           SetSettingCharacterMousePosition();

        FPS();

       //
        if (GameManager.Instance.state == State.WAIT)
       {
           if (WaitingTime[(int)Phase.Ready] >= 0) Active((int)Phase.Ready);
           WaitTime();
       }
       if (GameManager.Instance.waitTimer <= 0 && GameManager.Instance.state == State.BATTLE)
       {
            //��� �гο� Ÿ�̸�UI���� ���̺�UI�� ����
            BattleTime();
            //���ʹ� ī��Ʈ�� üũ
            EnemeyCount();
            //������� ���
            if (isSoundCheck) audioSource.Play(); isSoundCheck = false;
            //�Ͻ������� ������� �Ͻ�����, �Ͻ����� ������ ������� ���
            if (GameManager.Instance.gameSpeed == 0) audioSource.Pause();
            else audioSource.UnPause();
            //��ŸƮ ������ ���ð���ŭ �˾�UI ��� �� ����
            if (WaitingTime[(int)Phase.Start] >= 0) Active((int)Phase.Start);
            //���̺�1 ������ ���ð���ŭ �˾�UI ��� �� ����
            if (WaitingTime[(int)Phase.Wave1] >= 0) Active((int)Phase.Wave1);

            //UnitCount();
        }
       else 
       {

       }
       
    }

    public void SetSettingCharacterMousePosition()
    {
        settingCharacter.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    }


    void Init()
    {
        time = 0;
        audioSource = gameObject.GetComponent<AudioSource>();
        enemiesList = GameManager.Instance.enemiesList;
        costText.text = GameManager.Instance.cost.ToString();

        //
        for (int i = 0; i < 12; i++)
        {
            tBG.Add(mBG.transform.GetChild(i).gameObject);
            edge.Add(tBG[i].transform.GetChild(0).gameObject);
            if (tBG[i].activeSelf) tBG[i].SetActive(false);
            if (edge[i].activeSelf) edge[i].SetActive(false);
        }

        //�̴Ͼ� ��ư
        for (int i = 0; i < wBG.transform.childCount; i++)
            mBtn.Add(wBG.GetComponentsInChildren<MinionButton>()[i]);

        for (int i = 0; i < 3; i++)
            if (text[i].gameObject.activeSelf) text[i].gameObject.SetActive(false);
        text[3].text = currentEnemyCount.ToString();
        text[4].text = maxEnemyCount.ToString();
        text[5].text = maxMinionCount[0].ToString();

        for (int i = 0; i < 5; i++)
            if (phase[i].gameObject.activeSelf) phase[i].gameObject.SetActive(false);

        if (wave.gameObject.activeSelf) wave.gameObject.SetActive(false);
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
        for (int i = 0; i < 3; i++) text[i].gameObject.SetActive(true);

        float time = GameManager.Instance.waitTimer;
        min = (int)time / 60;
        sec = ((int)time - min * 60) % 60;

        if (min <= 0 && sec <= 0)
        {
            text[0].text = 0.ToString();
            text[2].text = 0.ToString();
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

    void UnitCount()
    {

    }

    /// <summary> ������ �˾�UI ���, ������ �ð� �� ���� </summary> <param name="index"></param>
    void Active(int index)
    {
        switch (index)
        {
            case (int)Phase.Ready:
                WaitingTime[(int)Phase.Ready] -= Time.deltaTime;
                phaseWaitingTime = WaitingTime[(int)Phase.Ready];
                isPhaseCheck = false;
                break;
            case (int)Phase.Start:
                WaitingTime[(int)Phase.Start] -= Time.deltaTime;
                phaseWaitingTime = WaitingTime[(int)Phase.Start];
                isPhaseCheck = false;
                break;
            case (int)Phase.Wave1:
                StartCoroutine("PhaseDelay");
                if (isPhaseCheck)
                {
                    WaitingTime[(int)Phase.Wave1] -= Time.deltaTime;
                    phaseWaitingTime = WaitingTime[(int)Phase.Wave1];
                    isPhaseCheck = false;
                }
                else
                    return;
                break;
            case (int)Phase.Wave2:
                StartCoroutine("PhaseDelay");
                if (isPhaseCheck)
                {
                    WaitingTime[(int)Phase.Wave2] -= Time.deltaTime;
                    phaseWaitingTime = WaitingTime[(int)Phase.Wave2];
                    isPhaseCheck = false;
                }
                else
                    return;
                break;
            case (int)Phase.Wave3:
                StartCoroutine("PhaseDelay");
                if (isPhaseCheck)
                {
                    WaitingTime[(int)Phase.Wave3] -= Time.deltaTime;
                    phaseWaitingTime = WaitingTime[(int)Phase.Wave3];
                    isPhaseCheck = false;
                }
                else
                    return;
                break;
            default:
                break;
        }

        if (phaseWaitingTime >= 0) phase[index].gameObject.SetActive(true);
        else phase[index].gameObject.SetActive(false);
    }

    /// <summary> �ڽ�Ʈ ���� (regenTime���� ����) </summary>
    void RegenCost()
    {
        time += Time.deltaTime;

        if (time >= regenTime)
        {
            if (GameManager.Instance.cost >= maxCost)
            {
                costText.text = GameManager.Instance.cost.ToString() + '+'.ToString();
                return;
            }

            GameManager.Instance.cost++;
            costText.text = GameManager.Instance.cost.ToString();
            time = 0;
        }
    }

    /// <summary> ĳ���� ��ġ�� �ڽ�Ʈ �Ҹ� </summary>
    public void UseCost(int index)
    {
        if (MinionManager.Instance.minionPrefabs.Count <= index) return;
        GameManager.Instance.cost -= MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().cost;
        costText.text = GameManager.Instance.cost.ToString();
    }

    /// <summary> �̴Ͼ� ��ġ </summary> <param name="index"></param>
    public void DeploymentMinion(int index)
    {
        if (GameManager.Instance.cost >= 0)
        {
            if (GameManager.Instance.cost >= MinionManager.Instance.minionPrefabs[index].GetComponent<DefenceMinion>().cost)
            {
                if (!tBG[index].activeSelf)
                {
                    mBtn[index].MBtnTBGPosition();
                    tBG[index].SetActive(true);
                }
            }
        }
        else
        {
            return;
        }
    }

    public void OnDoubleSpeedButton() => GameManager.Instance.gameSpeed =
            GameManager.Instance.gameSpeed == 1 || GameManager.Instance.gameSpeed == 0 ?
            GameManager.Instance.gameSpeed = 2 : GameManager.Instance.gameSpeed = 1;

    public void OnPauseButton() => GameManager.Instance.gameSpeed =
        GameManager.Instance.gameSpeed == 0 ? GameManager.Instance.gameSpeed = 1 : GameManager.Instance.gameSpeed = 0;


    IEnumerator PhaseDelay()
    {
        yield return new WaitForSeconds(WaitingTime[(int)Phase.Between]);
        isPhaseCheck = true;
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
