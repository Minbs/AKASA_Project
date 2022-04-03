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

public class GameManager : Singleton<GameManager>
{
    public int cost = 20;

    public float waitTimer = 20; // 대기 시간

    public float battleTime = 180; //전투 시간

    public State state;

    public EnemySpawner spawner;

    bool tileSetMode = false;

    public Camera tileCamera;
    public Camera characterCamera;



    Node rayNode = new Node();

    Ray ray;

  //  public GameObject hero;

    public Vector3 heroSetPosition;

    public List<GameObject> enemiesList = new List<GameObject>();

    public int heroesListIndex = 0;

    void Start()
    {
        state = State.WAIT;
    }

    void Update()
    {
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
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.collider.transform.tag == "Tile")
            {
                if (Input.GetMouseButtonDown(0) && raycastHit.collider.GetComponent<Tile>().IsCanSetUnit())
                {
                    tileSetMode = false;


                    Vector3 pos = raycastHit.collider.transform.position;
                    raycastHit.collider.GetComponent<Tile>().isOnUnit = true;
                    pos += heroSetPosition;

                    GameObject hero = Instantiate(HeroManager.Instance.heroPrefabs[heroesListIndex]);
                    hero.transform.position = pos;
                    tileSetMode = false;
                    UIManager.Instance.settingCharacter.SetActive(false);
                    

                    foreach (var tile in hero.GetComponent<Hero>().GetAttackRangeNodesList(Direction.LEFT))
                    {
                        if(BoardManager.Instance.GetTile(raycastHit.collider.GetComponent<Tile>().node + tile) != null)
                        hero.GetComponent<Hero>().attackRangeTiles.Add(BoardManager.Instance.GetTile(  raycastHit.collider.GetComponent<Tile>().node + tile));
                    }

                    foreach (var tile in BoardManager.Instance.tilesList)
                    {
                        tile.canUnitSetTile(tileSetMode);
                    }
                    UIManager.Instance.ShowAttackRangeTiles(false);
                    hero.SetActive(true);
                }


                if (rayNode != raycastHit.collider.GetComponent<Tile>().node && raycastHit.collider.GetComponent<Tile>().IsCanSetUnit())
                {
                    UIManager.Instance.ShowAttackRangeTiles(true, raycastHit.collider.GetComponent<Tile>());
                }
                else if (!raycastHit.collider.GetComponent<Tile>().IsCanSetUnit())
                {
                    UIManager.Instance.ShowAttackRangeTiles(false);
                }
            }
           

            rayNode = raycastHit.collider.GetComponent<Tile>().node;
        }
        else
        {
            UIManager.Instance.ShowAttackRangeTiles(false);
        }
    }

    public void CanSetTile()
    {
        if (tileSetMode)
            tileSetMode = false;
        else
            tileSetMode = true;

        foreach (var tile in BoardManager.Instance.tilesList)
        {
            tile.canUnitSetTile(tileSetMode);
        }

    }
}

