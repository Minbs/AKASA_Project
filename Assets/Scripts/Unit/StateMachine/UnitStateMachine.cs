using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class UnitStateMachine : MonoBehaviour
{
    public UnitBaseState currentState { get; set; }
    private UnitBaseState prevState;

    public  UnitMoveState moveState = new UnitMoveState(); //�ൿ����
    public  UnitApproachingState approachingState = new UnitApproachingState(); //��� ��� ����
    public  UnitAttackState AttackState = new UnitAttackState();

    public Unit unit { get; set; }
    public NavMeshAgent agent { get; set; }
   

    private void Awake()
    {
        currentState = moveState;
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
    /// ���� ���� �ȿ� �ִ� ���� Ÿ������ ����
    /// </summary>
    public void SetTargetInCognitiveRange(List<GameObject> targetsList)
    {
        if (targetsList.Count <= 0)
            return;

        GameObject target = null;

        foreach(var e in targetsList)
        {
            if(Mathf.Abs( Vector3.Distance(transform.position, e.transform.position)) < unit.cognitiveRangeDistance) // ���� ���� �ȿ� �ִ��� Ȯ��
            {
                if (target == null)
                    target = e;

                if (Mathf.Abs(Vector3.Distance(transform.position, e.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, target.transform.position))) // �� ����� ���� �ִ��� Ȯ��
                    target = e;
            }
        }

        unit.target = target;
    }

    public bool IsTargetInAttackRange() => Mathf.Abs(Vector3.Distance(transform.position, unit.target.transform.position)) <= unit.attackRangeDistance; // ���� ���� �ȿ� �ִ��� Ȯ��
    
}
