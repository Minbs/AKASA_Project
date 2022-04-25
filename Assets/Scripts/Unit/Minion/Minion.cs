using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinionClass
{
    //근거리 클래스
    Buster,
    Paladin,
    Guardian,
    Assassin,

    //원거리 클래스
    Chaser,
    Mage,
    Rescue,
    TacticalSupport
}

public class Minion : Unit
{
    public MinionClass minionClass;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
