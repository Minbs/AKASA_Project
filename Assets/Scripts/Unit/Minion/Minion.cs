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

public class Minion : Unit
{
    public MinionClass minionClass;

    public List<Node> attackRangeNodes = new List<Node>();
    public List<Tile> attackRangeTiles { get; set; }

    public AttackType attackType;
    public Sprite bulletSprite;

    public int stopCount; // ÀúÁö ¼ö
    public int currentStopCount { get; set; }

    public int cost;
    public float minionStandbyTime;
    public float minionWaitingTime;

    public float currentSkillGauge;
    public int maxSkillGauge;

    public Tile onTileNode { get; set; }

    public List<SkillAbility> activeSkillAbilities = new List<SkillAbility>();
    public ActiveSkillType activeSkillType;

    public SkillType skillType;

    public float healAmountRate = 100;

    public bool isNextBaseAttackEnhanced = false;

    public bool isEnhanced = false;

    public GameObject shootPivot;

    private void Awake()
    {
        attackRangeTiles = new List<Tile>();
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
    }

    public void AnimationSatateOnEvent(TrackEntry trackEntry, Event e)
    {
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
            Debug.Log(e.Data.Name);

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
        target.GetComponent<Unit>().Deal(atk);
    }

    public void HitScanAttack2()
    {
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

                        if ((t.onTile(enemy.transform) || (enemy.GetComponent<Unit>().target == gameObject && enemy.GetComponent<Enemy>().attackType == AttackType.Melee)) && enemy.GetComponent<Unit>().currentHp > 0)
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

                if ((isOut || target.GetComponent<Unit>().currentHp <= 0) && normalizedTime >= 1 && target.GetComponent<Unit>().target != gameObject)
                    target = null;
            }
        }
        else
        {
            if (target != null)
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

        
        if(transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName == skinName + "/skill" 
            && normalizedTime < 1)
        {
            return;
        }
        
        if (target != null && (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/attack" || ((transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName == skinName + "/attack" )
            && normalizedTime >= 1)))
        {

            Vector3 scale = Vector3.one;
            if (target.transform.position.x - transform.position.x >= -0.001)
            {
                scale.x = 1;
            }
            else
            {
                scale.x = -1;
            }

            transform.GetChild(0).localScale = new Vector3(Mathf.Abs(transform.GetChild(0).localScale.x) * scale.x, transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.z);

            if(isNextBaseAttackEnhanced)
            {
                if(activeSkillAbilities[0].abilityType.statusEffect.statusEffect != StatusEffect.StatusEffects.Poison)
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
            spineAnimation.PlayAnimation(skinName + "/idle", true, 1);
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
            atk *= (int)(skillAbility.power / 100);
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

        float initSpeed = target.GetComponent<Enemy>().speed;
        switch (skillAbility.statType)
        {
            case StatType.AttackSpeed:
                value = skillAbility.power / 100;
                target.attackSpeed += value;
                break;
            case StatType.HealAmountRate:
                value = skillAbility.power;
                target.GetComponent<Minion>().healAmountRate = value;
                break;
            case StatType.DEF:
                value = skillAbility.power / 100;
                target.def += value;
                break;
            case StatType.MoveSpeed:
                value = skillAbility.power / 100;
                target.GetComponent<Enemy>().speed *= 1+ value;
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
                target.GetComponent<Minion>().healAmountRate = 100;
                break;
            case StatType.DEF:
                target.def -= value;
                break;
            case StatType.MoveSpeed:
                target.GetComponent<Enemy>().speed = initSpeed;
                break;
        }
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (currentHp <= 0)
        {
            StartCoroutine(Die());
            return;
        }

        if(attackType == AttackType.Melee)
        {
            currentStopCount = stopCount;
            foreach (var enemy in GameManager.Instance.enemiesList)
            {
                if(enemy.GetComponent<Unit>().target == gameObject && enemy.GetComponent<Enemy>().attackType == AttackType.Melee)
                {
                    currentStopCount--;
                }    
            }
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

    }





}
