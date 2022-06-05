using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    Ray ray;
    RaycastHit hit;

    public float skillAimTimeSpeed;

    private GameObject skillUnit; // 스킬을 사용중인 유닛

    public bool isSkillActing = false;

    // Start is called before the first frame update
    void Start()
    {
        skillUnit = null;
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    public void UseCharacterSkill(int index)
    {
        string minionName;

        skillUnit = GameManager.Instance.minionsList[index];

        minionName = skillUnit.GetComponent<DefenceMinion>().Unitname;

        isSkillActing = true;
        Debug.Log(minionName + "Skill");

        StartCoroutine(minionName + "Skill");
    }


    IEnumerator HwaseonSkill()
    {
        Debug.Log("hwaseon");

        BattleUIManager.Instance.skillAimUI.SetActive(true);
        GameManager.Instance.SetGameSpeed(skillAimTimeSpeed);

        var targetsList = GameManager.Instance.minionsList;


        GameObject skillTarget;

        foreach (var minion in targetsList)
        {
            if (minion.Equals(skillUnit))
                continue;

            minion.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
        }



        while (true)
        {
            foreach (var minion in targetsList)
            {
                if (minion.GetComponent<Object>().currentHp <= 0)
                {
                    minion.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
                }
            }



            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Object")))
            {
                if (Input.GetMouseButtonDown(0)
                    && hit.transform.GetComponent<Minion>()
                    && hit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder.Equals(1))
                {
                    skillTarget = hit.transform.gameObject;
                    Debug.Log(hit.transform.name);
                    break;
                }
            }

            yield return null;
        }

        foreach (var minion in targetsList)
        {
            if (minion.Equals(skillTarget))
                continue;

            minion.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;

        }

        GameManager.Instance.SetGameSpeed(0);
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);
        skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill", false, 1);

        Vector3 startPos = skillUnit.transform.position;
        //  skillUnit.GetComponent<Unit>().spineAnimation.skeletonAnimation.AnimationState.TimeScale = 1;

        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            yield return null;
        }


        skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill2", false, 1);
        skillUnit.transform.position = skillTarget.transform.position;
        yield return null;

        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            Debug.Log("2");
            yield return null;
        }



        skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill3", false, 1);
        yield return null;

        skillUnit.transform.position = startPos;

        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {

            yield return null;
        }

        BattleUIManager.Instance.skillAimUI.SetActive(false);
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
        GameManager.Instance.SetGameSpeed(1);
        Debug.Log("연출 끝");
    }

    IEnumerator VeritySkill()
    {
        Debug.Log("verity");

        BattleUIManager.Instance.skillAimUI.SetActive(true);
        GameManager.Instance.SetGameSpeed(skillAimTimeSpeed);

        var  targetsList = GameManager.Instance.enemiesList;



        foreach (var enemy in targetsList)
        {
            enemy.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
        }



        while (true)
        {
            foreach (var enemy in targetsList)
            {
                if (enemy.GetComponent<Object>().currentHp <= 0)
                {
                    enemy.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
                }
            }



            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Object")))
            {
                if (Input.GetMouseButtonDown(0)
                    && hit.transform.tag.Equals("Enemy")
                    && hit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder.Equals(1))
                {
                    Debug.Log(hit.transform.name);
                    break;
                }
            }

            yield return null;
        }

        GameManager.Instance.SetGameSpeed(0);
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);

        skillUnit.GetComponent<Unit>().spineAnimation.skeletonAnimation.AnimationState.TimeScale = 1;

    }

    IEnumerator VogueSkill()
    {
        Debug.Log("vogue");

        BattleUIManager.Instance.skillAimUI.SetActive(true);
        GameManager.Instance.SetGameSpeed(skillAimTimeSpeed);

        var targetsList = GameManager.Instance.enemiesList;



        foreach (var enemy in targetsList)
        {
            enemy.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
        }

        GameObject skillTarget;

        while (true)
        {
            foreach (var enemy in targetsList)
            {
                if (enemy.GetComponent<Object>().currentHp <= 0)
                {
                    enemy.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
                }
            }



            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Object")))
            {
                if (Input.GetMouseButtonDown(0)
                    && hit.transform.tag.Equals("Enemy")
                    && hit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder.Equals(1))
                {
                    skillTarget = hit.transform.gameObject;
                    Debug.Log(hit.transform.name);
                    break;
                }
            }

            yield return null;
        }

        GameManager.Instance.SetGameSpeed(0);
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);

        skillUnit.GetComponent<Unit>().spineAnimation.skeletonAnimation.AnimationState.TimeScale = 1;

        Vector3 startPos = skillUnit.transform.position;

        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            skillUnit.transform.position = Vector3.Lerp(startPos, skillTarget.transform.position, skillUnit.GetComponent<Unit>().normalizedTime);


            yield return null;
        }

        BattleUIManager.Instance.skillAimUI.SetActive(false);
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
        GameManager.Instance.SetGameSpeed(1);
    }
    IEnumerator WraithSkill()
    {
        BattleUIManager.Instance.skillAimUI.SetActive(true);
        GameManager.Instance.SetGameSpeed(skillAimTimeSpeed);

        yield return null;
    }

    IEnumerator IsabellaSkill()
    {
        BattleUIManager.Instance.skillAimUI.SetActive(true);
        GameManager.Instance.SetGameSpeed(skillAimTimeSpeed);

        yield return null;
    }

    IEnumerator ZippoSkill()
    {
        BattleUIManager.Instance.skillAimUI.SetActive(true);
        GameManager.Instance.SetGameSpeed(skillAimTimeSpeed);

        yield return null;
    }
}
