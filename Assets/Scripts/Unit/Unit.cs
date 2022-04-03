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

    public string poolItemName;

    public Direction direction;

    public SpineAnimation spineAnimation;

    public GameObject target;

    public SkeletonDataAsset skeletonData;

    public string skinName;
    protected virtual void Start()
    {
        if (transform.GetChild(0).GetComponent<SpineAnimation>() == null)
        {
            transform.GetChild(0).gameObject.AddComponent<SpineAnimation>();
        }

        spineAnimation = transform.GetChild(0).GetComponent<SpineAnimation>();



        skeletonData = transform.GetChild(0).GetComponent<SkeletonAnimation>().skeletonDataAsset;
        
        transform.GetChild(0).GetComponent<SkeletonAnimation>().Initialize(true);
        //  UIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().Initialize(true);

        skinName = transform.GetChild(0).GetComponent<SkeletonAnimation>().initialSkinName;
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
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


}
