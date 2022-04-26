using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public enum State
{
    WAIT, //���� ���� ��
    BATTLE, //���� ����
    END //���� ����
}

public enum DeployState
{
    None,
    Positioning, // ��ġ ��ġ ����
    SetDirection // ��ġ ���� ����
}

public class GameManager : Singleton<GameManager>
{
    public int cost = 20; // �ʱ� ���� �ڽ�Ʈ

    public float waitTimer = 20; // ��� �ð�

    public float battleTime = 180; //���� �ð�

    public State state;
    public DeployState deployState { get; set; } // ��ġ ����

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
    /// ���� ��ġ Ÿ�� ����
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
    /// ���� ��ġ ���� ����
    /// </summary>
    public void SetMinionDirection()
    {
        Vector3 pos = unitSetTile.transform.position;
        pos += heroSetPosition;

        BattleUIManager.Instance.isSettingCharacterOn = false;
        BattleUIManager.Instance.settingCharacter.GetComponent<RectTransform>().anchoredPosition = characterCamera.WorldToScreenPoint(pos);
        Vector2 vec = Input.mousePosition - unitSetCameraPos;

        float dot = Vector2.Dot(vec.normalized, new Vector2(0, 1)); //�յ� �Ǻ�
        Vector3 cross = Vector3.Cross(vec.normalized, new Vector2(0, 1)); //�¿� �Ǻ�

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
            BattleUIManager.Instance.UseCost(BattleUIManager.Instance.mBtn[minionsListIndex].index);

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
    /// ��ư Ŭ�� �ݹ� �̺�Ʈ �Լ� ���ֹ�ġ���� ��ȯ
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

