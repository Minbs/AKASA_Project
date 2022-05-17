using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum OffenceModeDeployState
{
    None, // 배치할 유닛 선택x
    Positioning, // 배치 위치 결정
    Deployment // 미니언 배치
}

public class OffenceModeGameManager : Singleton<OffenceModeGameManager>
{
    public int cost = 20; // 초기 보유 코스트

    public State state;
    public OffenceModeDeployState deployState { get; set; } // 미니언 배치 상태

    public OffenceEnemySpawner spawner;

    public Camera tileCamera;
    public Camera characterCamera;

    public float gameSpeed = 1;

    Ray ray;

    public Vector3 minionSetPivot; // 배치 위치 조정 피봇

    public List<GameObject> enemiesList = new List<GameObject>(); // 씬에 있는 모든 적 리스트
    public List<GameObject> minionsList = new List<GameObject>();// 씬에 있는 모든 미니언 리스트

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

        var result = tilesList.OrderByDescending(x => Mathf.Ceil(x.transform.position.x) / 100).ThenByDescending(x => x.transform.position.z); // 타일 위치 기준으로 리스트 정렬
        tilesList = result.ToList();

        int rowCount = 0;
        foreach (var tile in tilesList)
        {
            tile.GetComponent<Tile>().node.row = rowCount;
            rowCount++;
        }

        startTile = tilesList[0]; //아군 소환 타일
        endTile = tilesList[tilesList.Count - 1]; // 적 소환 타일

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
    /// 미니언 배치 타일 결정
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
    /// 미니언 배치
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
    /// 버튼 클릭 콜백 이벤트 함수 미니언 배치 모드로 전환
    /// </summary>
    public void ChangeMinionPositioningState()
    {
        deployState = OffenceModeDeployState.Positioning;
        startTile.ShowOffenceModeDeployableTile(MinionManager.Instance.minionPrefabs[minionListIndex].GetComponent<Minion>().minionClass, true);
    }
}

