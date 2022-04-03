using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimation : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] animClip;
    public float speed;

    [SpineSlot]
    public string Slot;
    [SpineSkin]
    public string Skin;


    private void Awake() => skeletonAnimation = GetComponent<SkeletonAnimation>();

    

    public void PlayAnimation(string animName, bool loop, float timeScale) // 애니메이션 재생
    {


        skeletonAnimation.AnimationState.SetAnimation(0, animName, loop);
        skeletonAnimation.state.TimeScale = timeScale;


    }

    public void AddAnimation(string animName, bool loop, float timeScale, float startDelay) // 애니메이션 추가
    {
        skeletonAnimation.AnimationState.AddAnimation(0, animName, loop, startDelay);
        skeletonAnimation.state.TimeScale = timeScale;

    }

    public void SetSpineSkin(string skinName) // 전체 스킨 교체
    {
        skeletonAnimation.skeleton.SetSkin(skinName);
        skeletonAnimation.skeleton.SetSlotsToSetupPose();

    }

    public void SetSpineAttachment(string _slot, string _attachment)
    {
        skeletonAnimation.skeleton.SetAttachment(_slot, _attachment);
    }


}
