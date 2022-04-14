using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spine.Unity;
using Spine;
using Event = Spine.Event;
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

public enum AttackType
{
    Bullet,
    Melee
}

public class Minion : Unit
{
    public MinionClass minionClass;

    public List<Node> attackRangeNodes = new List<Node>();
    public List<Tile> attackRangeTiles { get; set; }

    public AttackType attackType;
    public Sprite bulletSprite;

    public int cost;
    public float minionWaitingTime;
    public float minionStandbyTime;

    private void Awake()
    {
        attackRangeTiles = new List<Tile>();
    }

    protected override void Start()
    {
        base.Start();
        transform.GetChild(0).GetComponent<SkeletonAnimation>().state.Event += AnimationSatateOnEvent;

        //        attackTimer = attackSpeed;
    }

    public void AnimationSatateOnEvent(TrackEntry trackEntry, Event e)
    {
        if (e.Data.Name == "shoot")
        {
            switch(attackType)
            {
                case AttackType.Bullet:
                 BulletAttack();
                    break;
                case AttackType.Melee:
                    MeleeAttack();
                    break;
            }
        }
    }

    public void MeleeAttack()
    {
        target.GetComponent<Unit>().Deal(atk);
    }

    public void BulletAttack()
    {
        Vector3 pos = transform.position;

        GameObject bulletObject = ObjectPool.Instance.PopFromPool("Bullet");
        bulletObject.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        bulletObject.transform.position = pos;
        bulletObject.GetComponent<Bullet>().Init(atk, target);

        bulletObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
    //    attackTimer += Time.deltaTime;

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
        Spine.TrackEntry trackEntry = new Spine.TrackEntry();
        trackEntry = spineAnimation.skeletonAnimation.AnimationState.Tracks.ElementAt(0);
        float normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd;

        if (target != null && transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/attack" || (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName == skinName + "/attack" && normalizedTime >= 1))
        {
            spineAnimation.PlayAnimation(skinName + "/attack", false, 1);
        }

        if (target == null && transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/idle")
        {
            spineAnimation.PlayAnimation(skinName + "/idle", true, 1);
        }
    }


  
}
