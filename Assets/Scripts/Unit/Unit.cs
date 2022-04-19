using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using Spine;
using Event = Spine.Event;
public enum Direction
{
    LEFT,
    UP,
    RIGHT,
    DOWN
}



public class Unit : MonoBehaviour
{
    public string poolItemName;
    public int maxHp;
    public int currentHp;
    public Image healthBar;

    public int atk;
    public float def;

    public float attackSpeed = 1;

    private bool isPoisoned = false;

    public Direction direction { get; set; }

    public SpineAnimation spineAnimation { get; set; }

    public GameObject target;

    public SkeletonDataAsset skeletonData { get; set; }

    public string skinName { get; set; }

    private Color initSkeletonColor;
    protected virtual void Start()
    {
        if (transform.GetChild(0).GetComponent<SpineAnimation>() == null)
        {
            transform.GetChild(0).gameObject.AddComponent<SpineAnimation>();
        }

        spineAnimation = transform.GetChild(0).GetComponent<SpineAnimation>();



        skeletonData = transform.GetChild(0).GetComponent<SkeletonAnimation>().skeletonDataAsset;
        
        transform.GetChild(0).GetComponent<SkeletonAnimation>().Initialize(true);

        skinName = transform.GetChild(0).GetComponent<SkeletonAnimation>().initialSkinName;

        initSkeletonColor = transform.GetChild(0).GetComponent<SkeletonAnimation>().skeleton.GetColor();

        currentHp = maxHp;
        UpdateHealthbar();
    }

    public void Poison(SkillAbility skillAbility, int damage, float duration)
    {
        if(isPoisoned == false)
        StartCoroutine(PoisionCorutine(skillAbility, damage, duration));
    }
    public IEnumerator PoisionCorutine(SkillAbility skillAbility, int damage, float duration)
    {
        float timer = 0f;

        float damageTimer = 0;
        float damageDelay = 1;

        isPoisoned = true;

        StartCoroutine( EffectManager.Instance.InstantiateHomingEffect("isabella_skill", gameObject, duration));

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


    public void Deal(int damage)
    {

        float damageSum = 0;
        damageSum = damage;



        if (damage < 0) //heal
        {
            if(gameObject.activeInHierarchy)
            StartCoroutine(ChangeUnitColor(Color.green, 0.2f));
        }
        else
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(ChangeUnitColor(Color.red, 0.2f));
            damageSum *= (1 / (1 + def));
        }

     

        currentHp -= (int)damageSum;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        UpdateHealthbar();
    }

    public void UpdateHealthbar()
    {
        healthBar.fillAmount = (float)currentHp / maxHp;
    }

    public void SetDirection(Direction direction)
    {
        Vector3 scale = Vector3.one;
        this.direction = direction;

        if(direction == Direction.LEFT)
        {
                scale.x = 1;
            transform.GetChild(0).localScale = new Vector3(Mathf.Abs(transform.GetChild(0).localScale.x) * scale.x, transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.z);
        }
        else if(direction == Direction.RIGHT)
        {
                scale.x = -1;
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
        Spine.TrackEntry trackEntry = new Spine.TrackEntry();
        trackEntry = spineAnimation.skeletonAnimation.AnimationState.Tracks.ElementAt(0);
        float normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd;

        if (transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName != skinName + "/knockdown")
        {
            spineAnimation.PlayAnimation(skinName + "/knockdown", false, 1);
        }

        if(transform.GetChild(0).GetComponent<SkeletonAnimation>().AnimationName == skinName + "/knockdown" && normalizedTime >= 1)
        {
            Destroy(gameObject);
        }

        yield return null;
    }
}
