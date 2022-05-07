using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : UnitBaseState
{
    public override void Begin(UnitStateMachine stateMachine)
    {
        stateMachine.agent.isStopped = true;
    }

    public override void Update(UnitStateMachine stateMachine)
    {
            if (!stateMachine.unit.isAnimationPlaying("/idle") && stateMachine.unit.GetComponent<Minion>() != null)
                stateMachine.unit.spineAnimation.PlayAnimation(stateMachine.unit.skinName + "/idle", true, 1);       
    }

    public override void End(UnitStateMachine stateMachine)
    {
        stateMachine.agent.isStopped = false;
    }
}
