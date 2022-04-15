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
    Rescue,
    TacticalSupport
}

public enum AttackType
{
    Bullet,
    Melee,
    SingleHeal
}

public class Minion : Unit
{
    public MinionClass minionClass;

    public List<Node> attackRangeNodes = new List<Node>();
    public List<Tile> attackRangeTiles { get; set; }

    public AttackType attackType;
    public Sprite bulletSprite;

    public int stopCount; // ÀúÁö ¼ö

    public int cost;
    public float waitingTime;

    private void Awake()
    {
        attackRangeTiles = new List<Tile>();
    }

    protected override void Start()
    {
        base.Start();
        transform.GetChild(0).GetComponent<SkeletonAnimation>().state.Event += AnimationSatateOnEvent;
    }

    public void AnimationSatateOnEvent(TrackEntry trackEntry, Event e)
    {
        if (e.Data.Name == "shoot")
        {
            switch (attackType)
            {
                case AttackType.Bullet:
                    BulletAttack();
                    break;
                case AttackType.Melee:
                    MeleeAttack();
                    break;
                case AttackType.SingleHeal:
                    SingleHeal();
                    break;
            }
        }
    }

    public void SingleHeal()
    {
        Vector3 pos = transform.position;
        GameObject bulletObject = ObjectPool.Instance.PopFromPool("Bullet");
        bulletObject.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        bulletObject.transform.position = pos;
        bulletObject.GetComponent<Bullet>().Init(-atk, target);

        bulletObject.SetActive(true);
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
        AimTarget();
        AttackTarget();

    }

    public void AimTarget()
    {
        Spine.TrackEntry trackEntry = new Spine.TrackEntry();
        trackEntry = spineAnimation.skeletonAnimation.AnimationState.Tracks.ElementAt(0);
        float normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd;

        var result = GameManager.Instance.minionsList.OrderBy(minion => minion.GetComponent<Unit>().currentHp);
        GameManager.Instance.minionsList = result.ToList();

        if (minionClass != MinionClass.Rescue)
        {

            if (target == null)
            {
                foreach (var enemy in GameManager.Instance.enemiesList)
                {
                    foreach (var t in attackRangeTiles)
                    {

                        if (t.onTile(enemy.transform) && enemy.GetComponent<Unit>().currentHp > 0)
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

                if ((isOut || target.GetComponent<Unit>().currentHp <= 0) && normalizedTime >= 1)
                    target = null;
            }
        }
        else
        {
            if(target != null)
            if ((target.GetComponent<Unit>().currentHp >= target.GetComponent<Unit>().maxHp || target.GetComponent<Unit>().currentHp <= 0) && normalizedTime >= 1)
            {            
                target = null;
            }



            foreach (var minion in GameManager.Instance.minionsList)
            {
                foreach (var t in attackRangeTiles)
                {

                    if (t.onTile(minion.transform) && minion.GetComponent<Unit>().currentHp < minion.GetComponent<Unit>().maxHp)
                    {
                        target = minion;

                        return;
                    }
                }
            }


        }

    }

    public void AttackTarget()
    {
        Spine.TrackEntry trackEntry = new Spine.TrackEntry();
        trackEntry = spineAnimation.skeletonAnimation.AnimationState.Tracks.ElementAt(0);
        float normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd;

        if (target != null && (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/attack" || (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName == skinName + "/attack" && normalizedTime >= 1)))
        {
            Vector3 scale = transform.localScale;
            if (target.transform.position.x - transform.position.x >= -0.001)
            {
                scale.x = 1;
            }
            else
            {
                scale.x = -1;
            }

            transform.localScale = scale;

            spineAnimation.PlayAnimation(skinName + "/attack", false, 1);



        }

        if (target == null && transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/idle")
        {
            spineAnimation.PlayAnimation(skinName + "/idle", true, 1);
        }
    }



}
