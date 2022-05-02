using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    public override void Begin(UnitStateMachine stateMachine)
    {
        stateMachine.agent.isStopped = true;
    }

    public override void Update(UnitStateMachine stateMachine)
    {
        if (stateMachine.unit.target == null)
            stateMachine.ChangeState(stateMachine.moveState);
        else
        {
            if (!stateMachine.IsTargetInAttackRange())
            {
                stateMachine.unit.target = null;
                return;
            }

            if (!stateMachine.unit.isAnimationPlaying("/attack"))
            {
                stateMachine.LookAtTarget(stateMachine.unit.target.transform.position);
                stateMachine.unit.spineAnimation.PlayAnimation(stateMachine.unit.skinName + "/attack", false, 1);
            }
        }
    }

    public override void End(UnitStateMachine stateMachine)
    {
        stateMachine.agent.isStopped = false;
    }
}
