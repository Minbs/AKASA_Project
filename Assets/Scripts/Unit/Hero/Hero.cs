using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spine.Unity;

public class Hero : Unit
{
    // Start is called before the first frame update
    public List<Node> attackRangeNodes = new List<Node>();
    public List<Tile> attackRangeTiles = new List<Tile>();
    public SkeletonDataAsset skeletonData;

    public GameObject target;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        AimTarget();
        AttackTarget();
    }

    public void AimTarget()
    {
        if(target == null)
        {
          


            foreach (var enemy in GameManager.Instance.enemiesList)
            {
                foreach (var t in attackRangeTiles)
                {

                    if(t.onTile(enemy.transform))
                    {
                        target = enemy;
                        Debug.Log("Æ÷Âø");
                        return;
                    }
                }
            }
        }
        else
        {
            bool isOut = true;
                foreach (var t in attackRangeTiles)
                {

                    if (t.onTile(target.transform))
                    {
                    isOut = false;
                    }
                }

            if (isOut)
                target = null;
            }
        
    }

    public void AttackTarget()
    {
        if(target != null && transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != "shoot")
        {
            transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "shoot", false);
        }

        if (target == null && transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != "idle")
        {
            transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "idle", true);
        }
    }

    public List<Node> GetAttackRangeNodesList(Direction direction)
    {
        List<Node> tiles = attackRangeNodes.ToList();

        int x = 1;
        int y = 1;

        int max = tiles.Count;
        for(int count = 0; count < max; count++)
        {
            switch(direction)
            {
                case Direction.LEFT:
                    tiles[count] = new Node(tiles[count].row  , tiles[count].column);
                    break;
                case Direction.UP:
                    tiles[count] = new Node(tiles[count].column, tiles[count].row);
                    break;
                case Direction.RIGHT:
                    tiles[count] = new Node(tiles[count].row * -1, tiles[count].column);
                    break;
                case Direction.DOWN:
                    tiles[count] = new Node(tiles[count].column, tiles[count].row * -1);
                    break;
            }
        }

        return tiles;
    }
}
