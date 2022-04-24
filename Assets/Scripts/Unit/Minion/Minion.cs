using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinionClass
{
    Buster,
    Paladin,
    Guardian,
    Assassin,
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
    void Update()
    {
        
    }
}
