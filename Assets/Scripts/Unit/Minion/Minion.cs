using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spine.Unity;

public enum MinionClass
{
    Buster,
    Paladin,
    Guardian,
    Assassin,
    Chaser,
    Mage,
    TacticalSupport
}
public class Minion : Unit
{
    public MinionClass minionClass;

    public List<Node> attackRangeNodes = new List<Node>();
    public List<Tile> attackRangeTiles = new List<Tile>();

    private float attackTimer = 0;
    public float attackSpeed;



    protected override void Start()
    {
        base.Start();
        attackTimer = attackSpeed;


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


  
}
