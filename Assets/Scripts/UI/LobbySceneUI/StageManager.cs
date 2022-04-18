using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private List<StageInfo> roundStage;
    [SerializeField] private StageInfo TargetStage;

    //[SerializeField] private List<>
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
