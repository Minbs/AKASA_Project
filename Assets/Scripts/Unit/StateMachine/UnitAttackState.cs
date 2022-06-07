using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    public override void Begin(UnitStateMachine stateMachine)
    {
        stateMachine.agent.isStopped = true;
        stateMachine.agent.velocity = Vector3.zero;
    }

    public override void Update(UnitStateMachine stateMachine)
    {
        if (stateMachine.unit.isAnimationPlaying("/attack"))
            return;


            if (stateMachine.unit.target == null || !stateMachine.unit.target.activeSelf)
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
                stateMachine.unit.spineAnimation.PlayAnimation(stateMachine.unit.skinName + "/attack", false, stateMachine.unit.attackSpeed * GameManager.Instance.gameSpeed);
            }
        }
    }

    public override void End(UnitStateMachine stateMachine)
    {
        stateMachine.agent.isStopped = false;
    }
}
