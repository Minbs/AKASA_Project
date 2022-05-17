using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spine.Unity;
using Spine;
using Event = Spine.Event;

public class OffenceEnemy : Unit
{
    List<Tile> moveTiles = new List<Tile>();

    [Header("EnemyStat")]
    public float speed;
    public AttackType attackType;
    public float attackRangeDistance;

    public float attackDelayTimer { get; set; }
    public float attackDelayDuration;

    protected override void Start()
    {
        base.Start();
        transform.GetChild(0).GetComponent<SkeletonAnimation>().state.Event += AnimationSatateOnEvent;

        moveTiles = OffenceModeGameManager.Instance.tilesList.ToList();
        moveTiles.Reverse();
        attackDelayTimer = attackDelayDuration;
    }

    private void OnDestroy()
    {
        if (OffenceModeGameManager.Instance != null)
            OffenceModeGameManager.Instance.enemiesList.Remove(gameObject);
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


        attackDelayTimer += Time.deltaTime;

        AimTarget();

        if (moveTiles.Count != 0)
        {
            if (target == null && !isAnimationPlaying("/attack"))
            {
                Move();
            }
            else
            {
                AttackTarget();
            }
        }
        else if (moveTiles.Count == 0)
        {
            OffenceModeGameManager.Instance.enemiesList.Remove(gameObject);
            ObjectPool.Instance.PushToPool(poolItemName, gameObject);
        }
    }

    void Move()
    {
        if (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/move")
        {
            spineAnimation.PlayAnimation(skinName + "/move", true, 1);
        }

        Vector3 des = moveTiles[0].transform.position;
        transform.position = Vector3.MoveTowards(transform.position, des, speed * Time.deltaTime);

        if (moveTiles.Count > 1)
        {
            if (des.x - transform.position.x < 0) SetDirection(Direction.RIGHT);
            else if (des.x - transform.position.x > 0) SetDirection(Direction.LEFT);
        }

        if (Vector3.Distance(transform.position, des) < 0.01f)
        {
            transform.position = des;
            moveTiles.RemoveAt(0);
        }
    }

    void AimTarget()
    {
        if (target == null && OffenceModeGameManager.Instance.minionsList.Count > 0)
        {
            foreach (var minion in OffenceModeGameManager.Instance.minionsList)
            {
                if (Vector3.Distance(transform.position, minion.transform.position) <= attackRangeDistance && transform.position.x < minion.transform.position.x)
                {
                    target = minion;
                    break;
                }
            }
        }
        else if (target != null)
        {
            if ((Vector3.Distance(transform.position, target.transform.position) > attackRangeDistance || transform.position.x >= target.transform.position.x) || target.GetComponent<Unit>().currentHp <= 0)
            {
                target = null;
            }
        }
    }

    void AttackTarget()
    {
        if (isAnimationPlaying("/attack"))
            return;

        attackDelayTimer = 0;

        spineAnimation.PlayAnimation(skinName + "/attack", false, 1);

        switch (attackType)
        {
            case AttackType.Melee:
                MeleeAttack();
                break;
            case AttackType.HitScan:
                HitScanAttack();
                break;
        }

    }

    void MeleeAttack()
    {
        target.GetComponent<Unit>().Deal(atk);
    }

    void HitScanAttack()
    {
        target.GetComponent<Unit>().Deal(atk);
    }

    public void AnimationSatateOnEvent(TrackEntry trackEntry, Event e)
    {
        Debug.Log(e.Data.Name);

        if (e.Data.Name == "shoot")
        {
            switch (attackType)
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
}
