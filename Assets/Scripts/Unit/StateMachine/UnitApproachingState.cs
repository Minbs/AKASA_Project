using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitApproachingState : UnitBaseState
{
    public override void Begin(UnitStateMachine stateMachine)
    {

    }

    public override void Update(UnitStateMachine stateMachine)
    {
        if (stateMachine.unit.spineAnimation.skeletonAnimation.AnimationName != stateMachine.unit.skinName + "/move")
            stateMachine.unit.spineAnimation.PlayAnimation(stateMachine.unit.skinName + "/move", true, 1);

        stateMachine.agent.SetDestination(stateMachine.unit.target.transform.position);


        if (stateMachine.unit.target == null)
            stateMachine.ChangeState(stateMachine.moveState);
        else
        {
            stateMachine.LookAtTarget(stateMachine.unit.target.transform.position);

            if (stateMachine.IsTargetInAttackRange())
                stateMachine.ChangeState(stateMachine.AttackState);
            if (!stateMachine.IsTargetInCognitiveRange())
                stateMachine.unit.target = null;
        }
    }

    public override void End(UnitStateMachine stateMachine)
    {

    }
}
