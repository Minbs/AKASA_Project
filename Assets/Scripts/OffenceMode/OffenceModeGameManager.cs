using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum OffenceModeDeployState
{
    None, // ��ġ�� ���� ����x
    Positioning, // ��ġ ��ġ ����
    Deployment // �̴Ͼ� ��ġ
}

public class OffenceModeGameManager : Singleton<OffenceModeGameManager>
{
    public int cost = 20; // �ʱ� ���� �ڽ�Ʈ

    public State state;
    public OffenceModeDeployState deployState { get; set; } // �̴Ͼ� ��ġ ����

    public OffenceEnemySpawner spawner;

    public Camera tileCamera;
    public Camera characterCamera;

    public float gameSpeed = 1;

    Ray ray;

    public Vector3 minionSetPivot; // ��ġ ��ġ ���� �Ǻ�

    public List<GameObject> enemiesList = new List<GameObject>(); // ���� �ִ� ��� �� ����Ʈ
    public List<GameObject> minionsList = new List<GameObject>();// ���� �ִ� ��� �̴Ͼ� ����Ʈ

    public int minionListIndex = 0;

    private GameObject unitSetTile;

    private Vector3 unitSetCameraPos;

    public Tile startTile;
    public Tile endTile;

    [SerializeField]
    public List<Tile> tilesList = new List<Tile>();

    public int currentWave = 1;

    void Start()
    {
        state = State.WAIT;
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");


        foreach (var tile in tiles)
        {
            if (tile.GetComponent<Tile>() == null) tile.AddComponent<Tile>();
            if (tile.GetComponent<BoxCollider>() == null) tile.AddComponent<BoxCollider>();
            tilesList.Add(tile.GetComponent<Tile>());
        }

        var result = tilesList.OrderByDescending(x => Mathf.Ceil(x.transform.position.x) / 100).ThenByDescending(x => x.transform.position.z); // Ÿ�� ��ġ �������� ����Ʈ ����
        tilesList = result.ToList();

        int rowCount = 0;
        foreach (var tile in tilesList)
        {
            tile.GetComponent<Tile>().node.row = rowCount;
            rowCount++;
        }

        startTile = tilesList[0]; //�Ʊ� ��ȯ Ÿ��
        endTile = tilesList[tilesList.Count - 1]; // �� ��ȯ Ÿ��

        StartCoroutine(spawner.Spawn());
    }

    void Update()
    {
        Time.timeScale = gameSpeed;

        ray = tileCamera.ScreenPointToRay(Input.mousePosition);

        switch (deployState)
        {
            case OffenceModeDeployState.Positioning:
                PositioningMinion();
                break;
            case OffenceModeDeployState.Deployment:
                DeployMinion();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// �̴Ͼ� ��ġ Ÿ�� ����
    /// </summary>
    public void PositioningMinion()
    {
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (MinionManager.Instance.minionPrefabs[minionListIndex] != null && raycastHit.collider.GetComponent<Tile>() != null)
            {
                if (Input.GetMouseButtonDown(0) && raycastHit.collider.GetComponent<Tile>().Equals(startTile))
                {
                    unitSetTile = raycastHit.collider.gameObject;
                    unitSetCameraPos = tileCamera.WorldToScreenPoint(raycastHit.collider.transform.position);
                    deployState = OffenceModeDeployState.Deployment;
                }
            }
        }
    }

    /// <summary>
    /// �̴Ͼ� ��ġ
    /// </summary>
    public void DeployMinion()
    {
        Vector3 pos = unitSetTile.transform.position;
        pos += minionSetPivot;
        OffenceModeUIManager.Instance.isSettingCharacterOn = false;
        OffenceModeUIManager.Instance.settingCharacter.GetComponent<RectTransform>().anchoredPosition = characterCamera.WorldToScreenPoint(pos);
        startTile.ShowOffenceModeDeployableTile(MinionManager.Instance.minionPrefabs[minionListIndex].GetComponent<Minion>().minionClass, false);
        GameObject minion = Instantiate(MinionManager.Instance.minionPrefabs[minionListIndex]);
        OffenceModeUIManager.Instance.settingCharacter.SetActive(false);
        OffenceModeUIManager.Instance.isSettingCharacterOn = true;
        minion.transform.position = pos;
        minionsList.Add(minion);
        minion.SetActive(true);
        deployState = OffenceModeDeployState.None;
    }

    /// <summary>
    /// ��ư Ŭ�� �ݹ� �̺�Ʈ �Լ� �̴Ͼ� ��ġ ���� ��ȯ
    /// </summary>
    public void ChangeMinionPositioningState()
    {
        deployState = OffenceModeDeployState.Positioning;
        startTile.ShowOffenceModeDeployableTile(MinionManager.Instance.minionPrefabs[minionListIndex].GetComponent<Minion>().minionClass, true);
    }
}

