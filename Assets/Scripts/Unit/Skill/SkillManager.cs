using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spine.Unity;
enum SkilRangeType
{
    Circle,
    Rectangle
}

enum SkillAimType
{
    Single,
    Circle,
    Auto
}

public class SkillManager : Singleton<SkillManager>
{
    Ray ray;
    RaycastHit hit;

    public float skillAimTimeSpeed;

    private GameObject skillUnit; // 스킬을 사용중인 유닛

    public bool isSkillActing = false;

    // 스킬 UI 변수
    public GameObject skillBackgroundImage;
    public GameObject skillCircleRangeUI;

    public GameObject skillAimCircleRangeUI;


    public GameObject poisonMist;
    public GameObject healDrone;
    public GameObject verityShot;

    private Vector3 skillHitpos;

    private bool isSkillAimEnd = false;

    private List<GameObject> skillTargets = new List<GameObject>();

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

        if (skillUnit.GetComponent<DefenceMinion>().skillTimer < skillUnit.GetComponent<DefenceMinion>().skillCoolTime)
        {
            return;
            Debug.Log(skillUnit.GetComponent<DefenceMinion>().skillTimer);
        }

        minionName = skillUnit.GetComponent<DefenceMinion>().Unitname;
        isSkillActing = true;
        skillBackgroundImage.SetActive(true);
        skillTargets.Clear();
        
       

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
        else if (skilRangeType.Equals(SkilRangeType.Rectangle))
        {

            //skillCircleRangeUI.SetActive(true);
            //skillCircleRangeUI.transform.localScale = new Vector3(2, 2, 2) * range;
            //skillCircleRangeUI.transform.position = skillUnit.transform.position;
            Vector3 box = new Vector3( range, 1, range2);
            Vector3 center = skillUnit.transform.position;
            center.x =  skillUnit.transform.position.x + range / 2;

            Collider[] colliders = Physics.OverlapBox(center, box, skillUnit.transform.rotation);


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
                    foreach (var target in targetsList)
                    {
                        target.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
                        target.GetComponent<Unit>().SetAimUnitColor(false);
                    }

                    Debug.Log(hit.transform.name);
                    returnTargets.Add(hit.transform.gameObject);
                    skillCircleRangeUI.SetActive(false);


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
                    Debug.Log("끝");
                    isSkillAimEnd = true;
                    skillHitpos = skillAimCircleRangeUI.transform.position;
                    skillCircleRangeUI.SetActive(false);
                    skillAimCircleRangeUI.SetActive(false);
                }

                }

        }
        else if (skillAimType.Equals(SkillAimType.Auto))
        {
            foreach(var target in targetsList)
            {
                if (target.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder.Equals(1))
                {
                    returnTargets.Add(target);
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
            Debug.Log("연출 끝");

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
            Debug.Log("연출 끝");

        skillBackgroundImage.SetActive(false);
        StartCoroutine( skillUnit.GetComponent<Unit>().ChangeStat(skillUnit ,"ats", 1,8));
        EffectManager.Instance.InstantiateHomingEffect("sophia_effect", skillUnit, 8);
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
    }

        IEnumerator HwaseonSkill()
        {
            var targetsList = GameManager.Instance.minionsList;

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
            Debug.Log("연출 끝");

        skillBackgroundImage.SetActive(false);
        StartCoroutine(skillUnit.GetComponent<Unit>().ChangeStat(skillUnit, "atk", skillUnit.GetComponent<Unit>().currentAtk * 0.4f, 8)) ;
        EffectManager.Instance.InstantiateHomingEffect("hwaseon_effect", skillTargets[0], 8);
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
    }

        IEnumerator VeritySkill()
        {
        var targetsList = GameManager.Instance.minionsList;

        while (skillTargets.Count <= 0)
        {
            skillTargets = AimSkillTargetsInRange(SkilRangeType.Circle, SkillAimType.Single, "Enemy", 10);
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
        yield return null;

        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            yield return null;
        }



        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
        GameManager.Instance.SetGameSpeed(1);
        Debug.Log("연출 끝");

        skillBackgroundImage.SetActive(false);
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
        isSkillActing = false;
        
        skillUnit.GetComponent<DefenceMinion>().skillTimer = 0;
    }

        IEnumerator PardoSkill()
        {
            var targetsList = GameManager.Instance.enemiesList;

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

        // 스킬 끝난 후 실행되는 함수 제작하기

        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;    
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
        GameManager.Instance.SetGameSpeed(1);
        Debug.Log("연출 끝"); 

        isSkillAimEnd = false;
        skillBackgroundImage.SetActive(false);

        GameObject skillObject = Instantiate(poisonMist);
        skillObject.transform.position = skillHitpos;
        Destroy(skillObject, 5);

    }

        IEnumerator AsherSkill()
    {
        var targetsList = GameManager.Instance.minionsList;

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
        Debug.Log("연출 끝");



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

        while (!isSkillAimEnd)
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

        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            yield return null;
        }

        skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill2", false, 1);
        Vector3 startPos = skillUnit.transform.position;
        yield return null;

        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            skillUnit.transform.position = Vector3.Lerp(startPos, skillHitpos, skillUnit.GetComponent<Unit>().normalizedTime);
            yield return null;
        }



        skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill3", false, 1);
        yield return null;
        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            yield return null;
        }



        isSkillAimEnd = false;
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().moveState);
        GameManager.Instance.SetGameSpeed(1);
        Debug.Log("연출 끝");

        skillBackgroundImage.SetActive(false);
    }

        IEnumerator WraithSkill()
        {


        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);

        skillTargets = AimSkillTargetsInRange(SkilRangeType.Rectangle, SkillAimType.Auto, "Enemy", 3, 1);
        GameManager.Instance.SetGameSpeed(0);
        skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill", false, 1);
        yield return null;
        //Vector3 startPos = skillUnit.transform.position;




        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            yield return null;
        }

        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
        GameManager.Instance.SetGameSpeed(1);
        Debug.Log("연출 끝");
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
        isSkillAimEnd = false;
        skillBackgroundImage.SetActive(false);

        yield return null;
        }

        IEnumerator IsabellaSkill()
        {
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);

        skillTargets = AimSkillTargetsInRange(SkilRangeType.Rectangle, SkillAimType.Auto, "Enemy", 3, 1);
        GameManager.Instance.SetGameSpeed(0);
        skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill", false, 1);
        yield return null;
        //Vector3 startPos = skillUnit.transform.position;




        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            yield return null;
        }

        foreach (var target in skillTargets)
        {
            target.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
        GameManager.Instance.SetGameSpeed(1);
        Debug.Log("연출 끝");
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
        isSkillAimEnd = false;
        skillBackgroundImage.SetActive(false);

        yield return null;


    }

        IEnumerator ZippoSkill()
        {

        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);

        skillTargets = AimSkillTargetsInRange(SkilRangeType.Rectangle, SkillAimType.Auto, "Enemy", 3, 1);
        GameManager.Instance.SetGameSpeed(0);
        skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill", false, 1);
        EffectManager.Instance.InstantiateAttackEffect("zippo_skill", skillUnit.transform.position);

        yield return null;
        //Vector3 startPos = skillUnit.transform.position;




        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            yield return null;
        }

        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
        GameManager.Instance.SetGameSpeed(1);
        Debug.Log("연출 끝");
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
        isSkillAimEnd = false;
        skillBackgroundImage.SetActive(false);

        yield return null;
    }

    IEnumerator KuenSkill()
    {

        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1;
        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().SkillPerformState);

        skillTargets = AimSkillTargetsInRange(SkilRangeType.Rectangle, SkillAimType.Auto, "Enemy", 3, 1);
        GameManager.Instance.SetGameSpeed(0);
        skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill1", false, 1);


        yield return null;
        //Vector3 startPos = skillUnit.transform.position;




        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            yield return null;
        }

        EffectManager.Instance.InstantiateAttackEffect("kuen_effect", skillUnit.transform.position);

        Vector3 startPos = skillUnit.transform.position;
        skillUnit.transform.position = new Vector3(1000, 1000, 1000);
        foreach (var target in skillTargets)
        {
            target.GetComponent<Unit>().Deal(skillUnit.GetComponent<Unit>().currentAtk * 0.5f);
        }

        yield return new WaitForSeconds(0.4f);


        foreach (var target in skillTargets)
        {
            target.GetComponent<Unit>().Deal(skillUnit.GetComponent<Unit>().currentAtk * 0.5f);
        }

        yield return new WaitForSeconds(0.4f);

        skillUnit.transform.position = startPos;
        skillUnit.GetComponent<Unit>().spineAnimation.PlayAnimation(skillUnit.GetComponent<Unit>().skinName + "/skill2", false, 1);
        yield return null;

        while (skillUnit.GetComponent<Unit>().normalizedTime < 1)
        {
            yield return null;
        }

        skillUnit.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
        GameManager.Instance.SetGameSpeed(1);
        Debug.Log("연출 끝");
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;
        isSkillAimEnd = false;
        skillBackgroundImage.SetActive(false);

        yield return null;
    }
    IEnumerator EremediumSkill()
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
        Debug.Log("연출 끝");

        skillBackgroundImage.SetActive(false);
        skillUnit.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = -1;

        isSkillActing = false;
    }



    public void MinionSkillEvent(string MinionName)
    {
        if(MinionName.Equals("Wraith")
            || MinionName.Equals("Zippo")
             || MinionName.Equals("Kuen")
             || MinionName.Equals("Eremedium")
             || MinionName.Equals("Isabella")
                || MinionName.Equals("Vogue")
                   || MinionName.Equals("Verity"))
            StartCoroutine(MinionName + "SkillEvent");



    }
    // 이벤트 콜백 함수 만들기

    IEnumerator WraithSkillEvent()
    {
        EffectManager.Instance.InstantiateAttackEffect("wraith_skill", skillUnit.transform.position);

        foreach(var target in skillTargets)
        {
            target.GetComponent<Unit>().Deal(skillUnit.GetComponent<Unit>().currentAtk * 0.5f);
        }

        yield return null;
    }
    IEnumerator IsabellaSkillEvent()
    {
        EffectManager.Instance.InstantiateAttackEffect("isabella_skill", skillUnit.transform.position);

        foreach (var target in skillTargets)
        {
            target.GetComponent<Unit>().Deal(skillUnit.GetComponent<Unit>().currentAtk * 0.5f);
            target.GetComponent<Rigidbody>().AddExplosionForce(100, skillUnit.transform.position, 3, 0, ForceMode.Impulse);
            target.GetComponent<UnitStateMachine>().ChangeState(skillUnit.GetComponent<UnitStateMachine>().idleState);
        }

        yield return null;
    }

    IEnumerator ZippoSkillEvent()
    {


        foreach (var target in skillTargets)
        {
            target.GetComponent<Unit>().Deal(skillUnit.GetComponent<Unit>().currentAtk * 0.5f);
            EffectManager.Instance.InstantiateAttackEffect("zippo_skillHit", target.transform.position);
        }

        yield return null;
    }

    IEnumerator KuenSkillEvent()
    {


        foreach (var target in skillTargets)
        {
            target.GetComponent<Unit>().Deal(skillUnit.GetComponent<Unit>().currentAtk * 0.5f);
            EffectManager.Instance.InstantiateAttackEffect("kuen_effect", target.transform.position);
        }

        yield return null;
    }

    IEnumerator EremediumSkillEvent()
    {

        yield return null;

        GameObject skillObject = Instantiate(healDrone);
        skillObject.transform.position = skillUnit.transform.position;
        skillObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;

        skillObject.GetComponent<HealDrone>().target = skillUnit;
        skillObject.GetComponent<HealDrone>().duration = 4;
        skillObject.GetComponent<HealDrone>().healAmount = skillUnit.GetComponent<Unit>().atk;
        skillObject.GetComponent<HealDrone>().healRange = 3;


    }

    IEnumerator VeritySkillEvent()
    {

        yield return null;

        GameObject skillObject = Instantiate(verityShot);

        Vector3 startPos = skillUnit.GetComponent<DefenceMinion>().shootPivot.transform.position;
        startPos.y += 2.1f;
        
        skillObject.transform.position = startPos;


        skillObject.GetComponent<VerityShot>().target = skillTargets[0];
        skillObject.GetComponent<VerityShot>().speed = 30;
        skillObject.GetComponent<VerityShot>().damage = 3;


    }
}
