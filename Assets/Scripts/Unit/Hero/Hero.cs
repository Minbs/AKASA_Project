using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spine.Unity;
using Spine;
using Event = Spine.Event;

public class Hero : Unit
{
    // Start is called before the first frame update
    public List<Node> attackRangeNodes = new List<Node>();
    public List<Tile> attackRangeTiles = new List<Tile>();



    

    private float attackTimer = 0;
    public float attackSpeed;



    protected override void Start()
    {
        base.Start();
        attackTimer = attackSpeed;
        transform.GetChild(0).GetComponent<SkeletonAnimation>().state.Event += AnimationSatateOnEvent;
    }

    private void AnimationSatateOnEvent(TrackEntry trackEntry, Event e)
    {
        if (e.Data.Name == "shoot")
        {
            Deal();
        }
    }

    public void Deal()
    {
        target.GetComponent<Unit>().hp -= 10;
    }


    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        AimTarget();
        AttackTarget();
    }

    public void AimTarget()
    {
        Spine.TrackEntry trackEntry = new Spine.TrackEntry();
        trackEntry = spineAnimation.skeletonAnimation.AnimationState.Tracks.ElementAt(0);
        float normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd;




        if (target == null)
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

            if (isOut && normalizedTime >= 1)
                target = null;
            }
        
    }

    public void AttackTarget()
    {

        if (target != null && attackTimer >= attackSpeed )
        {
  
            spineAnimation.PlayAnimation(skinName + "/attack", false, 1);
            attackTimer = 0;
        }

        if (target == null && transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/idle")
        {
            spineAnimation.PlayAnimation(skinName + "/idle", true, 1);
        }
    }


    public List<Node> GetAttackRangeNodesList(Direction direction)
    {
        List<Node> tiles = attackRangeNodes.ToList();

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
