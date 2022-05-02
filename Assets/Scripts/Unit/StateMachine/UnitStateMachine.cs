using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class UnitStateMachine : MonoBehaviour
{
    public UnitBaseState currentState { get; set; }
    private UnitBaseState prevState;

    public UnitIdleState idleState = new UnitIdleState(); //대기 상태
    public  UnitMoveState moveState = new UnitMoveState(); //행동선택
    public  UnitApproachingState approachingState = new UnitApproachingState(); //명령 대기 상태
    public  UnitAttackState AttackState = new UnitAttackState();

    public Unit unit { get; set; }
    public NavMeshAgent agent { get; set; }
   

    private void Awake()
    {
        currentState = idleState;
        unit = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (unit.currentHp <= 0)
        {
            agent.isStopped = true;
            StartCoroutine(unit.Die());
            return;
        }

        currentState.Update(this);
    }

    public void ChangeState (UnitBaseState state)
    {
        currentState.End(this);
        prevState = currentState;
        currentState = state;
        currentState.Begin(this);
        //Debug.Log(currentState);
    }

    public void MoveToDirection(Direction direction)
    {
        int desX = 1;

        switch(direction)
        {
            case Direction.LEFT:
                desX = -10;
                break;
            case Direction.RIGHT:
                desX = 10;
                break;
        }

        agent.SetDestination(new Vector3(desX, transform.position.y,transform.position.z));
    }

    /// <summary>
    /// 인지 범위 안에 있는 적을 타겟으로 설정
    /// </summary>
    public void SetTargetInCognitiveRange(List<GameObject> targetsList)
    {
        if (targetsList.Count <= 0)
            return;

        GameObject target = null;

        foreach(var e in targetsList)
        {
            if(Mathf.Abs( Vector3.Distance(transform.position, e.transform.position)) < unit.cognitiveRangeDistance) // 인지 범위 안에 있는지 확인
            {
                if (target == null)
                    target = e;

                if (Mathf.Abs(Vector3.Distance(transform.position, e.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, target.transform.position))) // 더 가까운 적이 있는지 확인
                    target = e;
            }
        }

        unit.target = target;
    }

    public bool IsTargetInAttackRange() => Mathf.Abs(Vector3.Distance(transform.position, unit.target.transform.position)) <= unit.attackRangeDistance; // 공격 범위 안에 있는지 확인

    public bool IsTargetInCognitiveRange() => Mathf.Abs(Vector3.Distance(transform.position, unit.target.transform.position)) <= unit.cognitiveRangeDistance; // 공격 범위 안에 있는지 확인

    public void LookAtTarget(Vector3 targetPos)
    {
        Vector3 scale = transform.GetChild(0).localScale;

       if ( transform.position.x < targetPos.x )
        {
            scale.x = Mathf.Abs(transform.GetChild(0).localScale.x) * -1; 
        }
        else if (transform.position.x > targetPos.x)
        {
            scale.x = Mathf.Abs(transform.GetChild(0).localScale.x) * 1;
        }

        transform.GetChild(0).localScale = scale;
    }
    
}
