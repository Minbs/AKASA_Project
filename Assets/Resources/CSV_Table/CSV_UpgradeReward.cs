using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


[System.Serializable]
public class EtherUpgradeReward
{
    public float Level;
    public float EtherUpgradeCost;
    public float UpgradeReward;
    public float TotalReward;


}

public class CSV_UpgradeReward : MonoBehaviour
{
    public EtherUpgradeReward EtherUpgradeReward_Level1;
    public EtherUpgradeReward EtherUpgradeReward_Level2;
    public EtherUpgradeReward EtherUpgradeReward_Level3;
    public EtherUpgradeReward EtherUpgradeReward_Level4;
    public EtherUpgradeReward EtherUpgradeReward_Level5;
    public EtherUpgradeReward EtherUpgradeReward_Level6;
    public EtherUpgradeReward EtherUpgradeReward_Level7;
    public EtherUpgradeReward EtherUpgradeReward_Level8;
    public EtherUpgradeReward EtherUpgradeReward_Level9;
    public EtherUpgradeReward EtherUpgradeReward_Level10;
    public EtherUpgradeReward EtherUpgradeReward_Level11;
    public EtherUpgradeReward EtherUpgradeReward_Level12;
    public EtherUpgradeReward EtherUpgradeReward_Level13;
    public EtherUpgradeReward EtherUpgradeReward_Level14;
    public EtherUpgradeReward EtherUpgradeReward_Level15;
    public EtherUpgradeReward EtherUpgradeReward_Level16;
    public EtherUpgradeReward EtherUpgradeReward_Level17;
    public EtherUpgradeReward EtherUpgradeReward_Level18;
    public EtherUpgradeReward EtherUpgradeReward_Level19;
    public EtherUpgradeReward EtherUpgradeReward_Level20;


    // Start is called before the first frame update
    void Start()
    {
        List<Dictionary<string, object>> EtherUpgradeRewardList = CSVReader.Read("LevelDesignDataList_20220511.xlsx - EtherUpgradeReward");
        StageWaveUpdate(EtherUpgradeReward_Level1, EtherUpgradeRewardList,0);
        StageWaveUpdate(EtherUpgradeReward_Level2, EtherUpgradeRewardList, 1);
        StageWaveUpdate(EtherUpgradeReward_Level3, EtherUpgradeRewardList, 2);
        StageWaveUpdate(EtherUpgradeReward_Level4, EtherUpgradeRewardList, 3);
        StageWaveUpdate(EtherUpgradeReward_Level5, EtherUpgradeRewardList, 4);
        StageWaveUpdate(EtherUpgradeReward_Level6, EtherUpgradeRewardList, 5);
        StageWaveUpdate(EtherUpgradeReward_Level7, EtherUpgradeRewardList, 6);
        StageWaveUpdate(EtherUpgradeReward_Level8, EtherUpgradeRewardList, 7);
        StageWaveUpdate(EtherUpgradeReward_Level9, EtherUpgradeRewardList, 8);
        StageWaveUpdate(EtherUpgradeReward_Level10, EtherUpgradeRewardList, 9);
        StageWaveUpdate(EtherUpgradeReward_Level11, EtherUpgradeRewardList, 10);
        StageWaveUpdate(EtherUpgradeReward_Level12, EtherUpgradeRewardList, 11);
        StageWaveUpdate(EtherUpgradeReward_Level13, EtherUpgradeRewardList, 12);
        StageWaveUpdate(EtherUpgradeReward_Level14, EtherUpgradeRewardList, 13);
        StageWaveUpdate(EtherUpgradeReward_Level15, EtherUpgradeRewardList, 14);
        StageWaveUpdate(EtherUpgradeReward_Level16, EtherUpgradeRewardList, 15);
        StageWaveUpdate(EtherUpgradeReward_Level17, EtherUpgradeRewardList, 16);
        StageWaveUpdate(EtherUpgradeReward_Level18, EtherUpgradeRewardList, 17);
        StageWaveUpdate(EtherUpgradeReward_Level19, EtherUpgradeRewardList, 18);
        StageWaveUpdate(EtherUpgradeReward_Level20, EtherUpgradeRewardList, 19);

    }

    void StageWaveUpdate(EtherUpgradeReward Rewarddata, List<Dictionary<string, object>> EtherUpgradeRewardList, int StageNum)
    {
        Rewarddata.Level = float.Parse(EtherUpgradeRewardList[StageNum]["Level"].ToString());
        Rewarddata.EtherUpgradeCost = float.Parse(EtherUpgradeRewardList[StageNum]["EtherUpgradeCost"].ToString());
        Rewarddata.UpgradeReward = float.Parse(EtherUpgradeRewardList[StageNum]["UpgradeReward"].ToString());
        Rewarddata.TotalReward = float.Parse(EtherUpgradeRewardList[StageNum]["TotalReward"].ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
