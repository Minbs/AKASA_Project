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
    public int maxHp;
    public int currentHp;
    public Image healthBar;

    public int atk;
    public int def;

    public float attackSpeed = 1;

    public string poolItemName{ get; set; }

    public Direction direction { get; set; }

    public SpineAnimation spineAnimation { get; set; }

    public GameObject target { get; set; }

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


    public void Deal(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);


        if (damage < 0) //heal
        {
            StartCoroutine(ChangeUnitColor(Color.green, 0.2f));
        }
        else
        {
        StartCoroutine(ChangeUnitColor(Color.red, 0.2f));
        }

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
        }
        else if(direction == Direction.RIGHT)
        {
                scale.x = -1;
        }

        transform.GetChild(0).localScale = new Vector3(Mathf.Abs(transform.GetChild(0).localScale.x) * scale.x, transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.z);
    }

    public IEnumerator ChangeUnitColor(Color color, float duration)
    {
        float timer = 0f;
    

        transform.GetChild(0).GetComponent<SkeletonAnimation>().skeleton.SetColor(color);


        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
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
