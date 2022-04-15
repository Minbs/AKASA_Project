using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System.Linq;
using TMPro;

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
    const int maxCost = 99;
    const int maxMinionCount = 12;
    List<GameObject> enemiesList = new List<GameObject>();

    public TextMeshProUGUI[] text;
    public Image[] phase;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI costText;

    [SerializeField]
    float readyWaitingTime = 1.0f, battleWaitingTime = 1.0f;
    [SerializeField]
    int maxEnemyCount = 3, waveCount = 1, regenTime = 3;

    float time = 0, waitingTime;
    int min, sec, currentEnemyCount = 0;
    //int index = 0;

    public GameObject minionBtnTranslucentBG;
    public List<GameObject> tBG = new List<GameObject>();
    List<TextMeshProUGUI> wTime = new List<TextMeshProUGUI>();
    public bool isCheck = false;
    public float times;
    //

    private void Awake()
    {
        for (int i = 0; i < maxMinionCount; i++)
        {
            tBG.Add(minionBtnTranslucentBG.transform.GetChild(i).gameObject);
            wTime.Add(tBG[i].GetComponentInChildren<TextMeshProUGUI>());

            if (tBG[i].activeSelf)
                tBG[i].SetActive(false);
        }
        
        for (int i = 0; i < 3; i++)
            if (text[i].gameObject.activeSelf)
                text[i].gameObject.SetActive(false);
        for (int i = 0; i < 2; i++)
            if (phase[i].gameObject.activeSelf)
                phase[i].gameObject.SetActive(false);
        if (wave.gameObject.activeSelf)
            wave.gameObject.SetActive(false);
    }

    void Start()
    {
        ObjectPool.Instance.CreatePoolObject("AttackRangeTile", attackRangeTileImage, 20, worldCanvas.transform);

        text[3].text = currentEnemyCount.ToString();
        text[4].text = maxEnemyCount.ToString();
        enemiesList = GameManager.Instance.enemiesList;

        time = 0;
        costText.text = GameManager.Instance.cost.ToString();
    }

    void Update()
    {
        if (isSettingCharacterOn)
            SetSettingCharacterMousePosition();

        //
        if (GameManager.Instance.waitTimer <= 0 && GameManager.Instance.state == State.BATTLE)
        {
            Active(1);
            BattleTime();
            RegenCost();
            EnemeyCount();

            if (isCheck == true)
                MinionWaitTimer(MinionButton.Instance.index);

            //UnitCount();
        }
        else if (GameManager.Instance.state == State.WAIT)
        {
            Active(0);
            WaitTime();
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
    void BattleTime()
    {
        for (int i = 0; i < 3; i++)
            text[i].gameObject.SetActive(false);

        wave.gameObject.SetActive(true);
        wave.text = "Wave ".ToString() + waveCount.ToString();
    }

    void WaitTime()
    {
        for (int i = 0; i < 3; i++)
            text[i].gameObject.SetActive(true);

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
        if (currentEnemyCount >= 0)
            text[3].text = currentEnemyCount.ToString();
    }

    void UnitCount()
    {

    }

    /// <summary>
    /// 전투 대비, 전투 시작 팝업UI 출력, 지정된 시간 후 해제 (전투 대비 : 0, 전투 시작 : 1)
    /// </summary>
    /// <param name="index"></param>
    void Active(int index)
    {
        if (index == 0)
        {
            readyWaitingTime -= Time.deltaTime;
            waitingTime = readyWaitingTime;
        }
        else if (index == 1)
        {
            battleWaitingTime -= Time.deltaTime;
            waitingTime = battleWaitingTime;
        }
        else
            return;

        if (waitingTime >= 0)
            phase[index].gameObject.SetActive(true);
        else
            phase[index].gameObject.SetActive(false);
    }

    /// <summary>
    /// 코스트 리젠 (regenTime마다 실행)
    /// </summary>
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

    /// <summary>
    /// 캐릭터 배치후 코스트 소모
    /// </summary>
    public void UseCost(int index)
    {
        if (MinionManager.Instance.heroPrefabs.Count <= index)
            return;

        GameManager.Instance.cost -= MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().cost;
        costText.text = GameManager.Instance.cost.ToString();
    }

    public void DeploymentMinion(int index)
    {
        if (GameManager.Instance.cost >= 0)
        {
            if (GameManager.Instance.cost >= MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().cost)
            {
                UseCost(index);
                MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().minionStandbyTime =
                    MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().minionWaitingTime;
                isCheck = true;
                //waitTimer(index);           
            }
            else if (GameManager.Instance.cost < MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().cost)
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    public float MinionStanbyTimer(int index)
    {
        MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().minionStandbyTime -= Time.deltaTime;

        if (MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().minionStandbyTime <= 0)
        {
            isCheck = false;

            if (tBG[index].activeSelf)
                tBG[index].SetActive(false);
        }

        Debug.Log(MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().minionStandbyTime);
        return MinionManager.Instance.heroPrefabs[index].GetComponent<Minion>().minionStandbyTime;
    }

    public void MinionWaitTimer(int index)
    {
        Debug.Log("check");

        if (!tBG[index].activeSelf)
        {
            tBG[index].SetActive(true);
        }

        MinionButton.Instance.MBtnTBGPosition();

        wTime[index].text = MinionStanbyTimer(index).ToString("F1") + "s".ToString();

        if (MinionStanbyTimer(index) <= 0 && isCheck == true)
        {
            tBG[index].SetActive(false);
            isCheck = false;
        }
    }
    //
}
