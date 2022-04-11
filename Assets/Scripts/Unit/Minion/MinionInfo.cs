using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;

[Serializable]
public class MinionInfo
{
    public List<Node> attackRangeNodesList;
    public SkeletonDataAsset skeletonData;
}
