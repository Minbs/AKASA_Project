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

public class GameManager : Singleton<GameManager>
{
    public int cost = 20; // 초기 보유 코스트

    public float waitTime = 30; // 대기 시간
    public float clearTimeTerm = 30;
    public float currentWaitTimer { get; set; }
    

    public State state;
    public DeployState deployState { get; set; } // 배치 상태

    public EnemySpawner spawner;

    public Camera tileCamera;
    public Camera characterCamera;

    public float gameSpeed { get; set; }

    public int currentWave { get; set; }

//    Node rayNode = new Node();

    Ray ray;

    public Vector3 minionSetPosition;

    public List<GameObject> enemiesList = new List<GameObject>();
    public List<GameObject> minionsList = new List<GameObject>();

    public int minionsListIndex = 0;

    private GameObject unitSetTile;

    private Vector3 unitSetCameraPos;

    public bool isLineOver = false;
    public bool isApproach = false;

    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemies)
            enemiesList.Add(e);

        currentWave = 0;
        StartCoroutine(WaitState());
    }

    void Update()
    {
        ray = tileCamera.ScreenPointToRay(Input.mousePosition);
 
    }

    IEnumerator WaitState()
    {
        state = State.WAIT;
        deployState = DeployState.NONE;
        SetGameSpeed(1);
        currentWaitTimer = waitTime;
        currentWave++;
        isLineOver = false;
        isApproach = false;


        while (currentWaitTimer > 0)
        {
            StartCoroutine(spawner.Spawn(currentWave));
            currentWaitTimer -= Time.deltaTime;
            WaitStateUpdate();
            yield return null;
        }

        deployState = DeployState.NONE;
        BattleUIManager.Instance.settingCharacter.SetActive(false);
        
        StartCoroutine(BattleState());
    }

    void WaitStateUpdate()
    {
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
        if (isApproach == false)
        {
            foreach (var m in minionsList)
            {
                if (isLineOver)
                    m.GetComponent<UnitStateMachine>().ChangeState(m.GetComponent<UnitStateMachine>().moveState);
            }

            if (isLineOver)
                isApproach = true;
        }


    }


    IEnumerator WaveEndState()
    {
        state = State.WAVE_END;
        SetGameSpeed(1);

        foreach (var m in minionsList)
        {
            if (m.activeSelf)
                m.GetComponent<UnitStateMachine>().ChangeState(m.GetComponent<UnitStateMachine>().moveState);
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

        Debug.Log("waveEndStateEnd");
        /*
foreach (var m in minionsList)
{
m.GetComponent<UnitStateMachine>().Initialize();
m.GetComponent<Unit>().SetPositionOnTile();
m.SetActive(true);
}
*/

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
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.collider.transform.tag == "Tile" && raycastHit.collider.GetComponent<Tile>().IsDeployableMinionTile())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    deployState = DeployState.Deploying;
                    unitSetTile = raycastHit.collider.gameObject;
                    unitSetCameraPos = tileCamera.WorldToScreenPoint(raycastHit.collider.transform.position);

                    BattleUIManager.Instance.DeploymentMinion(BattleUIManager.Instance.mBtn[minionsListIndex].index);
                }
            }
        }
        else
        {

        }
    }

    /// <summary>
    /// 유닛 배치 방향 결정
    /// </summary>
    public void DeployingMinion()
    {
        Vector3 pos = unitSetTile.transform.position;
        pos += minionSetPosition;

        BattleUIManager.Instance.isSettingCharacterOn = false;
        BattleUIManager.Instance.settingCharacter.GetComponent<RectTransform>().anchoredPosition = characterCamera.WorldToScreenPoint(pos);
        Direction direction = Direction.RIGHT;

        BattleUIManager.Instance.edge[minionsListIndex].SetActive(true);
        BattleUIManager.Instance.UseCost(BattleUIManager.Instance.mBtn[minionsListIndex].index);
        BattleUIManager.Instance.isCheck = true;

            GameObject minion = Instantiate(MinionManager.Instance.minionPrefabs[minionsListIndex], MinionManager.Instance.transform);
            minion.transform.position = pos;
            unitSetTile.GetComponent<Tile>().isOnUnit = true;
            minion.GetComponent<DefenceMinion>().onTile = unitSetTile.GetComponent<Tile>();
            deployState = DeployState.NONE;
            BattleUIManager.Instance.settingCharacter.SetActive(false);
            minion.GetComponent<Unit>().SetDirection(direction);

            foreach (var tile in BoardManager.Instance.minionDeployTilesList)
            {
                tile.ShowDeployableTile(false);
            }

            unitSetTile = null;
            minionsList.Add(minion);
            minion.SetActive(true);
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

    public void SetGameSpeed(float speed)
    {
        gameSpeed = speed;

        foreach (var e in enemiesList)
        {
            e.GetComponent<Unit>().spineAnimation.skeletonAnimation.AnimationState.TimeScale = gameSpeed;
        }

        foreach (var m in minionsList)
        {
            m.GetComponent<Unit>().spineAnimation.skeletonAnimation.AnimationState.TimeScale = gameSpeed;
        }
    }
}

