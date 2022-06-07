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
    List<Tile> moveTiles = new List<Tile>();
    [Header("EnemyStat")]

    public AttackType attackType;

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
