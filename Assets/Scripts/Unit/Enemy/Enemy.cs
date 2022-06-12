using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spine.Unity;
using Spine;
using Event = Spine.Event;

public class Enemy : Unit
{
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        transform.GetChild(0).GetComponent<SkeletonAnimation>().state.Event += AnimationSatateOnEvent;
        Init();
        UpdateHealthbar();
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        GameManager.Instance.enemiesList.Remove(gameObject);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (currentHp <= 0)
        {
            StartCoroutine(Die());
            return;
        }


    }

    void MeleeAttack()
    {
        target.GetComponent<Object>().Deal(currentAtk);
    }

    void HitScanAttack()
    {
        target.GetComponent<Object>().Deal(currentAtk);
    }

    public void MeleeRangeAttack()
    {
   //     Debug.Log(attackRange2);
        Vector3 box = new Vector3(attackRangeDistance, 1, attackRange2);
        Vector3 center = transform.position;
        center.x = transform.position.x + attackRangeDistance / 2;
        Collider[] targets = Physics.OverlapBox(center, box, Quaternion.identity);

        foreach (var e in targets)
        {
            if (!e.transform.parent.tag.Equals("Ally")) continue;

            e.transform.parent.GetComponent<Unit>().Deal(currentAtk);

        }
    }

    public void HealRangeAttack()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, attackRangeDistance);

        foreach (var e in targets)
        {
            if (!e.transform.tag.Equals("Enemy")
                || e.transform.parent .GetComponent<Object>().currentHp <= 0
                || e.transform.parent. GetComponent<Object>().currentHp >= e.transform.parent. GetComponent<Object>().maxHp) continue;



            e.transform.parent.GetComponent<Unit>().Deal(-currentAtk);

            EffectManager.Instance.InstantiateHomingEffect("heal", e.transform.parent.gameObject, 2);
        }
    }


    public void AnimationSatateOnEvent(TrackEntry trackEntry, Event e)
    {
        if (target == null)
        {
            return;
        }

        if (e.Data.Name == "shoot")
        {
            if(target != GameManager.Instance.turret)
            target.GetComponent<UnitStateMachine>().SetAttackTargetInRange(gameObject);

            switch(attackType)
            {
                case AttackType.Melee:
                    MeleeAttack();
                    break;
                case AttackType.HitScan:
                    HitScanAttack();
                    break;
                case AttackType.MeleeRange:
                    MeleeRangeAttack();
                    break;
                case AttackType.HealRange:
                    HealRangeAttack();
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "Finish")
        {
            GameManager.Instance.enemiesList.Remove(gameObject);
            Destroy(gameObject);
        }
    }

}
