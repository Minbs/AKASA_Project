using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using UnityEngine.AI;
public enum Direction
{
    LEFT,
    RIGHT
}



public class Unit : MonoBehaviour
{
    public string poolItemName;

    [Header("UnitStat")]
    public float maxHp;
    public float currentHp { get; set; }

    public Tile onTile { get; set; }

    public float atk;
    public float currentAtk; //{ get; set; }
    public float def;
  //  public float moveSpeed;
    public float attackRangeDistance; // ���� ���� ����
    public float cognitiveRangeDistance; // ���� ���� ����
    public float attackSpeed;  //{ get; set; }

    private bool isPoisoned = false;
    public float damageRedution = 0;
    public float healTakeAmount = 0;

    public Direction direction { get; set; }

    public SpineAnimation spineAnimation { get; set; }
    public SkeletonAnimation skeletonAnimation { get; set; }

    public GameObject target { get; set; }
    public Image healthBar;

    public SkeletonDataAsset skeletonData { get; set; }

    public string skinName { get; set; }

    private Color initSkeletonColor; // ���� ������ ����

    public float normalizedTime { get; set; }  //������ �ִϸ��̼� ���൵ 0~1

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
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

        if(GetComponent<Minion>())
        transform.GetComponent<NavMeshAgent>().enabled = false;
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
        transform.GetComponent<NavMeshAgent>().enabled = true;
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
                currentHp -= (float)(damage);
                UpdateHealthbar();
            }

            yield return null;
        }

        isPoisoned = false;
    }

    /// <summary>
    /// ������ �ο� damage�� ������ �� ȸ��
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

            //������ = (���ݷ� - ����) * N/100
            damageSum = (float)((float)damage - def) * (float)(100 - damageRedution) / 100;
            damageSum = Mathf.Max(damageSum, 1);
        }

        currentHp -= (float)damageSum;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        UpdateHealthbar();
    }

    /// <summary>
    /// ü�¹� UI �̹��� ����
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
    /// ������ �ִϸ��̼� ���� Ȯ�� �Լ� </summary> <param name="animationName"> ������ �ִϸ��̼� �̸�</param>
    /// </summary>
    public bool isAnimationPlaying(string animationName) =>  skeletonAnimation.AnimationName == skinName + animationName && normalizedTime< 1;  // �������� �ִϸ��̼��� �̸��� animationName�� �ٸ��ų� ������ �ʾ��� ��
}