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
    END //전투 종료
}

public enum DeployState
{
    None,
    Positioning, // 배치 위치 결정
    SetDirection // 배치 방향 결정
}

public class GameManager : Singleton<GameManager>
{
    public int cost = 20; // 초기 보유 코스트

    public float waitTimer = 20; // 대기 시간

    public float battleTime = 180; //전투 시간

    public State state;
    public DeployState deployState { get; set; } // 배치 상태

    public EnemySpawner spawner;

    public Camera tileCamera;
    public Camera characterCamera;

    public float gameSpeed = 1;

    public int currentWave { get; set; }

    Node rayNode = new Node();

    Ray ray;

    public Vector3 heroSetPosition;

    public List<GameObject> enemiesList = new List<GameObject>();
    public List<GameObject> minionsList = new List<GameObject>();

    public int minionsListIndex = 0;

    private GameObject unitSetTile;

    private Vector3 unitSetCameraPos;

    void Start()
    {
        state = State.WAIT;
        deployState = DeployState.None;
        currentWave = 1;
    }

    void Update()
    {
        Time.timeScale = gameSpeed;

        ray = tileCamera.ScreenPointToRay(Input.mousePosition);

        if (waitTimer <= 0 && state.Equals(State.WAIT))
        {
            state = State.BATTLE;
            StartCoroutine(spawner.Spawn());
        }
        else
        {
            waitTimer -= Time.deltaTime;
        }

        switch(deployState)
        {
            case DeployState.Positioning:
                PositioningMinion();
                break;
            case DeployState.SetDirection:
                SetMinionDirection();
                break;
            default:
                break;
        }



        if (battleTime > 0 && state.Equals(State.BATTLE))
        {
            battleTime -= Time.deltaTime;
        }
    }


 

    /// <summary>
    /// 유닛 배치 타일 결정
    /// </summary>
    private void PositioningMinion()
    {
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.collider.transform.tag == "Tile")
            {
                if (Input.GetMouseButtonDown(0) && raycastHit.collider.GetComponent<Tile>().IsDeployableMinionTile(MinionManager.Instance.minionPrefabs[minionsListIndex].GetComponent<DefenceMinion>().minionClass))
                {
                    deployState = DeployState.SetDirection;
                    unitSetTile = raycastHit.collider.gameObject;
                    unitSetCameraPos = tileCamera.WorldToScreenPoint(raycastHit.collider.transform.position);
                }
                if (rayNode != raycastHit.collider.GetComponent<Tile>().node && raycastHit.collider.GetComponent<Tile>().IsDeployableMinionTile(MinionManager.Instance.minionPrefabs[minionsListIndex].GetComponent<DefenceMinion>().minionClass))
                {
                    BattleUIManager.Instance.ShowAttackRangeTiles(true, raycastHit.collider.GetComponent<Tile>());
                }
                else if (!raycastHit.collider.GetComponent<Tile>().IsDeployableMinionTile(MinionManager.Instance.minionPrefabs[minionsListIndex].GetComponent<DefenceMinion>().minionClass))
                {
                    BattleUIManager.Instance.ShowAttackRangeTiles(false);
                }

                rayNode = raycastHit.collider.GetComponent<Tile>().node;
            }
        }
        else
        {
            BattleUIManager.Instance.ShowAttackRangeTiles(false);
        }
    }

    /// <summary>
    /// 유닛 배치 방향 결정
    /// </summary>
    public void SetMinionDirection()
    {
        Vector3 pos = unitSetTile.transform.position;
        pos += heroSetPosition;

        BattleUIManager.Instance.isSettingCharacterOn = false;
        BattleUIManager.Instance.settingCharacter.GetComponent<RectTransform>().anchoredPosition = characterCamera.WorldToScreenPoint(pos);
        Vector2 vec = Input.mousePosition - unitSetCameraPos;

        float dot = Vector2.Dot(vec.normalized, new Vector2(0, 1)); //앞뒤 판별
        Vector3 cross = Vector3.Cross(vec.normalized, new Vector2(0, 1)); //좌우 판별

        List<Node> temp = new List<Node>();
        Direction direction = Direction.LEFT;

        Vector3 scale = BattleUIManager.Instance.settingCharacter.transform.localScale;
        if (dot > 0 && cross.z < 0.5f && cross.z > -0.5f)
        {
            temp = BattleUIManager.Instance.GetAttackRangeNodesList(Direction.UP).ToList();
            direction = Direction.UP;
            BattleUIManager.Instance.ShowAttackRangeTiles(true, unitSetTile.GetComponent<Tile>(), direction);
        }
        if (dot < 0 && cross.z < 0.5f && cross.z > -0.5f)
        {
            temp = BattleUIManager.Instance.GetAttackRangeNodesList(Direction.DOWN).ToList();
            direction = Direction.DOWN;
            BattleUIManager.Instance.ShowAttackRangeTiles(true, unitSetTile.GetComponent<Tile>(), direction);
        }
        if (cross.z > 0 && dot < 0.5f && dot > -0.5f)
        {
            temp = BattleUIManager.Instance.GetAttackRangeNodesList(Direction.RIGHT).ToList();
            direction = Direction.RIGHT;
            BattleUIManager.Instance.ShowAttackRangeTiles(true, unitSetTile.GetComponent<Tile>(), direction);

            scale.x = -Mathf.Abs(scale.x);
            BattleUIManager.Instance.settingCharacter.transform.localScale = scale;
        }
        if (cross.z < 0 && dot < 0.5f && dot > -0.5f)
        {
            temp = BattleUIManager.Instance.GetAttackRangeNodesList(Direction.LEFT).ToList();
            direction = Direction.LEFT;
            BattleUIManager.Instance.ShowAttackRangeTiles(true, unitSetTile.GetComponent<Tile>(), direction);

            scale.x = Mathf.Abs(scale.x);
            BattleUIManager.Instance.settingCharacter.transform.localScale = scale;
        }

        if (Input.GetMouseButtonDown(0))
        {
            MinionManager.Instance.minionPrefabs[BattleUIManager.Instance.mBtn[minionsListIndex].index]
                .GetComponent<DefenceMinion>().minionStandbyTime =
                MinionManager.Instance.minionPrefabs[BattleUIManager.Instance.mBtn[minionsListIndex].index]
                .GetComponent<DefenceMinion>().minionWaitingTime;
            BattleUIManager.Instance.DeploymentMinion(BattleUIManager.Instance.mBtn[minionsListIndex].index);
            BattleUIManager.Instance.isCostCheck = true;

            GameObject minion = Instantiate(MinionManager.Instance.minionPrefabs[minionsListIndex]);
            minion.transform.position = pos;
            unitSetTile.GetComponent<Tile>().isOnUnit = true;
            deployState = DeployState.None;
            BattleUIManager.Instance.settingCharacter.SetActive(false);
            BattleUIManager.Instance.isSettingCharacterOn = true;
            minion.GetComponent<Unit>().SetDirection(direction);
            //  hero.GetComponent<Minion>().onTile.no
            scale.x = Mathf.Abs(scale.x);
            BattleUIManager.Instance.settingCharacter.transform.localScale = scale;
            foreach (var tile in temp)
            {
                if (BoardManager.Instance.GetTile(unitSetTile.GetComponent<Tile>().node + tile) != null)
                    minion.GetComponent<DefenceMinion>().attackRangeTiles.Add(BoardManager.Instance.GetTile(unitSetTile.GetComponent<Tile>().node + tile));
            }

            foreach (var tile in BoardManager.Instance.tilesList)
            {
                tile.ShowDeployableTile(false);
            }
            BattleUIManager.Instance.ShowAttackRangeTiles(false);
            unitSetTile = null;
            minionsList.Add(minion);
            minion.SetActive(true);
        }


    }

    /// <summary>
    /// 버튼 클릭 콜백 이벤트 함수 유닛배치모드로 전환
    /// </summary>
    public void ChangeMinionPositioningState()
    {
        deployState = DeployState.Positioning;

        foreach (var tile in BoardManager.Instance.tilesList)
        {
            tile.ShowDeployableTile(true, MinionManager.Instance.minionPrefabs[minionsListIndex].GetComponent<DefenceMinion>().minionClass);
        }
    }
}

