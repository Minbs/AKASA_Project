using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System.Linq;
using TMPro;

public enum Phase
{
    Ready,  //전투 대비
    Start,  //전투 시작
    Wave1,  //웨이브 1
    Wave2,  //웨이브 2
    Wave3,   //웨이브 3
    Between //웨이브 사이간격
}

/// <summary>
/// </summary>
public class BattleUIManager : Singleton<BattleUIManager>
{
    public List<Node> attackRangeNodes = new List<Node>();
    public List<GameObject> attackRangeTileImages = new List<GameObject>();

    public GameObject attackRangeTileImage;
    public GameObject worldCanvas;

    public GameObject settingCharacter;
    public SkeletonDataAsset skeletonDataAsset;

    public bool isSettingCharacterOn = true;

    //
    //최대 코스트(상수)
    const int maxCost = 99;
    int[] maxMinionCount = { 3, 5 };
    List<GameObject> enemiesList = new List<GameObject>();

    ///text - 0:LimitTimeMin, 1:LimitTimeColon, 2:LimitTimeSec, 3:GameTargetCurrent, 4:GameTargetMax, 5:MinionAvailable
    public TextMeshProUGUI[] text;
    ///phase - 0:Ready, 1:Start, 2:Wave1, 3:Wave2, 4:Wave3
    public Image[] phase;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI costText;

    ///WaitingTime - 0:Ready, 1:Start, 2:Wave1, 3:Wave2, 4:Wave3, 5:Between
    [SerializeField]
    float[] WaitingTime;
    [SerializeField]
    int maxEnemyCount, waveCount, regenTime;

    float time, phaseWaitingTime;
    int min, sec, currentEnemyCount;
    bool isPhaseCheck;

    public GameObject minionBtnTranslucentBG;
    public List<GameObject> tBG = new List<GameObject>();
    public List<TextMeshProUGUI> wTime = new List<TextMeshProUGUI>();
    public bool isCheck = false;
    bool isSoundCheck = true;

    //대기열 패널
    public GameObject wBG;
    //대기열 미니언 버튼
    public List<MinionButton> mBtn;

    AudioSource audioSource;

    void Start()
    {
        ObjectPool.Instance.CreatePoolObject("AttackRangeTile", attackRangeTileImage, 20, worldCanvas.transform);

        //초기화
        Init();
    }

    void Update()
    {
        if (isSettingCharacterOn)
            SetSettingCharacterMousePosition();

        //
        //전투 대비 시간일 때
        if (GameManager.Instance.state == State.WAIT)
        {
            if (WaitingTime[(int)Phase.Ready] >= 0) Active((int)Phase.Ready);
            WaitTime();
        }
        //전투 시작 시간일 때
        if (GameManager.Instance.waitTimer <= 0 && GameManager.Instance.state == State.BATTLE)
        {
            //상단 패널에 타이머UI에서 웨이브UI로 변경
            BattleTime();
            //에너미 카운트수 체크
            EnemeyCount();
            //배경음악 재생
            if (isSoundCheck) audioSource.Play(); isSoundCheck = false;
            //일시정지시 배경음악 일시중지, 일시정지 해제시 배경음악 재생
            if (GameManager.Instance.gameSpeed == 0) audioSource.Pause(); 
            else audioSource.UnPause();
            //스타트 페이즈 대기시간만큼 팝업UI 출력 후 해제
            if (WaitingTime[(int)Phase.Start] >= 0) Active((int)Phase.Start);
            //웨이브1 페이즈 대기시간만큼 팝업UI 출력 후 해제
            if (WaitingTime[(int)Phase.Wave1] >= 0) Active((int)Phase.Wave1);
            //정해진 시간(regenTime)마다 코스트 1씩 추가
            RegenCost();
            //UnitCount();
        }
        //
    }

    public void SetSettingCharacterMousePosition()
    {
        settingCharacter.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    }

    public void RemoveAttackRangeTiles()
    {
        if (attackRangeTileImages.Count > 0)
        {

            foreach (var r in attackRangeTileImages)
            {
                ObjectPool.Instance.PushToPool("AttackRangeTile", r, worldCanvas.transform);

            }

            attackRangeTileImages.Clear();

        }
    }

    public void ShowAttackRangeTiles(bool isActive, Tile tile = null, Direction direction = Direction.LEFT)
    {
        if (isActive)
        {
            RemoveAttackRangeTiles();

            List<Node> temp; //= new List<Node>();
            temp = GetAttackRangeNodesList(direction);

            foreach (var t in temp)
            {
                Tile tile1 = BoardManager.Instance.GetTile(t + tile.node);

                if (tile1 != null)
                {
                    Vector3 pos = tile1.gameObject.transform.position;

                    GameObject attackTile = ObjectPool.Instance.PopFromPool("AttackRangeTile");
                    Vector3 size = tile1.gameObject.GetComponent<Renderer>().bounds.size;
                    pos.y += size.y + 0.005f;
                    attackTile.GetComponent<RectTransform>().position = pos;
                    attackTile.SetActive(true);
                    attackRangeTileImages.Add(attackTile);
                }
            }
        }
        else
        {
            RemoveAttackRangeTiles();

        }
    }

    public List<Node> GetAttackRangeNodesList(Direction direction)
    {
        List<Node> tiles = attackRangeNodes.ToList();

        int max = tiles.Count;
        for (int count = 0; count < max; count++)
        {
            switch (direction)
            {
                case Direction.LEFT:
                    tiles[count] = new Node(tiles[count].row, tiles[count].column);
                    break;
                case Direction.UP:
                    tiles[count] = new Node(tiles[count].column, tiles[count].row);
                    break;
                case Direction.RIGHT:
                    tiles[count] = new Node(tiles[count].row * -1, tiles[count].column);
                    break;
                case Direction.DOWN:
                    tiles[count] = new Node(tiles[count].column, tiles[count].row * -1);
                    break;
            }
        }

        return tiles;
    }

    //
    void Init()
    {
        time = 0;
        audioSource = gameObject.GetComponent<AudioSource>();
        enemiesList = GameManager.Instance.enemiesList;
        costText.text = GameManager.Instance.cost.ToString();

        //미니언 버튼
        for (int i = 0; i < wBG.transform.childCount; i++)
            mBtn.Add(wBG.GetComponentsInChildren<MinionButton>()[i]);

        for (int i = 0; i < 12; i++)
        {
            tBG.Add(minionBtnTranslucentBG.transform.GetChild(i).gameObject);
            wTime.Add(tBG[i].GetComponentInChildren<TextMeshProUGUI>());
            if (tBG[i].activeSelf) tBG[i].SetActive(false);
        }

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

    /// <summary> 페이즈 팝업UI 출력, 지정된 시간 후 해제 </summary> <param name="index"></param>
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

    /// <summary> 코스트 리젠 (regenTime마다 실행) </summary>
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

    /// <summary> 캐릭터 배치후 코스트 소모 </summary>
    public void UseCost(int index)
    {
        if (MinionManager.Instance.heroPrefabs.Count <= index) return;
        GameManager.Instance.cost -= MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().cost;
        costText.text = GameManager.Instance.cost.ToString();
    }

    /// <summary> 미니언 배치 </summary> <param name="index"></param>
    public void DeploymentMinion(int index)
    {
        if (GameManager.Instance.cost >= 0)
        {
            if (GameManager.Instance.cost >= MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().cost)
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
        GameManager.Instance.gameSpeed == 1 ? GameManager.Instance.gameSpeed = 2 : GameManager.Instance.gameSpeed = 1;

    public void OnPauseButton() => GameManager.Instance.gameSpeed =
        GameManager.Instance.gameSpeed == 0 ? GameManager.Instance.gameSpeed = 1 : GameManager.Instance.gameSpeed = 0;

    IEnumerator PhaseDelay()
    {
        yield return new WaitForSeconds(WaitingTime[(int)Phase.Between]);
        isPhaseCheck = true;
    }
}
