using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spine.Unity;
using Spine;
using Event = Spine.Event;

public class OffenceMinion : Minion
{
    public float speed;
    [Header("MinionStat")]

    public AttackType attackType;

    [DrawIf("minionClass", MinionClass.Buster)]
    public int stopCount; // ÀúÁö ¼ö


    public int currentStopCount { get; set; }


    public int cost;
    public float minionStandbyTime { get; set; }
    public float minionWaitingTime;

    public float currentSkillGauge { get; set; }
    public int maxSkillGauge;

    public Tile onTileNode { get; set; }

    public List<SkillAbility> activeSkillAbilities = new List<SkillAbility>();
    public ActiveSkillType activeSkillType;


    public Sprite bulletSprite;

    public SkillType skillType;

    public float healAmountRate { get; set; }



    public bool isNextBaseAttackEnhanced { get; set; }


    public GameObject shootPivot;





    public bool isEnhanced { get; set; }

    List<Tile> moveTiles = new List<Tile>();

    public float attackRangeDistance;

    private void Awake()
    {
    }

    private void OnDestroy()
    {
        if (OffenseModeGameManager.Instance != null)
        {
            OffenseModeGameManager.Instance.minionsList.Remove(gameObject);
        }
    }

    protected override void Start()
    {
        base.Start();
        transform.GetChild(0).GetComponent<SkeletonAnimation>().state.Event += AnimationSatateOnEvent;
        healAmountRate = 100;

        moveTiles = OffenseModeGameManager.Instance.tilesList.ToList();
    }

    public void AnimationSatateOnEvent(TrackEntry trackEntry, Event e)
    {
        Debug.Log(e.Data.Name);

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


            switch (skillType)
            {
                case SkillType.EnhanceNextAttack:
                    if (attackType == AttackType.Melee)
                        MeleeAttack2();
                    if (attackType == AttackType.HitScan)
                        HitScanAttack2();
                    if (attackType == AttackType.Bullet)
                        BulletAttack2();
                    break;
            }
        }
    }

    public void BulletAttack2()
    {
        Vector3 pos = shootPivot.transform.position;
        GameObject bulletObject = ObjectPool.Instance.PopFromPool("Bullet");
        bulletObject.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        bulletObject.transform.position = pos;
        bulletObject.GetComponent<Bullet>().Init(atk, target);
        bulletObject.GetComponent<Bullet>().duration = activeSkillAbilities[0].duration;
        bulletObject.GetComponent<Bullet>().skillAbility = activeSkillAbilities[0];
        bulletObject.GetComponent<Bullet>().isPoison = true;
        bulletObject.GetComponent<Bullet>().power = atk * (activeSkillAbilities[0].power / 100);

        bulletObject.SetActive(true);
    }

    public void HitScanAttack()
    {
        if (target == null)
        {
            return;
        }


        EffectManager.Instance.InstantiateAttackEffect("hwaseon_hit", target.transform.position);
        target.GetComponent<Unit>().Deal(atk);
    }

    public void HitScanAttack2()
    {
        if (target == null)
        {
            return;
        }



        StartCoroutine(EffectManager.Instance.InstantiateHomingEffect("hwaseon_skill", target, activeSkillAbilities[0].duration));
        EffectManager.Instance.InstantiateAttackEffect("hwaseon_skill", target.transform.position);
        target.GetComponent<Unit>().Deal(atk);
    }

    #region BaseAttack
    public void SingleHeal()
    {
        Vector3 pos = transform.position;
        GameObject bulletObject = ObjectPool.Instance.PopFromPool("Bullet");
        bulletObject.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        bulletObject.transform.position = pos;
        bulletObject.GetComponent<Bullet>().Init(-(int)(atk * (healAmountRate / 100)), target);

        bulletObject.SetActive(true);
    }

    public void MeleeAttack()
    {
        target.GetComponent<Unit>().Deal(atk);
    }

    public void MeleeAttack2()
    {
        target.GetComponent<Unit>().Deal(atk);

        if (activeSkillAbilities[0].abilityType.statusEffect.statusEffect == StatusEffect.StatusEffects.Heal)
        {
            Deal(-(6 / 100) * maxHp);
        }
    }

    public void BulletAttack()
    {
        Vector3 pos = shootPivot.transform.position;
        GameObject bulletObject = ObjectPool.Instance.PopFromPool("Bullet");
        bulletObject.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        bulletObject.transform.position = pos;
        bulletObject.GetComponent<Bullet>().Init(atk, target);

        bulletObject.SetActive(true);
    }

    void AimTarget()
    {
        if(minionClass == MinionClass.Rescue)
        {
            if (target == null && OffenseModeGameManager.Instance.minionsList.Count > 0)
            {
                foreach (var minion in OffenseModeGameManager.Instance.minionsList)
                {
                    if (Vector3.Distance(transform.position, minion.transform.position) <= attackRangeDistance && transform.position.x > minion.transform.position.x)
                    {
                        target = minion;
                        break;
                    }
                }
            }
            else if (target != null)
            {
                if ((Vector3.Distance(transform.position, target.transform.position) > attackRangeDistance || transform.position.x <= target.transform.position.x) || target.GetComponent<Unit>().currentHp <= 0)
                {
                    target = null;
                }
            }
        }


        if (target == null && OffenseModeGameManager.Instance.enemiesList.Count > 0)
        {
            foreach (var enemy in OffenseModeGameManager.Instance.enemiesList)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRangeDistance && transform.position.x > enemy.transform.position.x)
                {
                    target = enemy;
                    break;
                }
            }
        }
        else if (target != null)
        {
            if ((Vector3.Distance(transform.position, target.transform.position) > attackRangeDistance || transform.position.x <= target.transform.position.x) || target.GetComponent<Unit>().currentHp <= 0)
            {
                target = null;
            }
        }
    }

    public void AttackTarget()
    {
        Spine.TrackEntry trackEntry = new Spine.TrackEntry();
        trackEntry = spineAnimation.skeletonAnimation.AnimationState.Tracks.ElementAt(0);
        float normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd;


        if (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName == skinName + "/skill"
            && normalizedTime < 1)
        {
            return;
        }

        if (target != null && (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/attack" || ((transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName == skinName + "/attack")
            && normalizedTime >= 1)))
        {

            if (isNextBaseAttackEnhanced)
            {
                if (activeSkillAbilities[0].abilityType.statusEffect.statusEffect != StatusEffect.StatusEffects.Poison)
                    isNextBaseAttackEnhanced = false;

                spineAnimation.PlayAnimation(skinName + "/skill", false, 1 * attackSpeed);

            }
            else
            {
                spineAnimation.PlayAnimation(skinName + "/attack", false, 1 * attackSpeed);
            }
        }

        if (target == null && transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/idle")
        {
         //   spineAnimation.PlayAnimation(skinName + "/idle", true, 1);
        }
    }
    #endregion

    #region Skill
    void UseSkill()
    {
        currentSkillGauge = 0;

        if (activeSkillAbilities.Count <= 0)
            return;



        for (int i = 0; i < activeSkillAbilities.Count; i++)
        {
            StartCoroutine(PerformSkill(activeSkillAbilities[0]));
        }


    }

    IEnumerator PerformSkill(SkillAbility skillAbility)
    {


        List<GameObject> targets = new List<GameObject>();

        if (skillAbility.abilityType.rangeType == Ranges.Self)
        {
            targets.Add(gameObject);
        }


        if (skillAbility.abilityType.note == Notes.StatChange && skillAbility.abilityType.rangeType == Ranges.Self)
        {
            foreach (var t in targets)
            {
                StartCoroutine(ChangeStat(skillAbility, t.GetComponent<Unit>()));
            }
        }

        if (skillAbility.abilityType.note == Notes.EnhanceNextBaseAttack)
        {
            foreach (var t in targets)
            {
                if (skillAbility.abilityType.statusEffect.statusEffect != StatusEffect.StatusEffects.Poison)
                    StartCoroutine(EnhanceNextAttack(skillAbility));
                else
                    StartCoroutine(EnhanceAttack(skillAbility));
            }
        }
        else
        {
            spineAnimation.PlayAnimation(skinName + "/skill", false, 1);
        }

        yield return null;
    }

    public IEnumerator EnhanceNextAttack(SkillAbility skillAbility)
    {
        int initAtk = atk;

        if (skillAbility.statType == StatType.ATK)
        {
            isNextBaseAttackEnhanced = true;
            float sum = atk;
            sum *= skillAbility.power / 100;
            atk = (int)sum;
        }

        if (skillAbility.statType == StatType.DEF)
        {
            isNextBaseAttackEnhanced = true;
        }

        if (skillAbility.statType == StatType.MoveSpeed)
        {
            isNextBaseAttackEnhanced = true;
        }

        while (isNextBaseAttackEnhanced)
        {
            yield return null;
        }

        if (skillAbility.statType == StatType.ATK)
        {
            atk = initAtk;
        }

        if (skillAbility.statType == StatType.DEF)
        {
            StartCoroutine(ChangeStat(skillAbility, target.GetComponent<Unit>()));
        }

        if (skillAbility.statType == StatType.MoveSpeed)
        {
            StartCoroutine(ChangeStat(skillAbility, target.GetComponent<Unit>()));
        }
    }

    public IEnumerator EnhanceAttack(SkillAbility skillAbility)
    {
        float timer = 0f;
        float duration = skillAbility.duration;


        isNextBaseAttackEnhanced = true;


        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        isNextBaseAttackEnhanced = false;
    }

    public IEnumerator ChangeStat(SkillAbility skillAbility, Unit target)
    {
        float timer = 0f;
        float duration = skillAbility.duration;
        float value = 0;
        float initSpeed = 0;

        if (target.GetComponent<OffenceModeEnemy>() != null)
            initSpeed = target.GetComponent<OffenceModeEnemy>().speed;

        switch (skillAbility.statType)
        {
            case StatType.AttackSpeed:
                value = skillAbility.power / 100;
                target.attackSpeed += value;
                StartCoroutine(EffectManager.Instance.InstantiateHomingEffect("verity_skill", gameObject, skillAbility.duration));
                break;
            case StatType.HealAmountRate:
                value = skillAbility.power;
                target.GetComponent<OffenceMinion>().healAmountRate = value;
                break;
            case StatType.DEF:
                value = skillAbility.power / 100;
                target.def += value;
                break;
            case StatType.MoveSpeed:
                value = skillAbility.power / 100;
                target.GetComponent<OffenceModeEnemy>().speed *= 1 + value;
                break;

        }



        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }


        switch (skillAbility.statType)
        {
            case StatType.AttackSpeed:
                target.attackSpeed -= value;
                break;
            case StatType.HealAmountRate:
                target.GetComponent<OffenceMinion>().healAmountRate = 100;
                break;
            case StatType.DEF:
                target.def -= value;
                break;
            case StatType.MoveSpeed:
                if(target != null)
                target.GetComponent<OffenceModeEnemy>().speed = initSpeed;
                break;
        }
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        Spine.TrackEntry trackEntry = new Spine.TrackEntry();
        trackEntry = spineAnimation.skeletonAnimation.AnimationState.Tracks.ElementAt(0);
        float normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd;

        if (currentHp <= 0)
        {
            StartCoroutine(Die());
            return;
        }


        currentSkillGauge += Time.deltaTime;

        if (currentSkillGauge >= maxSkillGauge)
        {
            currentSkillGauge = maxSkillGauge;

            if (activeSkillType == ActiveSkillType.Auto)
            {
                UseSkill();
                return;
            }
        }

        AimTarget();
        AttackTarget();


        if (target == null &&
            (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/attack"
            || (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName == skinName + "/attack" && normalizedTime >= 1)
            &&  (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/skill"
            || (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName == skinName + "/skill" && normalizedTime >= 1))))
        {
            Move();
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

        if (Vector3.Distance(transform.position, des) < 0.01f)
        {
            transform.position = des;


            moveTiles.RemoveAt(0);


        }

        if (moveTiles.Count == 0)
        {
            OffenseModeGameManager.Instance.minionsList.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
