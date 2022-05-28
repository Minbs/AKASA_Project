using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spine.Unity;
using Spine;
using Event = Spine.Event;
using UnityEditor;


public enum AttackType
{
    Bullet,
    Melee,
    SingleHeal,
    HitScan
}

public enum SkillType
{
    Buff,
    EnhanceNextAttack
}

public enum ActiveSkillType
{
    Auto,
    Manual
}

public class DefenceMinion : Minion
{
    [Header("MinionStat")]
    public AttackType attackType;

    public float cost;
    public float sellCost;
    //public float minionStandbyTime { get; set; }
    //public float minionWaitingTime;

    public Sprite bulletSprite;

    public float healAmountRate { get; set; }

    public GameObject shootPivot;

    private void Awake()
    {
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.minionsList.Remove(gameObject);
        }
    }

    protected override void Start()
    {
        base.Start();
        transform.GetChild(0).GetComponent<SkeletonAnimation>().state.Event += AnimationSatateOnEvent;
        healAmountRate = 100;
    }

    public void AnimationSatateOnEvent(TrackEntry trackEntry, Event e)
    {
        if (target == null)
        {
            return;
        }

        if (e.Data.Name == "shoot" && transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName == skinName + "/attack")
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
                case AttackType.HitScan:
                    HitScanAttack();
                    break;
            }
        }

        if (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName == skinName + "/skill")
        {

        }
    }



    #region BaseAttack
    public void HitScanAttack()
    {
        if (target == null)
        {
            return;
        }


        EffectManager.Instance.InstantiateAttackEffect("hwaseon_hit", target.transform.position);
        target.GetComponent<Unit>().Deal(currentAtk);
    }
    public void SingleHeal()
    {
        Vector3 pos = transform.position;
        GameObject bulletObject = ObjectPool.Instance.PopFromPool("Bullet");
        bulletObject.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        bulletObject.transform.position = pos;
        bulletObject.GetComponent<Bullet>().Init(-(int)(currentAtk * (healAmountRate / 100)), target);

        bulletObject.SetActive(true);
    }

    public void MeleeAttack()
    {
        target.GetComponent<Unit>().Deal(currentAtk);
    }

    public void BulletAttack()
    {
        Vector3 pos = shootPivot.transform.position;
        GameObject bulletObject = ObjectPool.Instance.PopFromPool("Bullet");
        bulletObject.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        bulletObject.transform.position = pos;
        bulletObject.GetComponent<Bullet>().Init(currentAtk, target);

        bulletObject.SetActive(true);
    }
    #endregion

    //시야 확인 변수
    public float sightAngle;
    public float sightDistance;

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;

        Handles.DrawSolidArc(transform.position, Vector3.up, transform.right, -sightAngle / 2, sightDistance);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.right, sightAngle / 2, sightDistance);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}

