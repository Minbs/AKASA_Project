using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillPerformState : UnitBaseState
{
    public override void Begin(UnitStateMachine stateMachine)
    {
        stateMachine.agent.isStopped = true;
        stateMachine.unit.spineAnimation.PlayAnimation(stateMachine.unit.skinName + "/skill1", false, GameManager.Instance.gameSpeed);
    }

    public override void Update(UnitStateMachine stateMachine)
    {
        if (stateMachine.unit.normalizedTime >= 1)
            stateMachine.ChangeState(stateMachine.moveState);
    }

    public override void End(UnitStateMachine stateMachine)
    {
        stateMachine.agent.isStopped = false;
    }
}
