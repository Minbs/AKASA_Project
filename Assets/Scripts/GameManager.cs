using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public enum State
{
    WAIT, //전투 시작 전
    BATTLE, //전투 진행
    SKILL_PERFORM, //스킬 시전
    WAVE_END,
    END //전투 종료
}

public enum DeployState
{
    NONE,
    POSITIONING, // 배치 위치 결정
    Deploying // 배치 방향 결정
}

[Serializable]
public struct IncomeUpgradeData
{
    public int upgradeCost;
    public int income;
}

public class GameManager : Singleton<GameManager>
{
    public float cost = 20; // 초기 보유 코스트
    public float costTime = 10; // 초기 코스트 획득량

    public float waitTime = 30; // 대기 시간
    public float clearTimeTerm = 30;
    public float currentWaitTimer { get; set; }

    public State state { get; set; }
    public DeployState deployState { get; set; } // 배치 상태

    public EnemySpawner spawner;

    public Camera tileCamera;
    public Camera characterCamera;

    public float gameSpeed { get; set; }

    public int currentWave { get; set; }

    public List<int> waveClearRewards;

    Ray ray;

    public Vector3 minionSetPosition;

    public List<GameObject> enemiesList = new List<GameObject>();
    public List<GameObject> minionsList = new List<GameObject>();

    public int minionsListIndex { get; set; }

    private GameObject unitSetTile;

    public GameObject settingCharacter { get; set; }

    public bool isChangePosition { get; set; }

    public int totalIncome { get; set; }
    public List<IncomeUpgradeData> incomeUpgradeDatas;
    public int incomeUpgradeCount { get; set; }

    public GameObject turret;

    void Start()
    {
        //Time.timeScale = 2;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemies)
            enemiesList.Add(e);

        currentWave = 0;
        totalIncome = incomeUpgradeDatas[0].income;
        incomeUpgradeCount = 0;
        isChangePosition = false;
        StartCoroutine(WaitState());
    }

    void Update()
    {
        ray = tileCamera.ScreenPointToRay(Input.mousePosition);

        if (!settingCharacter)
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                if (raycastHit.collider.transform.tag == "Tower"
                    && Input.GetMouseButtonUp(1))
                {
                    BattleUIManager.Instance.SetIncomeUpgradeButtonActive(true);
                }
                else if ((raycastHit.collider.transform.tag != "Tower"
                    && !raycastHit.collider.gameObject.Equals(BattleUIManager.Instance.upgradeSell.incomeUpgradeButton))
                    && (Input.GetMouseButtonUp(1)
                    || Input.GetMouseButtonUp(0)))
                {
                    BattleUIManager.Instance.SetIncomeUpgradeButtonActive(false);
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(1)
                    || Input.GetMouseButtonUp(0))
                {
                    BattleUIManager.Instance.SetIncomeUpgradeButtonActive(false);
                }
            }
        }
    }

    IEnumerator WaitState()
    {
        state = State.WAIT;
        deployState = DeployState.NONE;
        SetGameSpeed(1);
        currentWaitTimer = waitTime;
        currentWave++;



        while (currentWaitTimer > 0)
        {
            StartCoroutine(spawner.Spawn(currentWave));
            currentWaitTimer -= Time.deltaTime;
            WaitStateUpdate();
            yield return null;
        }

        deployState = DeployState.NONE;




        if (settingCharacter)
        {
            if (isChangePosition)
            {
                Vector3 pos = settingCharacter.GetComponent<DefenceMinion>().onTile.gameObject.transform.position;
                pos += minionSetPosition;
                settingCharacter.transform.position = pos;
                settingCharacter.GetComponent<DefenceMinion>().onTile.isOnUnit = true;
                BattleUIManager.Instance.upgradeSell.sellPanel.SetActive(false);
            }
            else
            {
                Destroy(settingCharacter);
            }
        }

        BattleUIManager.Instance.upgradeSell.minionUpgradeUI.SetActive(false);
        settingCharacter = null;
        StartCoroutine(BattleState());
    }

    void WaitStateUpdate()
    {
        if(BattleUIManager.Instance.upgradeSell.minionUpgradeUI.activeSelf)
        {
                if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Object")) )
                {
                if (!raycastHit.collider.transform.parent.GetComponent<Minion>()
                    && Input.GetMouseButtonUp(1))
                    BattleUIManager.Instance.upgradeSell.minionUpgradeUI.SetActive(false);
                }
               else
            {
                if (Input.GetMouseButtonUp(1))
                {
                    BattleUIManager.Instance.upgradeSell.minionUpgradeUI.SetActive(false);
                }
            }
        }

        switch (deployState)
        {
            case DeployState.POSITIONING:
                PositioningMinion();
                break;
            case DeployState.Deploying:
                DeployingMinion();
                break;
            default:
                break;
        }
    }

    IEnumerator BattleState()
    {
        state = State.BATTLE;
        SetGameSpeed(1);

        //1. 스킬 타입 2. 이름 순 오름차순 정렬
        var tempList = minionsList.OrderBy(x => x.GetComponent<DefenceMinion>().skillType).ThenBy(x => x.GetComponent<DefenceMinion>().Unitname);

        minionsList = tempList.ToList();

    //    minionsList

        foreach (var e in enemiesList)
        {
            e.GetComponent<UnitStateMachine>().ChangeState(e.GetComponent<UnitStateMachine>().moveState);
        }

        foreach (var tile in BoardManager.Instance.minionDeployTilesList)
        {
            tile.ShowDeployableTile(false);
        }



        while (enemiesList.Count > 0)
        {
            BattleStateUpdate();
            yield return null;
        }

        StartCoroutine(WaveEndState());
    }

    void BattleStateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SkillManager.Instance.UseCharacterSkill(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SkillManager.Instance.UseCharacterSkill(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SkillManager.Instance.UseCharacterSkill(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SkillManager.Instance.UseCharacterSkill(3);
        }
    }


    IEnumerator WaveEndState()
    {
        state = State.WAVE_END;
        SetGameSpeed(1);

        foreach (var m in minionsList)
        {
            if (m.activeSelf)
            {
                m.GetComponent<UnitStateMachine>().ChangeState(m.GetComponent<UnitStateMachine>().moveState);
                m.GetComponent<Unit>().currentHp = m.GetComponent<Unit>().maxHp;
                m.GetComponent<Unit>().UpdateHealthbar();
                m.GetComponent<Unit>().target = null;
            }
        }

        /**
        //웨이브 클리어 후 간격

        float timer = clearTimeTerm;

        while (timer > 0)
        {
            WaveEndStateUpdate();
            timer -= Time.deltaTime;
            yield return null;
        }
        */

        while (true)
        {
            WaveEndStateUpdate();

            bool isAllMinionReturn = true;
            foreach (var m in minionsList)
            {
                if (!m.GetComponent<UnitStateMachine>().currentState.Equals(m.GetComponent<UnitStateMachine>().idleState) && m.activeSelf)
                    isAllMinionReturn = false;
            }

            if (isAllMinionReturn)
                break;

            yield return null;
        }

        cost += waveClearRewards[currentWave - 1];
        BattleUIManager.Instance.cost.costText.text = cost.ToString();

        StartCoroutine(WaitState());
    }

    void WaveEndStateUpdate()
    {

    }
    /// <summary>
    /// 유닛 배치 타일 결정
    /// </summary>
    private void PositioningMinion()
    {
        BattleUIManager.Instance.upgradeSell.minionUpgradeUI.SetActive(false);

        foreach (var minion in minionsList)
        {
            if (minion.Equals(settingCharacter))
                continue;

            minion.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            minion.GetComponent<UnitStateMachine>().agent.enabled = false;
        }

        if (settingCharacter)
        {
            Vector3 mousePosition
           = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 22);


            settingCharacter.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.collider.transform.tag == "Tile" && raycastHit.collider.GetComponent<Tile>().IsDeployableMinionTile())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    deployState = DeployState.Deploying;
                    unitSetTile = raycastHit.collider.gameObject;
                }
            }
            else if (raycastHit.collider.gameObject.Equals(BattleUIManager.Instance.upgradeSell.sellPanel) && isChangePosition)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    cost += settingCharacter.GetComponent<DefenceMinion>().sellCost;
                    BattleUIManager.Instance.cost.costText.text = cost.ToString();
                    minionsList.Remove(settingCharacter);
                    Destroy(settingCharacter);
                    settingCharacter = null;
                    isChangePosition = false;
                    BattleUIManager.Instance.upgradeSell.sellPanel.SetActive(false);
                    deployState = DeployState.NONE;
                }
            }
        }
        else
        {

        }
    }

    /// <summary>
    /// 유닛 배치
    /// </summary>
    public void DeployingMinion()
    {
        Vector3 pos = unitSetTile.transform.position;
        pos += minionSetPosition;

        Direction direction = Direction.RIGHT;

        //BattleUIManager.Instance.isCheck = true;


        settingCharacter.transform.position = pos;
        unitSetTile.GetComponent<Tile>().isOnUnit = true;
        settingCharacter.GetComponent<DefenceMinion>().onTile = unitSetTile.GetComponent<Tile>();
        deployState = DeployState.NONE;
        settingCharacter.GetComponent<Unit>().SetDirection(direction);

        foreach (var tile in BoardManager.Instance.minionDeployTilesList)
        {
            tile.ShowDeployableTile(false);
        }

        settingCharacter.GetComponent<Unit>().Init();
        unitSetTile = null;

        if (!isChangePosition)
        {
            minionsList.Add(settingCharacter);
        }


        settingCharacter.GetComponent<UnitStateMachine>().isDeploying = false;
        settingCharacter = null;
        isChangePosition = false;
        BattleUIManager.Instance.upgradeSell.sellPanel.SetActive(false);




        foreach (var m in minionsList)
        {
            SynergyManager.Instance.CheckClassSynergy(m);
            m.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
            m.GetComponent<UnitStateMachine>().agent.enabled = true;
        }

    }

    /// <summary>
    /// 버튼 클릭 콜백 이벤트 함수 유닛배치모드로 전환
    /// </summary>
    public void ChangeMinionPositioningState()
    {
        deployState = DeployState.POSITIONING;

        foreach (var tile in BoardManager.Instance.minionDeployTilesList)
        {
            tile.ShowDeployableTile(true);
        }
    }

    public void minionChangePos(GameObject minion)
    {
        if (settingCharacter)
            return;

        isChangePosition = true;
        settingCharacter = minion;
        settingCharacter.GetComponent<Unit>().healthBar.transform.parent.gameObject.SetActive(false);
        settingCharacter.GetComponent<Unit>().onTile.isOnUnit = false;
        settingCharacter.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;

        BattleUIManager.Instance.SetSellCostText(settingCharacter.GetComponent<DefenceMinion>().sellCost);
        BattleUIManager.Instance.upgradeSell.sellPanel.SetActive(true);
        deployState = DeployState.POSITIONING;

        foreach (var tile in BoardManager.Instance.minionDeployTilesList)
        {
            tile.ShowDeployableTile(true);
        }
    }


    public void SetGameSpeed(float speed)
    {
        if (gameSpeed.Equals(speed))
            return;

        gameSpeed = speed;

        foreach (var e in enemiesList)
        {
            e.GetComponent<Unit>().spineAnimation.skeletonAnimation.AnimationState.TimeScale = gameSpeed;
            e.GetComponent<UnitStateMachine>().agent.velocity = e.GetComponent<UnitStateMachine>().agent.velocity * speed;
        }

        foreach (var m in minionsList)
        {
            m.GetComponent<Unit>().spineAnimation.skeletonAnimation.AnimationState.TimeScale = gameSpeed;
            m.GetComponent<UnitStateMachine>().agent.velocity = m.GetComponent<UnitStateMachine>().agent.velocity * speed;
        }
    }

}

