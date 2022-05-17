using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class EnemyStat
{
    public string Name;
    public string Grade;
    public string Class;
    public float Level;
    public float HP;
    public float Def;
    public float Atk;
    public float AtkSpeed;
    public float MoveSpeed;
    public float AtkRange;
    public float CognitiveRange;
    public float RewardCost;

}

public class CSV_EnemyStatus : MonoBehaviour
{
    public EnemyStat Enemy_A_Level1;
    public EnemyStat Enemy_A_Level2;
    public EnemyStat Enemy_A_Level3;
    public EnemyStat Enemy_A_Level4;
    public EnemyStat Enemy_A_Level5;
    public EnemyStat Enemy_A_Level6;
    public EnemyStat Enemy_A_Level7;
    public EnemyStat Enemy_A_Level8;
    public EnemyStat Enemy_A_Level9;
    public EnemyStat Enemy_A_Level10;
    public EnemyStat Enemy_A_Level11;
    public EnemyStat Enemy_A_Level12;
    public EnemyStat Enemy_A_Level13;
    public EnemyStat Enemy_A_Level14;
    public EnemyStat Enemy_A_Level15;

    public EnemyStat Enemy_B_Level1;
    public EnemyStat Enemy_B_Level2;
    public EnemyStat Enemy_B_Level3;
    public EnemyStat Enemy_B_Level4;
    public EnemyStat Enemy_B_Level5;
    public EnemyStat Enemy_B_Level6;
    public EnemyStat Enemy_B_Level7;
    public EnemyStat Enemy_B_Level8;
    public EnemyStat Enemy_B_Level9;
    public EnemyStat Enemy_B_Level10;
    public EnemyStat Enemy_B_Level11;
    public EnemyStat Enemy_B_Level12;
    public EnemyStat Enemy_B_Level13;
    public EnemyStat Enemy_B_Level14;
    public EnemyStat Enemy_B_Level15;

    public EnemyStat Enemy_C_Level1;
    public EnemyStat Enemy_C_Level2;
    public EnemyStat Enemy_C_Level3;
    public EnemyStat Enemy_C_Level4;
    public EnemyStat Enemy_C_Level5;
    public EnemyStat Enemy_C_Level6;
    public EnemyStat Enemy_C_Level7;
    public EnemyStat Enemy_C_Level8;
    public EnemyStat Enemy_C_Level9;
    public EnemyStat Enemy_C_Level10;
    public EnemyStat Enemy_C_Level11;
    public EnemyStat Enemy_C_Level12;
    public EnemyStat Enemy_C_Level13;
    public EnemyStat Enemy_C_Level14;
    public EnemyStat Enemy_C_Level15;



    // Start is called before the first frame update
    void Start()
    {
        List<Dictionary<string, object>> EnemyAStatList = CSVReader.Read("LevelDesignDataList_20220511.xlsx - Enemy_AStatus");
        List<Dictionary<string, object>> EnemyBStatList = CSVReader.Read("LevelDesignDataList_20220511.xlsx - Enemy_BStatus");
        List<Dictionary<string, object>> EnemyCStatList = CSVReader.Read("LevelDesignDataList_20220511.xlsx - Enemy_CStatus");

        

        EnemyStatUpdate(Enemy_A_Level1, EnemyAStatList,0);
        EnemyStatUpdate(Enemy_A_Level2, EnemyAStatList,1);
        EnemyStatUpdate(Enemy_A_Level3, EnemyAStatList, 2);
        EnemyStatUpdate(Enemy_A_Level4, EnemyAStatList, 3);
        EnemyStatUpdate(Enemy_A_Level5, EnemyAStatList, 4);
        EnemyStatUpdate(Enemy_A_Level6, EnemyAStatList, 5);
        EnemyStatUpdate(Enemy_A_Level7, EnemyAStatList, 6);
        EnemyStatUpdate(Enemy_A_Level8, EnemyAStatList, 7);
        EnemyStatUpdate(Enemy_A_Level9, EnemyAStatList, 8);
        EnemyStatUpdate(Enemy_A_Level10, EnemyAStatList, 9);
        EnemyStatUpdate(Enemy_A_Level11, EnemyAStatList, 10);
        EnemyStatUpdate(Enemy_A_Level12, EnemyAStatList, 11);
        EnemyStatUpdate(Enemy_A_Level13, EnemyAStatList, 12);
        EnemyStatUpdate(Enemy_A_Level14, EnemyAStatList, 13);
        EnemyStatUpdate(Enemy_A_Level15, EnemyAStatList, 14);

        EnemyStatUpdate(Enemy_B_Level1, EnemyBStatList, 0);
        EnemyStatUpdate(Enemy_B_Level2, EnemyBStatList, 1);
        EnemyStatUpdate(Enemy_B_Level3, EnemyBStatList, 2);
        EnemyStatUpdate(Enemy_B_Level4, EnemyBStatList, 3);
        EnemyStatUpdate(Enemy_B_Level5, EnemyBStatList, 4);
        EnemyStatUpdate(Enemy_B_Level6, EnemyBStatList, 5);
        EnemyStatUpdate(Enemy_B_Level7, EnemyBStatList, 6);
        EnemyStatUpdate(Enemy_B_Level8, EnemyBStatList, 7);
        EnemyStatUpdate(Enemy_B_Level9, EnemyBStatList, 8);
        EnemyStatUpdate(Enemy_B_Level10, EnemyBStatList, 9);
        EnemyStatUpdate(Enemy_B_Level11, EnemyBStatList, 10);
        EnemyStatUpdate(Enemy_B_Level12, EnemyBStatList, 11);
        EnemyStatUpdate(Enemy_B_Level13, EnemyBStatList, 12);
        EnemyStatUpdate(Enemy_B_Level14, EnemyBStatList, 13);
        EnemyStatUpdate(Enemy_B_Level15, EnemyBStatList, 14);

        EnemyStatUpdate(Enemy_C_Level1, EnemyCStatList, 0);
        EnemyStatUpdate(Enemy_C_Level2, EnemyCStatList, 1);
        EnemyStatUpdate(Enemy_C_Level3, EnemyCStatList, 2);
        EnemyStatUpdate(Enemy_C_Level4, EnemyCStatList, 3);
        EnemyStatUpdate(Enemy_C_Level5, EnemyCStatList, 4);
        EnemyStatUpdate(Enemy_C_Level6, EnemyCStatList, 5);
        EnemyStatUpdate(Enemy_C_Level7, EnemyCStatList, 6);
        EnemyStatUpdate(Enemy_C_Level8, EnemyCStatList, 7);
        EnemyStatUpdate(Enemy_C_Level9, EnemyCStatList, 8);
        EnemyStatUpdate(Enemy_C_Level10, EnemyCStatList, 9);
        EnemyStatUpdate(Enemy_C_Level11, EnemyCStatList, 10);
        EnemyStatUpdate(Enemy_C_Level12, EnemyCStatList, 11);
        EnemyStatUpdate(Enemy_C_Level13, EnemyCStatList, 12);
        EnemyStatUpdate(Enemy_C_Level14, EnemyCStatList, 13);
        EnemyStatUpdate(Enemy_C_Level15, EnemyCStatList, 14);


    }

    void EnemyStatUpdate(EnemyStat charactor, List<Dictionary<string, object>> ListData, int charactorlevel)
    {
        charactor.Name = ListData[charactorlevel]["Name"].ToString();
        charactor.Grade = ListData[charactorlevel]["Grade"].ToString();
        charactor.Class = ListData[charactorlevel]["Class"].ToString();
        charactor.Level = float.Parse(ListData[charactorlevel]["Level"].ToString());
        charactor.HP = float.Parse(ListData[charactorlevel]["Hp"].ToString());
        charactor.Def = float.Parse(ListData[charactorlevel]["Def"].ToString());
        charactor.Atk = float.Parse(ListData[charactorlevel]["Atk"].ToString());
        charactor.AtkSpeed = float.Parse(ListData[charactorlevel]["AtkSpeed"].ToString());
        charactor.MoveSpeed = float.Parse(ListData[charactorlevel]["MoveSpeed"].ToString());
        charactor.AtkRange = float.Parse(ListData[charactorlevel]["AtkRange"].ToString());
        charactor.CognitiveRange = float.Parse(ListData[charactorlevel]["CognitiveRange"].ToString());
        charactor.RewardCost = float.Parse(ListData[charactorlevel]["RewardCost"].ToString());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
