using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitApproachingState : UnitBaseState
{
    public override void Begin(UnitStateMachine stateMachine)
    {
        stateMachine.agent.SetDestination(stateMachine.unit.target.transform.position);
    }

    public override void Update(UnitStateMachine stateMachine)
    {
        if (stateMachine.unit.spineAnimation.skeletonAnimation.AnimationName != stateMachine.unit.skinName + "/move")
            stateMachine.unit.spineAnimation.PlayAnimation(stateMachine.unit.skinName + "/move", true, 1);

        if (stateMachine.unit.target == null)
            stateMachine.ChangeState(stateMachine.moveState);
        else
        {
            if (stateMachine.IsTargetInAttackRange())
                stateMachine.ChangeState(stateMachine.AttackState);
        }
    }

    public override void End(UnitStateMachine stateMachine)
    {

    }
}
