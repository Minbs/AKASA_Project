using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public State state;

    public EnemySpawner spawner;

    bool tileSetMode = false;

    public Camera tileCamera;
    public Camera characterCamera;

    public List<Node> attackRangeTiles = new List<Node>();
    public List<GameObject> attackRangeTileImages = new List<GameObject>();

    Node rayNode = new Node();

    Ray ray;

    public GameObject hero1;

    void Start()
    {
        state = State.WAIT;

        Node node = new Node();
        node = new Node(0, 0);
        attackRangeTiles.Add(node);

        node = new Node(-1, 0);
        attackRangeTiles.Add(node);

        node = new Node(-2, 0);
        attackRangeTiles.Add(node);

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
    }

    public void ShowAttackRangeTiles()
    {
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.collider.transform.tag == "Tile")
            {
                if (Input.GetMouseButtonDown(0) && raycastHit.collider.GetComponent<Tile>().IsCanSetUnit())
                {
                    Debug.Log("h");
                    tileSetMode = false;
                    if (attackRangeTileImages.Count > 0)
                    {

                        foreach (var r in attackRangeTileImages)
                        {
                            ObjectPool.Instance.PushToPool("AttackRangeTile", r);

                        }

                        attackRangeTileImages.Clear();

                    }

                    Vector3 pos = raycastHit.collider.transform.position;
                    pos.y += 0.16f;
                    pos.z += 0.2f;
                    hero1.transform.position = pos;
                    hero1.SetActive(true);
                }


                if (rayNode != raycastHit.collider.GetComponent<Tile>().node && raycastHit.collider.GetComponent<Tile>().IsCanSetUnit())
                {
                    if (attackRangeTileImages.Count > 0)
                    {

                        foreach (var r in attackRangeTileImages)
                        {
                            ObjectPool.Instance.PushToPool("AttackRangeTile", r);

                        }

                        attackRangeTileImages.Clear();
                    }

                    foreach (var t in attackRangeTiles)
                    {

                        Tile tile = BoardManager.Instance.GetTile(t + raycastHit.collider.GetComponent<Tile>().node);

                        if (tile != null)
                        {
                            Vector3 pos = tile.gameObject.transform.position;

                            GameObject attackTile = ObjectPool.Instance.PopFromPool("AttackRangeTile");
                            pos.y += 0.151f;
                            attackTile.transform.position = pos;
                            attackTile.SetActive(true);
                            attackRangeTileImages.Add(attackTile);
                        }

                    }



                 
                }
                else if(!raycastHit.collider.GetComponent<Tile>().IsCanSetUnit())
                {

                if (attackRangeTileImages.Count > 0)
                {

                    foreach (var r in attackRangeTileImages)
                    {
                        ObjectPool.Instance.PushToPool("AttackRangeTile", r);

                    }

                    attackRangeTileImages.Clear();
                }
                }

                rayNode = raycastHit.collider.GetComponent<Tile>().node;
            }
            else
            {
                if (attackRangeTileImages.Count > 0)
                {

                    foreach (var r in attackRangeTileImages)
                    {
                        ObjectPool.Instance.PushToPool("AttackRangeTile", r);

                    }

                    attackRangeTileImages.Clear();
                }
            }    
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
