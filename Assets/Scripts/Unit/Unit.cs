using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;

public enum Direction
{
    LEFT,
    RIGHT
}



public class Unit : MonoBehaviour
{
    public GameObject GameDataManager;
    public string poolItemName;
    public string Unitname;
    private Stat ParsingStat;

    [Header("UnitStat")]
    public float maxHp;
    public float currentHp { get; set; }
    public Tile onTile { get; set; }
    public float atk;
    public float currentAtk; //{ get; set; }
    public float def;
  //  public float moveSpeed;
    public float attackRangeDistance; // 유닛 공격 범위
    public float cognitiveRangeDistance; // 유닛 인지 범위
    public float attackSpeed;  //{ get; set; }

    private bool isPoisoned = false;
    public int damageRedution = 0;
    public int healTakeAmount = 0;

    public Direction direction { get; set; }

    public SpineAnimation spineAnimation { get; set; }
    public SkeletonAnimation skeletonAnimation { get; set; }

    public GameObject target { get; set; }
    public Image healthBar;

    public SkeletonDataAsset skeletonData { get; set; }

    public string skinName { get; set; }

    private Color initSkeletonColor; // 최초 스파인 색상

    public float normalizedTime { get; set; }  //스파인 애니메이션 진행도 0~1

    protected virtual void Awake()
    {
        
    }

    

    protected virtual void Start()
    {
        if(Unitname == "Enemy1" || Unitname == "Enemy2")
        {

           
        }
        else
        {
            GameDataManager.gameObject.GetComponent<CSV_Player_Status>().StartParsing();
            ParsingStat = GameDataManager.gameObject.GetComponent<CSV_Player_Status>().Call_Stat(Unitname);

            maxHp = ParsingStat.HP;
            atk = ParsingStat.Atk;
            def = ParsingStat.Def;
            attackRangeDistance = ParsingStat.AtkRange;
            cognitiveRangeDistance = ParsingStat.CognitiveRange;
            attackSpeed = ParsingStat.AtkSpeed;
        }



        if (!GetComponent<UnitStateMachine>())
        {
            gameObject.AddComponent<UnitStateMachine>();
        }

        if (transform.GetChild(0).GetComponent<SpineAnimation>() == null) transform.GetChild(0).gameObject.AddComponent<SpineAnimation>();

        spineAnimation = transform.GetChild(0).GetComponent<SpineAnimation>();
        skeletonAnimation = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        skeletonData = transform.GetChild(0).GetComponent<SkeletonAnimation>().skeletonDataAsset;
        transform.GetChild(0).GetComponent<SkeletonAnimation>().Initialize(true);
        skinName = transform.GetChild(0).GetComponent<SkeletonAnimation>().initialSkinName;
        initSkeletonColor = transform.GetChild(0).GetComponent<SkeletonAnimation>().skeleton.GetColor();

    
        UpdateHealthbar();
    }

    public void Init()
    {
        currentAtk = atk;
        currentHp = maxHp;
        attackSpeed = 1;
        damageRedution = 0;
        healthBar.transform.parent.gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        UpdateHealthbar();
    }

    protected virtual void Update()
    {
        Spine.TrackEntry trackEntry = new Spine.TrackEntry();
        trackEntry = spineAnimation.skeletonAnimation.AnimationState.Tracks.ElementAt(0);
        normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd;


    }

    public void Poison(SkillAbility skillAbility, float damage, float duration)
    {
        if (isPoisoned == false)
            StartCoroutine(PoisionCorutine(skillAbility, damage, duration));
    }

    public IEnumerator PoisionCorutine(SkillAbility skillAbility, float damage, float duration)
    {
        float timer = 0f;

        float damageTimer = 0;
        float damageDelay = 1;

        isPoisoned = true;

        EffectManager.Instance.InstantiateHomingEffect("isabella_skill", gameObject, duration);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageDelay)
            {
                damageTimer = 0;
                currentHp -= (int)(damage);
                UpdateHealthbar();
            }

            yield return null;
        }

        isPoisoned = false;
    }

    /// <summary>
    /// 데미지 부여 damage가 음수일 때 회복
    /// </summary>
    /// <param name="damage"></param>
    public void Deal(float damage)
    {
        float damageSum = 0;

        if (damage < 0) //heal
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(ChangeUnitColor(Color.green, 0.2f));

            damageSum = damage + (float)damage * ((float)healTakeAmount / 100);
            Debug.Log(damageSum);
        }
        else
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(ChangeUnitColor(Color.red, 0.2f));

            //데미지 = (공격력 - 방어력) * N/100
            damageSum = (float)(damage - def) * (float)(100 - damageRedution) / 100;
            damageSum = Mathf.Max(damageSum, 1);
        }

        currentHp -= (int)damageSum;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        UpdateHealthbar();
    }

    /// <summary>
    /// 체력바 UI 이미지 갱신
    /// </summary>
    public void UpdateHealthbar() => healthBar.fillAmount = (float)currentHp / maxHp;

    public void SetDirection(Direction direction)
    {
        Vector3 scale = Vector3.one;
        this.direction = direction;

        if (direction == Direction.LEFT)
        {
            scale.x = -1;
            transform.GetChild(0).localScale = new Vector3(Mathf.Abs(transform.GetChild(0).localScale.x) * scale.x, transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.z);
        }
        else if (direction == Direction.RIGHT)
        {
            scale.x = 1;
            transform.GetChild(0).localScale = new Vector3(Mathf.Abs(transform.GetChild(0).localScale.x) * scale.x, transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.z);
        }
    }

    public IEnumerator ChangeUnitColor(Color color, float duration)
    {
        if (gameObject != null)
            StopCoroutine("ChangeUnitColor");

        float timer = 0f;

        transform.GetChild(0).GetComponent<SkeletonAnimation>().skeleton.SetColor(color);


        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;

            if (gameObject != null)
                StopCoroutine("ChangeUnitColor");
        }


        transform.GetChild(0).GetComponent<SkeletonAnimation>().skeleton.SetColor(initSkeletonColor);
    }

    public IEnumerator Die()
    {
        if (!isAnimationPlaying("/knockdown"))
        {
            spineAnimation.PlayAnimation(skinName + "/knockdown", false, GameManager.Instance.gameSpeed);
        }

        if (skeletonAnimation.AnimationName == skinName + "/knockdown" && normalizedTime >= 1)
        {
            if (GetComponent<Minion>())
            {
                gameObject.SetActive(false);
            }
            else if(GetComponent<Enemy>())
            {
                ObjectPool.Instance.PushToPool(poolItemName, gameObject);
                GameManager.Instance.enemiesList.Remove(gameObject);
            }
        }

        yield return null;
    }

    public void SetPositionOnTile()
    {
        transform.position = onTile.gameObject.transform.position + GameManager.Instance.minionSetPosition;
    }

    /// <summary>
    /// 스파인 애니메이션 종료 확인 함수 </summary> <param name="animationName"> 스파인 애니메이션 이름</param>
    /// </summary>
    public bool isAnimationPlaying(string animationName) =>  skeletonAnimation.AnimationName == skinName + animationName && normalizedTime< 1;  // 실행중인 애니메이션의 이름이 animationName과 다르거나 끝나지 않았을 때
}