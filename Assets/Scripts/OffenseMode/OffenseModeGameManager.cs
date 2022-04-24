using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class OffenseModeGameManager : Singleton<OffenseModeGameManager>
{
    public int cost = 20;

    public State state;

    public OffenceEnemySpawner spawner;

    bool tileSetMode = false;

    public Camera tileCamera;
    public Camera characterCamera;

    public float gameSpeed = 1;

    Node rayNode = new Node();

    Ray ray;

    public Vector3 heroSetPosition;

    public List<GameObject> enemiesList = new List<GameObject>();
    public List<GameObject> minionsList = new List<GameObject>();

    public int heroesListIndex = 0;

    public bool unitSetMode = false;

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
            if (tile.GetComponent<Tile>() == null)
            {
                tile.AddComponent<Tile>();
            }

            if (tile.GetComponent<BoxCollider>() == null)
            {
                tile.AddComponent<BoxCollider>();
            }

           tilesList.Add(tile.GetComponent<Tile>());
        }


        var result =tilesList.OrderByDescending(x => Mathf.Ceil(x.transform.position.x) / 100).ThenByDescending(x => x.transform.position.z); // 타일 위치 기준으로 리스트 정렬
        tilesList = result.ToList();

        int rowCount = 0;
        foreach (var tile in tilesList)
        {
            tile.GetComponent<Tile>().node.row = rowCount;
            rowCount++;
        }

        startTile = tilesList[0];
        endTile = tilesList[tilesList.Count - 1];

        StartCoroutine(spawner.Spawn());
    }

    void Update()
    {
        Time.timeScale = gameSpeed;

        ray = tileCamera.ScreenPointToRay(Input.mousePosition);

        ShowAttackRangeTiles();
    }

    public void ShowAttackRangeTiles()
    {
        if (unitSetMode)
        {
            Vector3 pos = unitSetTile.transform.position;
            pos += heroSetPosition;

            OffenceModeUIManager.Instance.isSettingCharacterOn = false;
            OffenceModeUIManager.Instance.settingCharacter.GetComponent<RectTransform>().anchoredPosition = characterCamera.WorldToScreenPoint(pos);

            /*
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
            */
            startTile.canOffenceUnitSetTile(MinionManager.Instance.heroPrefabs[heroesListIndex].GetComponent<Minion>().minionClass, false);

   

                GameObject hero = Instantiate(MinionManager.Instance.heroPrefabs[heroesListIndex]);
                hero.transform.position = pos;
                unitSetMode = false;
                tileSetMode = false;
                OffenceModeUIManager.Instance.settingCharacter.SetActive(false);
   
                OffenceModeUIManager.Instance.isSettingCharacterOn = true;
                /*
                hero.GetComponent<Unit>().SetDirection(direction);
                scale.x = Mathf.Abs(scale.x);
                BattleUIManager.Instance.settingCharacter.transform.localScale = scale;
                */
                minionsList.Add(hero);
                hero.SetActive(true);
            
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
       //         Debug.Log(raycastHit.collider.name);
                if (raycastHit.collider.GetComponent<Tile>() == startTile)
                {
  
                    if (Input.GetMouseButtonDown(0))
                    {
                        tileSetMode = false;
                        unitSetMode = true;
                        unitSetTile = raycastHit.collider.gameObject;
                        unitSetCameraPos = tileCamera.WorldToScreenPoint(raycastHit.collider.transform.position);
                    }

                    rayNode = raycastHit.collider.GetComponent<Tile>().node;
                }
            }
            else
            {
               // BattleUIManager.Instance.ShowAttackRangeTiles(false);
            }
        }
    }

    public void CanSetTile()
    {
        tileSetMode = true;


            startTile.canOffenceUnitSetTile(MinionManager.Instance.heroPrefabs[heroesListIndex].GetComponent<Minion>().minionClass, true);
        

    }
}

