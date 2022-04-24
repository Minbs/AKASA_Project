using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spine.Unity;
using Spine;
using Event = Spine.Event;

public class OffenceModeEnemy : Unit
{
    // Start is called before the first frame update
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

        moveTiles = OffenseModeGameManager.Instance.tilesList.ToList();
        moveTiles.Reverse();
        attackDelayTimer = attackDelayDuration;
    }

    private void OnDestroy()
    {
        if(OffenseModeGameManager.Instance != null)
            OffenseModeGameManager.Instance.enemiesList.Remove(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Spine.TrackEntry trackEntry = new Spine.TrackEntry();
        trackEntry = spineAnimation.skeletonAnimation.AnimationState.Tracks.ElementAt(0);
        float normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd;

        attackDelayTimer += Time.deltaTime;

        if (currentHp <= 0)
        {
            StartCoroutine(Die());
            return;
        }

   
        AimTarget();

        if ( moveTiles.Count != 0)
        {
            if(target == null && (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/attack") || (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName == skinName + "/attack" && normalizedTime >= 1))
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
            OffenseModeGameManager.Instance.enemiesList.Remove(gameObject);
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
        //des.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, des, speed * Time.deltaTime);

        if (moveTiles.Count > 1)
        {

            if (des.x - transform.position.x < 0)
                SetDirection(Direction.RIGHT);
            if (des.x - transform.position.x > 0)
                SetDirection(Direction.LEFT);
           if (des.z - transform.position.z > 0)
               SetDirection(Direction.UP);
            if (des.z - transform.position.z < 0)
                SetDirection(Direction.DOWN);
        }

        if (Vector3.Distance(transform.position, des) < 0.01f)
        {
            transform.position = des;
           

            moveTiles.RemoveAt(0);

            
        }
    }

    void AimTarget()
    {
   
            if (target == null && OffenseModeGameManager.Instance.minionsList.Count > 0)
            {
                foreach (var minion in OffenseModeGameManager.Instance.minionsList)
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
        Spine.TrackEntry trackEntry = new Spine.TrackEntry();
        trackEntry = spineAnimation.skeletonAnimation.AnimationState.Tracks.ElementAt(0);
        float normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd;

        if (target != null && (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/attack" || (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName == skinName + "/attack" && normalizedTime >= 1)))
        {
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
}
