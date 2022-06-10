using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [Header("StageSelectScene First")]
    [SerializeField] private List<StageInfo> roundStage;
    [SerializeField] private StageInfo TargetStage;

    [Space(10f)]
    [Header("StageSelectScene Second")]
    [SerializeField] public List<StageCard> StageCardList;
    [SerializeField] private int StageCount;

    //[SerializeField] private List<>
    // Start is called before the first frame update
    void Start()
    {
        //for(int i = 0; i < StageCardList.Count; i++)
        //{
        //    StageCardList[i].
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenStage()
    {
        if(StageCardList.Count - 1 > StageCount)
            StageCardList[++StageCount].OpenStage();
        //StageCount++;
    }


    public void SetTargetStage(GameObject obj)
    {
        TargetStage = obj.GetComponent<StageInfo>();
        TargetStage.Selected();
    }
    public void PopTargetStage()
    {
        TargetStage.Selected();
        TargetStage = null;
    }

}
