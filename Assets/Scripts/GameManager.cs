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

    public bool isLineOver = false;
    public bool isApproach = false;

    void Start()
    {
        state = State.WAIT;
        deployState = DeployState.None;
        currentWave = 1;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemies)
            enemiesList.Add(e);
    }

    void Update()
    {
        Time.timeScale = gameSpeed;

        ray = tileCamera.ScreenPointToRay(Input.mousePosition);

        if (waitTimer <= 0 && state.Equals(State.WAIT))
        {
            waitTimer = 0;
            state = State.BATTLE;

            foreach(var e in enemiesList)
            {
                e.GetComponent<UnitStateMachine>().ChangeState(e.GetComponent<UnitStateMachine>().moveState);
            }

            foreach (var tile in BoardManager.Instance.minionDeployTilesList)
            {
                tile.ShowDeployableTile(false);
            }

            BattleUIManager.Instance.settingCharacter.SetActive(false);
        }
        else if (waitTimer > 0 && state.Equals(State.WAIT))
        {
            waitTimer -= Time.deltaTime;
        }

        if(state.Equals(State.WAIT))
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
            if (raycastHit.collider.transform.tag == "Tile" && raycastHit.collider.GetComponent<Tile>().IsDeployableMinionTile())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    deployState = DeployState.SetDirection;
                    unitSetTile = raycastHit.collider.gameObject;
                    unitSetCameraPos = tileCamera.WorldToScreenPoint(raycastHit.collider.transform.position);

                    BattleUIManager.Instance.DeploymentMinion(BattleUIManager.Instance.mBtn[minionsListIndex].index);
                }

                rayNode = raycastHit.collider.GetComponent<Tile>().node;
            }
        }
        else
        {

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
        Direction direction = Direction.RIGHT;

        BattleUIManager.Instance.edge[minionsListIndex].SetActive(true);
        BattleUIManager.Instance.UseCost(BattleUIManager.Instance.mBtn[minionsListIndex].index);
        BattleUIManager.Instance.isCheck = true;

            GameObject minion = Instantiate(MinionManager.Instance.minionPrefabs[minionsListIndex]);
            minion.transform.position = pos;
            unitSetTile.GetComponent<Tile>().isOnUnit = true;
            deployState = DeployState.None;
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
    /// ��ư Ŭ�� �ݹ� �̺�Ʈ �Լ� ���ֹ�ġ���� ��ȯ
    /// </summary>
    public void ChangeMinionPositioningState()
    {
        deployState = DeployState.Positioning;

        foreach (var tile in BoardManager.Instance.minionDeployTilesList)
        {
            tile.ShowDeployableTile(true);
        }
    }
}

