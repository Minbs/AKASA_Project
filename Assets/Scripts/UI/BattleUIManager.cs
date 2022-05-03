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

    public GameObject settingCharacter;
    public SkeletonDataAsset skeletonDataAsset;

    public bool isSettingCharacterOn = true;

    //
    //�ִ� �ڽ�Ʈ(���)
    const int maxCost = 99;
    int[] maxMinionCount = { 3, 5 };
    List<GameObject> enemiesList = new List<GameObject>();

    //�ؽ�ƮUI - 0:LimitTimeMin, 1:LimitTimeColon, 2:LimitTimeSec, 3:GameTargetCurrent, 4:GameTargetMax, 5:MinionAvailable
    public TextMeshProUGUI[] text;
    //������UI - 0:Ready, 1:Start, 2:Wave1, 3:Wave2, 4:Wave3
    public Image[] phase;
    //���̺�UI
    public TextMeshProUGUI wave;
    //�ڽ�Ʈ �� UI
    public TextMeshProUGUI costText;

    //WaitingTime - 0:Ready, 1:Start, 2:Wave1, 3:Wave2, 4:Wave3, 5:Between
    [SerializeField]
    float[] WaitingTime;
    [SerializeField]
    int maxEnemyCount, waveCount, regenTime;

    float time, phaseWaitingTime;
    int min, sec, currentEnemyCount;
    bool isPhaseCheck;

    //M_Btn_TranslucentBG ������Ʈ �ʿ�
    public GameObject minionBtnTranslucentBG;
    //TimeEdge ������Ʈ �ʿ�
    public List<GameObject> tBG = new List<GameObject>();
    //WaitTime ������Ʈ �ʿ�
    public List<TextMeshProUGUI> wTime = new List<TextMeshProUGUI>();
    //�̴Ͼ� ���ġ Ÿ�̸� ����Ȯ��
    public bool isCheck = false;
    //���� ����Ȯ��(���� ���� ���� false)
    bool isSoundCheck = true;

    //��⿭ �г�
    public GameObject wBG;
    //��⿭ �̴Ͼ� ��ư
    public List<MinionButton> mBtn;

    //����
    AudioSource audioSource;

    void Start()
    {
<<<<<<< HEAD
=======
        ObjectPool.Instance.CreatePoolObject("AttackRangeTile", attackRangeTileImage, 20, worldCanvas.transform);

        //�ʱ�ȭ
>>>>>>> e94892ada7b254ce6c1b80656754faa6baa3e7ee
        Init();
    }

    void Update()
    {
<<<<<<< HEAD
       if (settingCharacter.activeSelf)
           SetSettingCharacterMousePosition();

       //
       if (GameManager.Instance.state == State.WAIT)
       {
           if (WaitingTime[(int)Phase.Ready] >= 0)
               Active((int)Phase.Ready);
           WaitTime();
       }
       if (GameManager.Instance.waitTimer <= 0 && GameManager.Instance.state == State.BATTLE)
       {
           if (isSoundCheck == true)
           {
               audioSource.Play();
               isSoundCheck = false;
           }

           if (WaitingTime[(int)Phase.Start] >= 0)
           {
               Active((int)Phase.Start);
           }

           if (WaitingTime[(int)Phase.Wave1] >= 0)
           {
               StartCoroutine("PhaseDelay");
               Active((int)Phase.Wave1);
           }

           BattleTime();
           RegenCost();
           EnemeyCount();

           //UnitCount();
       }
       else 
       {

       }
       
=======
        if (isSettingCharacterOn)
            SetSettingCharacterMousePosition();

        //
        //���� ��� �ð��� ��
        if (GameManager.Instance.state == State.WAIT)
        {
            if (WaitingTime[(int)Phase.Ready] >= 0) Active((int)Phase.Ready);
            WaitTime();
        }
        //���� ���� �ð��� ��
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
            //������ �ð�(regenTime)���� �ڽ�Ʈ 1�� �߰�
            RegenCost();
            //UnitCount();
        }
        //
>>>>>>> e94892ada7b254ce6c1b80656754faa6baa3e7ee
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
            tBG.Add(minionBtnTranslucentBG.transform.GetChild(i).gameObject);
            wTime.Add(tBG[i].GetComponentInChildren<TextMeshProUGUI>());
            if (tBG[i].activeSelf) tBG[i].SetActive(false);
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
                    isCheck = true;
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
}
