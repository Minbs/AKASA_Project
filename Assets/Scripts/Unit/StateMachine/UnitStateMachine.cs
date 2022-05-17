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
    public UnitMoveState moveState = new UnitMoveState(); //행동선택
    public UnitApproachingState approachingState = new UnitApproachingState(); //명령 대기 상태
    public UnitAttackState AttackState = new UnitAttackState();
    public UnitSkillPerformState SkillPerformState = new UnitSkillPerformState();

    public Unit unit { get; set; }
    public NavMeshAgent agent { get; set; }

    private float speed;

    private void Awake()
    {
        currentState = idleState;
        unit = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = agent.speed;
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

        agent.speed = speed  * GameManager.Instance.gameSpeed;

        currentState.Update(this);
    }

    public void ChangeState(UnitBaseState state)
    {
        currentState.End(this);
        prevState = currentState;
        currentState = state;
        currentState.Begin(this);
//        Debug.Log(currentState);
    }

    public void MoveToDirection(Direction direction)
    {
        int desX = 1;

        switch (direction)
        {
            case Direction.LEFT:
                desX = -10;
                break;
            case Direction.RIGHT:
                desX = 10;
                break;
        }

        agent.SetDestination(new Vector3(desX, transform.position.y, transform.position.z));
    }

    public void Initialize()
    {
        unit.currentHp = unit.maxHp;
        unit.UpdateHealthbar();
        ChangeState(idleState);
    }

    /// <summary>
    /// 인지 범위 안에 있는 적을 타겟으로 설정
    /// </summary>
    public void SetTargetInCognitiveRange(List<GameObject> targetsList)
    {
        if (targetsList.Count <= 0)
            return;

        GameObject target = null;

        foreach (var e in targetsList)
        {
            if (e.GetComponent<Unit>().currentHp <= 0)
            {
                Debug.Log(e.gameObject.name);
                continue;
            }

            if (Mathf.Abs(Vector3.Distance(transform.position, e.transform.position)) < unit.cognitiveRangeDistance) // 인지 범위 안에 있는지 확인
            {
                if (unit.GetComponent<Minion>() != null
                    && unit.GetComponent<Minion>().minionClass == MinionClass.Rescue)
                {
                    if (e.GetComponent<Unit>().currentHp < e.GetComponent<Unit>().maxHp) // 최대 체력보다 현재 체력이 낮으면
                    {
                        if (target == null)
                            target = e;

                        if (target.GetComponent<Unit>().currentHp < e.GetComponent<Unit>().currentHp) // 해당 아군이 현재 타겟보다 체력이 낮으면
                            target = e;
                    }


                }
                else
                {
                    if (target == null)
                        target = e;

                    if (Mathf.Abs(Vector3.Distance(transform.position, e.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, target.transform.position))) // 더 가까운 적이 있는지 확인
                        target = e;
                }
            }
        }

        unit.target = target;
    } //범위 모양, 길이, 우선 순위 넣어서 만들기

    public bool IsTargetInAttackRange()  // 공격, 힐 범위 안에 있는지 확인
    {
        if (unit.target.GetComponent<Unit>().currentHp <= 0)
            return false;

        if (unit.GetComponent<Minion>() != null
        && unit.GetComponent<Minion>().minionClass == MinionClass.Rescue)
        {
            if (unit.target.GetComponent<Unit>().currentHp >= unit.target.GetComponent<Unit>().maxHp)
                return false;
        }

        return Mathf.Abs(Vector3.Distance(transform.position, unit.target.transform.position)) <= unit.attackRangeDistance;
    }

    public bool IsTargetInCognitiveRange() // 인지 범위 안에 있는지 확인
    {    
            if (unit.target.GetComponent<Unit>().currentHp <= 0)
                return false;

            if (unit.GetComponent<Minion>() != null
        && unit.GetComponent<Minion>().minionClass == MinionClass.Rescue)
        {
            if (unit.target.GetComponent<Unit>().currentHp >= unit.target.GetComponent<Unit>().maxHp)
                return false;
        }

        return Mathf.Abs(Vector3.Distance(transform.position, unit.target.transform.position)) <= unit.cognitiveRangeDistance;
    }

    public void LookAtTarget(Vector3 targetPos)
    {
        Vector3 scale = transform.GetChild(0).localScale;

        if (transform.position.x < targetPos.x)
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
