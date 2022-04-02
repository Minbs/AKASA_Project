using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
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
    public int hp;

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

        transform.GetChild(0).GetComponent<SkeletonAnimation>().state.Event += AnimationSatateOnEvent;

        skeletonData = transform.GetChild(0).GetComponent<SkeletonAnimation>().skeletonDataAsset;
        
        transform.GetChild(0).GetComponent<SkeletonAnimation>().Initialize(true);
        //  UIManager.Instance.settingCharacter.GetComponent<SkeletonGraphic>().Initialize(true);

        skinName = transform.GetChild(0).GetComponent<SkeletonAnimation>().initialSkinName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AnimationSatateOnEvent(TrackEntry trackEntry, Event e)
    {
        if (e.Data.Name == "shoot")
        {
            Deal();
        }
    }

    public void Deal()
    {
        target.GetComponent<Unit>().hp -= 10;
    }
}
