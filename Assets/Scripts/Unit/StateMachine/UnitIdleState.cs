using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : UnitBaseState
{
    public override void Begin(UnitStateMachine stateMachine)
    {
        stateMachine.agent.isStopped = true;
        stateMachine.LookAtTarget(new Vector3(10, 0, 0));
    }

    public override void Update(UnitStateMachine stateMachine)
    {
            if (!stateMachine.unit.isAnimationPlaying("/idle") && stateMachine.unit.GetComponent<Minion>() != null)
                stateMachine.unit.spineAnimation.PlayAnimation(stateMachine.unit.skinName + "/idle", true, GameManager.Instance.gameSpeed);       
    }

    public override void End(UnitStateMachine stateMachine)
    {
        stateMachine.agent.isStopped = false;
    }
}
