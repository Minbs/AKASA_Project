using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


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


    public int atk;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Deal(int damage)
    {
        currentHp -= damage;
        StartCoroutine(ChangeUnitColor(Color.red, 0.2f));
    }

    public void SetDirection(Direction direction)
    {
        Vector3 scale = transform.localScale;
        this.direction = direction;

        if(direction == Direction.LEFT)
        {
            if (scale.x < 0)
                scale.x *= -1;
        }
        else if(direction == Direction.RIGHT)
        {
            if (scale.x > 0)
                scale.x *= -1;
        }

        transform.localScale = scale;
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


}
