using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class CSV_EnemyStat : MonoBehaviour
{

    public Stat Enemy_A;
    public Stat Enemy_B;
    public Stat Enemy_Boss;
    public Stat Enemy_Healer;
    public Stat Enemy_Tank;



    // Start is called before the first frame update
    void Start()
    {
        StartParsing();

    }



    public void StartParsing()
    {
        List<Dictionary<string, object>> EnemyAStatList = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_AStatus");
        List<Dictionary<string, object>> EnemyBStatList = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_BStatus");
        List<Dictionary<string, object>> EnemyBossStatList = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_BossStatus");
        List<Dictionary<string, object>> EnemyHealerStatList = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_HealerStatus");
        List<Dictionary<string, object>> EnemyTankStatList = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_TankStatus");
        StatUpdate(Enemy_A, EnemyAStatList, 0);
        StatUpdate(Enemy_B, EnemyBStatList, 0);
        StatUpdate(Enemy_Boss, EnemyBossStatList, 0);
        StatUpdate(Enemy_Healer, EnemyHealerStatList, 0);
        StatUpdate(Enemy_Tank, EnemyTankStatList, 0);
    }

    void StatUpdate(Stat charactor, List<Dictionary<string, object>> ListData, int charactorlevel)
    {

        charactor.Name = ListData[charactorlevel]["Name"].ToString();
        charactor.Grade = ListData[charactorlevel]["Grade"].ToString();
        charactor.Class = ListData[charactorlevel]["Class"].ToString();
        charactor.HP = float.Parse(ListData[charactorlevel]["Hp"].ToString());
        charactor.Def = float.Parse(ListData[charactorlevel]["Def"].ToString());
        charactor.Atk = float.Parse(ListData[charactorlevel]["Atk"].ToString());
        charactor.AtkSpeed = float.Parse(ListData[charactorlevel]["AtkSpeed"].ToString());
        charactor.MoveSpeed = float.Parse(ListData[charactorlevel]["MoveSpeed"].ToString());
        charactor.AtkRange1 = float.Parse(ListData[charactorlevel]["AtkRange1"].ToString());
        charactor.AtkRange2 = float.Parse(ListData[charactorlevel]["AtkRange2"].ToString());
        charactor.CognitiveRange = float.Parse(ListData[charactorlevel]["CognitiveRange"].ToString());
        charactor.RewardCost = float.Parse(ListData[charactorlevel]["RewardCost"].ToString());

    }

    public Stat Call_Stat(string name, int Level)
    {

        Stat errerStat = new Stat();

        if (name == "Enemy1")
        {
            List<Dictionary<string, object>> EnemyAStatList = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_AStatus");
            StatUpdate(Enemy_A, EnemyAStatList, Level);
            return Enemy_A;
        }

        if (name == "Enemy2")
        {
            List<Dictionary<string, object>> EnemyBStatList = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_BStatus");
            StatUpdate(Enemy_B, EnemyBStatList, Level);
            return Enemy_B;
        }

        if (name == "EnemyBoss")
        {
            List<Dictionary<string, object>> EnemyBossStatList = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_BossStatus");
            StatUpdate(Enemy_Boss, EnemyBossStatList, Level);
            return Enemy_Boss;
        }

        if (name == "EnemyHealer")
        {
            List<Dictionary<string, object>> EnemyHealerStatList = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_HealerStatus");
            StatUpdate(Enemy_Healer, EnemyHealerStatList, Level); ;
            return Enemy_Healer;
        }
        if (name == "EnemyTank")
        {
            List<Dictionary<string, object>> EnemyTankStatList = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_TankStatus");
            StatUpdate(Enemy_Tank, EnemyTankStatList, Level);
            return Enemy_Tank;
        }

        return errerStat;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
