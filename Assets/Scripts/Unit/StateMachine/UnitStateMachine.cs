using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class UnitStateMachine : MonoBehaviour
{
    public UnitBaseState currentState { get; set; }
    private UnitBaseState prevState;

    public UnitIdleState idleState = new UnitIdleState(); //��� ����
    public UnitMoveState moveState = new UnitMoveState(); //�ൿ����
    public UnitApproachingState approachingState = new UnitApproachingState(); //��� ��� ����
    public UnitAttackState AttackState = new UnitAttackState();

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

    public void ChangeState(UnitBaseState state)
    {
        currentState.End(this);
        prevState = currentState;
        currentState = state;
        currentState.Begin(this);
        Debug.Log(currentState);
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

    /// <summary>
    /// ���� ���� �ȿ� �ִ� ���� Ÿ������ ����
    /// </summary>
    public void SetTargetInCognitiveRange(List<GameObject> targetsList)
    {
        if (targetsList.Count <= 0)
            return;

        GameObject target = null;

        foreach (var e in targetsList)
        {
            if (e.GetComponent<Unit>().currentHp <= 0)
                continue;

            if (Mathf.Abs(Vector3.Distance(transform.position, e.transform.position)) < unit.cognitiveRangeDistance) // ���� ���� �ȿ� �ִ��� Ȯ��
            {
                if (unit.GetComponent<Minion>() != null
                    && unit.GetComponent<Minion>().minionClass == MinionClass.Rescue)
                {
                    if (e.GetComponent<Unit>().currentHp < e.GetComponent<Unit>().maxHp) // �ִ� ü�º��� ���� ü���� ������
                    {
                        if (target == null)
                            target = e;

                        if (target.GetComponent<Unit>().currentHp < e.GetComponent<Unit>().currentHp) // �ش� �Ʊ��� ���� Ÿ�ٺ��� ü���� ������
                            target = e;
                    }


                }
                else
                {
                    if (target == null)
                        target = e;

                    if (Mathf.Abs(Vector3.Distance(transform.position, e.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, target.transform.position))) // �� ����� ���� �ִ��� Ȯ��
                        target = e;
                }
            }
        }

        unit.target = target;
    }

    public bool IsTargetInAttackRange()  // ����, �� ���� �ȿ� �ִ��� Ȯ��
    {
        if (unit.GetComponent<Minion>() != null
        && unit.GetComponent<Minion>().minionClass == MinionClass.Rescue)
        {
            if (unit.target.GetComponent<Unit>().currentHp >= unit.target.GetComponent<Unit>().maxHp)
                return false;
        }

        return Mathf.Abs(Vector3.Distance(transform.position, unit.target.transform.position)) <= unit.attackRangeDistance;
    }

    public bool IsTargetInCognitiveRange() // ���� ���� �ȿ� �ִ��� Ȯ��
    {
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
