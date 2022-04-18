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

public class GameManager : Singleton<GameManager>
{
    public int cost = 20;

    public float waitTimer = 20; // ��� �ð�

    public float battleTime = 180; //���� �ð�

    public State state;

    public EnemySpawner spawner;

    bool tileSetMode = false;

    public Camera tileCamera;
    public Camera characterCamera;

    public float gameSpeed = 1;



    Node rayNode = new Node();

    Ray ray;

  //  public GameObject hero;

    public Vector3 heroSetPosition;

    public List<GameObject> enemiesList = new List<GameObject>();
    public List<GameObject> minionsList = new List<GameObject>();

    public int heroesListIndex = 0;

    public bool unitSetMode = false;

    private GameObject unitSetTile;

    private Vector3 unitSetCameraPos;

    void Start()
    {
        state = State.WAIT;
    }

    void Update()
    {
        Time.timeScale = gameSpeed;

        ray = tileCamera.ScreenPointToRay(Input.mousePosition);

        if (waitTimer <= 0 && state == State.WAIT)
        {
            state = State.BATTLE;
            StartCoroutine(spawner.Spawn());
        }
        else
        {
            waitTimer -= Time.deltaTime;
        }

        if (tileSetMode)
        {
            ShowAttackRangeTiles();
        }

        if(battleTime <= 0 && state == State.BATTLE)
        {
            battleTime -= Time.deltaTime;
        }
    }

    public void ShowAttackRangeTiles()
    {



        if (unitSetMode)
        {
            Vector3 pos = unitSetTile.transform.position;
            pos += heroSetPosition;
          
            BattleUIManager.Instance.isSettingCharacterOn = false;
            BattleUIManager.Instance.settingCharacter.GetComponent<RectTransform>().anchoredPosition = characterCamera.WorldToScreenPoint(pos);
            //    Debug.Log("mouse : " + Input.mousePosition.normalized + ", tile : " + unitSetCameraPos.normalized);
            Vector2 vec = Input.mousePosition - unitSetCameraPos;
            
            float dot = Vector2.Dot(vec.normalized,new Vector2(0, 1)); //�յ� �Ǻ�
            Vector3 cross = Vector3.Cross(vec.normalized, new Vector2(0,1)); //�¿� �Ǻ�

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
                GameObject hero = Instantiate(MinionManager.Instance.heroPrefabs[heroesListIndex]);
                hero.transform.position = pos;
                unitSetTile.GetComponent<Tile>().isOnUnit = true;
                unitSetMode = false;
                tileSetMode = false;
                BattleUIManager.Instance.settingCharacter.SetActive(false);
                BattleUIManager.Instance.isSettingCharacterOn = true;
                hero.GetComponent<Unit>().SetDirection(direction);
              //  hero.GetComponent<Minion>().onTile.no
                scale.x = Mathf.Abs(scale.x);
                BattleUIManager.Instance.settingCharacter.transform.localScale = scale;
                foreach (var tile in temp)
                {
                    if (BoardManager.Instance.GetTile(unitSetTile.GetComponent<Tile>().node + tile) != null)
                        hero.GetComponent<Minion>().attackRangeTiles.Add(BoardManager.Instance.GetTile(unitSetTile.GetComponent<Tile>().node + tile));
                }

                foreach (var tile in BoardManager.Instance.tilesList)
                {
                    tile.canUnitSetTile(hero.GetComponent<Minion>().minionClass, false);
                }
                BattleUIManager.Instance.ShowAttackRangeTiles(false);
                unitSetTile = null;
                minionsList.Add(hero);
                hero.SetActive(true);
            }
          

        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {


                if (raycastHit.collider.transform.tag == "Tile" && !unitSetMode)
                {
                    if (Input.GetMouseButtonDown(0) && raycastHit.collider.GetComponent<Tile>().IsCanSetUnit(MinionManager.Instance.heroPrefabs[heroesListIndex].GetComponent<Minion>().minionClass))
                    {
                        //  tileSetMode = false;
                        unitSetMode = true;
                        unitSetTile = raycastHit.collider.gameObject;
                        unitSetCameraPos = tileCamera.WorldToScreenPoint(raycastHit.collider.transform.position);
                    }




                    if (rayNode != raycastHit.collider.GetComponent<Tile>().node && raycastHit.collider.GetComponent<Tile>().IsCanSetUnit(MinionManager.Instance.heroPrefabs[heroesListIndex].GetComponent<Minion>().minionClass))
                    {
                        BattleUIManager.Instance.ShowAttackRangeTiles(true, raycastHit.collider.GetComponent<Tile>());
                    }
                    else if (!raycastHit.collider.GetComponent<Tile>().IsCanSetUnit(MinionManager.Instance.heroPrefabs[heroesListIndex].GetComponent<Minion>().minionClass))
                    {
                        BattleUIManager.Instance.ShowAttackRangeTiles(false);
                    }
                }


                rayNode = raycastHit.collider.GetComponent<Tile>().node;
            }
            else
            {
                BattleUIManager.Instance.ShowAttackRangeTiles(false);
            }
        }






    }

    public void CanSetTile()
    {
            tileSetMode = true;

        foreach (var tile in BoardManager.Instance.tilesList)
        {
            tile.canUnitSetTile(MinionManager.Instance.heroPrefabs[heroesListIndex].GetComponent<Minion>().minionClass, true);
        }

    }
}

