using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spine.Unity;
using Spine;
using Event = Spine.Event;



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
            switch (skillType)
            {
                case SkillType.EnhanceNextAttack:
                    if(attackType == AttackType.Melee)
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


        EffectManager.Instance.InstantiateAttackEffect("hwaseon_hit",target.transform.position);
        target.GetComponent<Unit>().Deal(atk);
    }

    public void HitScanAttack2()
    {
        if (target == null)
        {
            return;
        }



        EffectManager.Instance.InstantiateHomingEffect("hwaseon_skill", target, activeSkillAbilities[0].duration);
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

        if(activeSkillAbilities[0].abilityType.statusEffect.statusEffect == StatusEffect.StatusEffects.Heal)
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
                if(skillAbility.abilityType.statusEffect.statusEffect != StatusEffect.StatusEffects.Poison)
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
          //  Debug.Log("en : " + atk);
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

   //     if(target.GetComponent<Enemy>() != null)
     //       initSpeed = target.GetComponent<Enemy>().moveSpeed;

        switch (skillAbility.statType)
        {
            case StatType.AttackSpeed:
                value = skillAbility.power / 100;
                target.attackSpeed += value;
                EffectManager.Instance.InstantiateHomingEffect("verity_skill", gameObject, skillAbility.duration);
                break;
            case StatType.HealAmountRate:
                value = skillAbility.power;
                target.GetComponent<DefenceMinion>().healAmountRate = value;
                break;
            case StatType.DEF:
                value = skillAbility.power / 100;
                target.def += value;
                break;
            case StatType.MoveSpeed:
                value = skillAbility.power / 100;
          //      target.GetComponent<Enemy>().moveSpeed *= 1+ value;
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
                target.GetComponent<DefenceMinion>().healAmountRate = 100;
                break;
            case StatType.DEF:
                target.def -= value;
                break;
            case StatType.MoveSpeed:
   //             target.GetComponent<Enemy>().moveSpeed = initSpeed;
                break;
        }
    }

    #endregion

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}

