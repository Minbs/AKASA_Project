using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveState : UnitBaseState
{
    public override void Begin(UnitStateMachine stateMachine)
    {

    }

    public override void Update(UnitStateMachine stateMachine)
    {
        stateMachine.MoveToDirection(stateMachine.unit.direction);

        if (stateMachine.unit.spineAnimation.skeletonAnimation.AnimationName != stateMachine.unit.skinName + "/move")
            stateMachine.unit.spineAnimation.PlayAnimation(stateMachine.unit.skinName + "/move", true, 1);

        if(stateMachine.gameObject.GetComponent<Minion>())
        stateMachine.SetTargetInCognitiveRange(GameManager.Instance.enemiesList);
        else if(stateMachine.gameObject.GetComponent<Enemy>())
            stateMachine.SetTargetInCognitiveRange(GameManager.Instance.minionsList);

        if (stateMachine.unit.target != null)
            stateMachine.ChangeState(stateMachine.approachingState);
    }

    public override void End(UnitStateMachine stateMachine)
    {

    }


}
