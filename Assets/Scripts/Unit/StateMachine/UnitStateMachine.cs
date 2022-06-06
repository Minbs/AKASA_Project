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

    public bool isDeploying { get; set; }

    private void Awake()
    {
        currentState = idleState;
        unit = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();

        if (GetComponent<Minion>())
            isDeploying = true;
        else if (GetComponent<Enemy>())
            LookAtTarget(new Vector3(-10, 0, 0));
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeploying)
            return;

        if (unit.currentHp <= 0)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            StartCoroutine(unit.Die());
            return;
        }

        agent.speed = speed * GameManager.Instance.gameSpeed;

        currentState.Update(this);
    }

    public void ChangeState(UnitBaseState state)
    {
        currentState.End(this);
        prevState = currentState;
        currentState = state;
        currentState.Begin(this);

        if(GetComponent<Minion>()
            && GetComponent<Minion>().minionClass == MinionClass.Rescue)
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

    public void Initialize()
    {
        unit.currentHp = unit.maxHp;
        unit.UpdateHealthbar();
        ChangeState(idleState);
    }

    /// <summary>
    /// 인지 범위 안에 있는 적을 타겟으로 설정
    /// </summary>
    public void SetTargetInCognitiveRange()
    {
        if (unit.target
            && !unit.target.activeSelf
            && unit.target.GetComponent<Object>().currentHp <= 0)
            unit.target = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, unit.cognitiveRangeDistance, LayerMask.GetMask("Object"));

        if (colliders.Length <= 0)
            return;

        if (GetComponent<Minion>())
        {
            foreach (var col in colliders)
            {
                GameObject obj = col.transform.parent.gameObject;


                if (GetComponent<Minion>().minionClass != MinionClass.Rescue)
                {
                    if (!obj.transform.tag.Equals("Enemy")
                    || obj.GetComponent<Object>().currentHp <= 0
                    || !obj.activeSelf)
                        continue;

                    if (!unit.target)
                    {
                        unit.target = obj;
                    }
                    else
                    {
                        if (Mathf.Abs(Vector3.Distance(transform.position, obj.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, unit.target.transform.position)))
                        {
                            unit.target = obj;
                        }
                    }
                }
                else
                {
                    if (!obj.GetComponent<Minion>()
|| obj.GetComponent<Object>().currentHp <= 0
|| obj.GetComponent<Object>().currentHp >= obj.GetComponent<Object>().maxHp
|| !obj.activeSelf)
                        continue;

                    if (!unit.target)
                    {
                        unit.target = obj;
                    }
                    else
                    {
                        if (Mathf.Abs(Vector3.Distance(transform.position, obj.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, unit.target.transform.position)))
                        {
                            unit.target = obj;
                        }
                    }
                }

                
            }
        }
        else if (GetComponent<Enemy>())
        {
            foreach (var col in colliders)
            {
                GameObject obj = col.transform.parent.gameObject;

                if (!obj.transform.tag.Equals("Ally")
                    || obj.GetComponent<Object>().currentHp <= 0)
                    continue;

                if (!unit.target)
                {
                    unit.target = obj;
                }
                else
                {
                    if (Mathf.Abs(Vector3.Distance(transform.position, obj.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, unit.target.transform.position)))
                    {
                        unit.target = obj;
                    }
                }
            }
        }

      
    }

    /// <summary>
    /// 자신 또는 범위 내의 아군을 공격한 대상을 타겟으로설정
    /// </summary>
    public void SetAttackTargetInRange(GameObject attackEnemy)
    {
        float targetSetRange = 3.0f;

        foreach (var e in GameManager.Instance.minionsList)
        {
            if (e.GetComponent<Unit>().currentHp <= 0
            || e.GetComponent<Minion>().minionClass == MinionClass.Rescue
            || (e.GetComponent<UnitStateMachine>().currentState.GetType().ToString() != e.GetComponent<UnitStateMachine>().idleState.GetType().ToString()
            && e.GetComponent<UnitStateMachine>().currentState.GetType().ToString() != e.GetComponent<UnitStateMachine>().moveState.GetType().ToString()))
            {
                continue;
            }

            // Debug.Log(Mathf.Abs(Vector3.Distance(transform.position, e.transform.position)));

            if (Mathf.Abs(Vector3.Distance(transform.position, e.transform.position)) <= targetSetRange) // 인지 범위 안에 있는지 확인
            {
                e.GetComponent<Unit>().target = attackEnemy;
                e.GetComponent<UnitStateMachine>().ChangeState(approachingState);
            }
        }
    }

    public bool IsTargetInAttackRange()  // 공격, 힐 범위 안에 있는지 확인
    {
        if (unit.target.GetComponent<Object>().currentHp <= 0
            && !unit.target.activeSelf)
        {
            unit.target = null;
            return false;
        }

        
        if (GetComponent<Minion>()
            &&GetComponent<Minion>().minionClass == MinionClass.Rescue )
        {
            if (unit.target == GameManager.Instance.turret
                || unit.target.GetComponent<Unit>().currentHp >= unit.target.GetComponent<Unit>().maxHp)
            {
                unit.target = null;
                return false;
            }
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, unit.attackRangeDistance, LayerMask.GetMask("Object"));

        foreach(var col in colliders)
        {
            GameObject obj = col.transform.parent.gameObject;

            if (obj.Equals(unit.target.gameObject)
                && obj.GetComponent<Object>().currentHp > 0)
                return true;
        }
        return false;//Mathf.Abs(Vector3.Distance(transform.position, unit.target.transform.position)) <= unit.attackRangeDistance;
    }

    public bool IsTargetInCognitiveRange() // 인지 범위 안에 있는지 확인
    {

        if (unit.target.GetComponent<Object>().currentHp <= 0
            && !unit.target.activeSelf)
        {
            unit.target = null;
            return false;
        }

        if (unit.GetComponent<Minion>() != null
    && unit.GetComponent<Minion>().minionClass == MinionClass.Rescue)
        {
            if (unit.target.GetComponent<Unit>().currentHp >= unit.target.GetComponent<Unit>().maxHp)
            {
                
                    unit.target = null;
                    return false;
            }
        }

        return Mathf.Abs(Vector3.Distance(transform.position, unit.target.transform.position)) <= unit.cognitiveRangeDistance;
    }


    public void LookAtTarget(Vector3 targetPos)
    {
        Vector3 scale = transform.GetChild(0).localScale;
        Vector3 prevScale = scale;


        if (transform.position.x < targetPos.x)
        {
            scale.x = Mathf.Abs(transform.GetChild(0).localScale.x) * 1;
        }
        else if (transform.position.x > targetPos.x)
        {
            scale.x = Mathf.Abs(transform.GetChild(0).localScale.x) * -1;
        }

        transform.GetChild(0).localScale = scale;

        if (!prevScale.Equals(scale))
            agent.velocity = Vector3.zero;
    }

    public void ReturnToTilePosition()
    {
        agent.SetDestination(unit.onTile.transform.position);
        LookAtTarget(unit.onTile.transform.position);

        Debug.Log(Vector3.Distance(unit.transform.position, unit.onTile.transform.position));

        if (Vector3.Distance(unit.transform.position, unit.onTile.transform.position) < 0.15)
        {
            unit.transform.position = unit.onTile.transform.position;
            ChangeState(idleState);
        }
    }
}
