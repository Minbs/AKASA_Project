using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spine.Unity;
enum SkilRangeType
{
    Circle
}

enum SkillAimType
{
    Single,
    Circle
}

public class SkillManager : Singleton<SkillManager>
{
    Ray ray;
    RaycastHit hit;

    public float skillAimTimeSpeed;

    private GameObject skillUnit; // ��ų�� ������� ����

    public bool isSkillActing = false;

    // ��ų UI ����
    public GameObject skillBackgroundImage;
    public GameObject skillCircleRangeUI;

    public GameObject skillAimCircleRangeUI;


    public GameObject poisonMist;

    private Vector3 skillHitpos;

    private bool isSkillAimEnd = false;


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
        if (index > GameManager.Instance.minionsList.Count - 1 || skillBackgroundImage.activeSelf)
            return;


        string minionName;

        skillUnit = GameManager.Instance.minionsList[index];

        minionName = skillUnit.GetComponent<DefenceMinion>().Unitname;
        isSkillActing = true;
        skillBackgroundImage.SetActive(true);
        Debug.Log(minionName + "Skill");

        StartCoroutine(minionName + "Skill");
    }

    private List<GameObject> AimSkillTargetsInRange(SkilRangeType skilRangeType, SkillAimType skillAimType, string unitType = "Minion", float range = 0, float range2 = 0)
    {
        List<GameObject> targetsList = null;
        GameManager.Instance.SetGameSpeed(skillAimTimeSpeed);

        List<GameObject> returnTargets = new List<GameObject>();

        if (unitType.Equals("Minion"))
        {
            targetsList = GameManager.Instance.minionsList;
        }
        else if (unitType.Equals("Enemy"))
        {
            targetsList = GameManager.Instance.enemiesList;
        }

        foreach (var target in targetsList)
        {
            target.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
            target.GetComponent<Unit>().SetAimUnitColor(false);
        }

        if (skilRangeType.Equals(SkilRangeType.Circle))
        {
            skillCircleRangeUI.SetActive(true);
            skillCircleRangeUI.transform.localScale = new Vector3(2, 2, 2) * range;
            skillCircleRangeUI.transform.position = skillUnit.transform.position;


            Collider[] colliders = Physics.OverlapSphere(skillUnit.transform.position, range);


                foreach (var target in targetsList)
                {
                    foreach (var col in colliders)
                    {
                        if (!col.transform.parent.GetComponent<Unit>())
                            continue;

                        if (col.transform.parent.gameObject.Equals(target)
                            && col.transform.parent.GetComponent<Object>().currentHp > 0)
                        {
                            col.GetComponent<MeshRenderer>().sortingOrder = 1;
                        }
                    }
                }
        }

        if (skillAimType.Equals(SkillAimType.Single))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Object")))
            {
                if (Input.GetMouseButtonDown(0)
                    && hit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder.Equals(1))
                {
                    Debug.Log(hit.transform.name);
                    returnTargets.Add(hit.transform.gameObject);
                }
            }
        }
        else if (skillAimType.Equals(SkillAimType.Circle))
        {
            skillAimCircleRangeUI.SetActive(true);
            skillAimCircleRangeUI.transform.localScale = new Vector3(2, 2, 2) * range2;


            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default")))
            {
                Vector3 pos1, pos2;
                pos1 = skillUnit.transform.position;
                pos2 = hit.point;

                pos1.y = skillCircleRangeUI.transform.position.y;
                pos2.y = skillCircleRangeUI.transform.position.y;

                var hitPosDir = (pos2 - pos1).normalized;
                float distance = Vector3.Distance(pos2, pos1);
                distance = Mathf.Min(distance, range);

                var hitPos = pos1 + hitPosDir * distance;
                skillAimCircleRangeUI.transform.position = hitPos;


                    Collider[] colliders = Physics.OverlapSphere(skillAimCircleRangeUI.transform.position, range2);
                    foreach (var col in colliders)
                    {
                    if (col.transform.parent.GetComponent<Unit>()
                        && col.GetComponent<MeshRenderer>().sortingOrder.Equals(1))
                    {
                        col.transform.parent.GetComponent<Unit>().SetAimUnitColor(true);

                        if (Input.GetMouseButtonDown(0))
                        {
                            returnTargets.Add(col.transform.parent.gameObject);
                            Debug.Log(col.transform.parent.gameObject);
                       
                        }
                    }
                    }

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("��");
                    isSkillAimEnd = true;
                    skillHitpos = skillAimCircleRangeUI.transform.position;
                    skillCircleRangeUI.SetActive(false);
                    skillAimCircleRangeUI.SetActive(false);
                }

                }

        }



        return returnTargets;
        }

        IEnumerator PaySkill()
        {
            GameManager.Instance.SetGameSpeed(0);
            skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
            skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);
            skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill", false, 1);
            yield return null;
            //Vector3 startPos = skillUnit.transform.position;

            while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
            {
                yield return null;
            }

            skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
            GameManager.Instance.SetGameSpeed(1);
            Debug.Log("���� ��");

        skillBackgroundImage.SetActive(false);
        EffectManager.Instance.InstantiateHomingEffect("pay_effect", skillUnit, 8);
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;

    }

        IEnumerator SophiaSkill()
        {

            GameManager.Instance.SetGameSpeed(0);
            skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
            skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);
            skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill", false, 1);
            yield return null;
            //Vector3 startPos = skillUnit.transform.position;

            while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
            {
                yield return null;
            }

            skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
            GameManager.Instance.SetGameSpeed(1);
            Debug.Log("���� ��");

        skillBackgroundImage.SetActive(false);
        StartCoroutine( skillUnit.GetComponent<Unit>().ChangeStat(skillUnit ,"ats", 1,8));
        EffectManager.Instance.InstantiateHomingEffect("sophia_effect", skillUnit, 8);
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
    }

        IEnumerator HwaseonSkill()
        {
            var targetsList = GameManager.Instance.minionsList;

            List<GameObject> skillTargets = new List<GameObject>();

            while (skillTargets.Count <= 0)
            {
                skillTargets = AimSkillTargetsInRange(SkilRangeType.Circle, SkillAimType.Single, "Minion", 3);
                yield return null;
            }

            foreach (var minion in targetsList)
            {
                minion.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
            }

            foreach (var target in skillTargets)
            {
                target.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
            }

            GameManager.Instance.SetGameSpeed(0);
            skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
            skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);
            skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill", false, 1);

            Vector3 startPos = skillUnit.transform.position;

            while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
            {
                yield return null;
            }


            skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill2", false, 1);

            Vector3 offset;
            offset = skillTargets[0].transform.position;
            offset.x = skillTargets[0].transform.GetChild(0).localScale.x * 10 * 1.2f;

            skillUnit.transform.position = offset;
            yield return null;

            while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
            {
                yield return null;
            }



            skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill3", false, 1);
            yield return null;

            skillUnit.transform.position = startPos;

            while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
            {

                yield return null;
            }

            skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
            GameManager.Instance.SetGameSpeed(1);
            Debug.Log("���� ��");

            skillBackgroundImage.SetActive(false);
        }

        IEnumerator VeritySkill()
        {
            Debug.Log("verity");


            var targetsList = GameManager.Instance.enemiesList;



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

        IEnumerator PardoSkill()
        {
            var targetsList = GameManager.Instance.enemiesList;

            List<GameObject> skillTargets = new List<GameObject>();

            while (!isSkillAimEnd)
            {
                skillTargets = AimSkillTargetsInRange(SkilRangeType.Circle, SkillAimType.Circle, "Enemy", 5, 1);
                yield return null;
            }



            foreach (var minion in targetsList)
            {
                minion.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
            }

            foreach (var target in skillTargets)
            {
                target.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
           target.GetComponent<Unit>().SetAimUnitColor(false);
        }


        GameManager.Instance.SetGameSpeed(0);
            skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
            skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);
            skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill", false, 1);
            yield return null;

        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            yield return null;
        }

        // ��ų ���� �� ����Ǵ� �Լ� �����ϱ�

        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;    
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
        GameManager.Instance.SetGameSpeed(1);
        Debug.Log("���� ��"); 

        isSkillAimEnd = false;
        skillBackgroundImage.SetActive(false);

        GameObject skillObject = Instantiate(poisonMist);
        skillObject.transform.position = skillHitpos;
        Destroy(skillObject, 5);

    }

        IEnumerator AsherSkill()
    {
        var targetsList = GameManager.Instance.minionsList;

        List<GameObject> skillTargets = new List<GameObject>();

        while (skillTargets.Count <= 0)
        {
            skillTargets = AimSkillTargetsInRange(SkilRangeType.Circle, SkillAimType.Circle, "Minion", 5, 3);
            yield return null;
        }



        foreach (var minion in targetsList)
        {
            minion.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
        }

        foreach (var target in skillTargets)
        {
            target.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
            target.GetComponent<Unit>().SetAimUnitColor(false);
        }


        GameManager.Instance.SetGameSpeed(0);
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);
        skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill", false, 1);
        yield return null;

        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            yield return null;
        }

        isSkillAimEnd = false;
        skillBackgroundImage.SetActive(false);
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
        GameManager.Instance.SetGameSpeed(1);
        Debug.Log("���� ��");



        foreach (var minion in targetsList)
        {
            minion.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
        }

        foreach (var target in skillTargets)
        {
            target.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
            EffectManager.Instance.InstantiateHomingEffect("asher_barrier", target, 3);
            StartCoroutine(skillUnit.GetComponent<Unit>().ChangeStat(skillUnit, "non", 0, 3));
        }
        


    }

        IEnumerator VogueSkill()
        {
        var targetsList = GameManager.Instance.enemiesList;

        List<GameObject> skillTargets = new List<GameObject>();

        while (skillTargets.Count <= 0)
        {
            skillTargets = AimSkillTargetsInRange(SkilRangeType.Circle, SkillAimType.Circle, "Enemy", 5, 2);
            yield return null;
        }



        foreach (var minion in targetsList)
        {
            minion.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
        }

        foreach (var target in skillTargets)
        {
            target.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
            target.GetComponent<Unit>().SetAimUnitColor(false);
        }


        GameManager.Instance.SetGameSpeed(0);
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);
        skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill", false, 1);
        yield return null;

        Vector3 startPos = skillUnit.transform.position;

        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            skillUnit.transform.position = Vector3.Lerp(startPos, skillHitpos, skillUnit.GetComponent<Unit>().normalizedTime);

            yield return null;
        }



        isSkillAimEnd = false;
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
        GameManager.Instance.SetGameSpeed(1);
        Debug.Log("���� ��");

        skillBackgroundImage.SetActive(false);
    }

        IEnumerator WraithSkill()
        {

            GameManager.Instance.SetGameSpeed(skillAimTimeSpeed);

            yield return null;
        }

        IEnumerator IsabellaSkill()
        {

            GameManager.Instance.SetGameSpeed(skillAimTimeSpeed);

            yield return null;
        }

        IEnumerator ZippoSkill()
        {
            GameManager.Instance.SetGameSpeed(skillAimTimeSpeed);

            yield return null;
        }

    IEnumerator EremediumSkill()
    {
        GameManager.Instance.SetGameSpeed(0);
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);
        skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill", false, 1);
        yield return null;

        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            yield return null;
        }

        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
        GameManager.Instance.SetGameSpeed(1);
        Debug.Log("���� ��");

        skillBackgroundImage.SetActive(false);
        //EffectManager.Instance.InstantiateHomingEffect("pay_effect", skillUnit, 8);
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;

    }


    // �̺�Ʈ �ݹ� �Լ� �����
}
