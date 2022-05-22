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
        if (stateMachine.gameObject.GetComponent<Enemy>())
            stateMachine.MoveToDirection(stateMachine.unit.direction);
        else if(stateMachine.gameObject.GetComponent<Minion>() && stateMachine.unit.target == null)
            stateMachine.ReturnToTilePosition();

        if (stateMachine.unit.spineAnimation.skeletonAnimation.AnimationName != stateMachine.unit.skinName + "/move")
            stateMachine.unit.spineAnimation.PlayAnimation(stateMachine.unit.skinName + "/move", true, GameManager.Instance.gameSpeed);

        if (GameManager.Instance.state == State.BATTLE)
            BattleMove(stateMachine);
        else if (GameManager.Instance.state == State.WAVE_END)
            stateMachine.ReturnToTilePosition();
    }

    public override void End(UnitStateMachine stateMachine)
    {

    }

    public void BattleMove(UnitStateMachine stateMachine)
    {
        if (stateMachine.gameObject.GetComponent<Minion>())
        {
            if (stateMachine.gameObject.GetComponent<Minion>().minionClass == MinionClass.Rescue)
                stateMachine.SetTargetInCognitiveRange(GameManager.Instance.minionsList);
            else
                stateMachine.SetTargetInCognitiveRange(GameManager.Instance.enemiesList);
        }
        else if (stateMachine.gameObject.GetComponent<Enemy>())
            stateMachine.SetTargetInCognitiveRange(GameManager.Instance.minionsList);

        if (stateMachine.unit.target != null)
            stateMachine.ChangeState(stateMachine.approachingState);
    }
}
