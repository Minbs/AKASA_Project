using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class Stat
{
    public string Name;
    public string Grade;
    public string Class;
    public string Brood;
    public string Belong;
    public float HP;
    public float HPUP;
    public float Def;
    public float DefUP;
    public float Atk;
    public float AtkAdd;
    public float AtkSpeed;
    public float AtkSpeedUp;
    public float MoveSpeed;
    public float AtkRange;
    public float CognitiveRange;
    public float UpgradeCost;
    public float BuyCost;
    public float CellCost;
    public float RewardCost;
}

public class CSV_Player_Status : Singleton<CSV_Player_Status>
{
    public Stat VeriyStat;
    public Stat AngelusStat;
    public Stat AsherStat;
    public Stat EremediumStat;
    public Stat IsabellaStat;
    public Stat KuenStat;
    public Stat NoahStat;
    public Stat PardoStat;
    public Stat PayStat;
    public Stat SophiaStat;
    public Stat WraithStat;
    public Stat ZippoStat;
    public Stat EilgosStat;
    public Stat HwaseonStat;
    public Stat EnemyAStat;
    public Stat EnemyBStat;
    public Stat EnemyCStat;

    public Stat[] VeriyStat_Array;
    public Stat[] AngelusStat_Array;
    public Stat[] AsherStat_Array;
    public Stat[] EremediumStat_Array;
    public Stat[] IsabellaStat_Array;
    public Stat[] KuenStat_Array;
    public Stat[] NoahStat_Array;
    public Stat[] PardoStat_Array;
    public Stat[] PayStat_Array;
    public Stat[] SophiaStat_Array;
    public Stat[] WraithStat_Array;
    public Stat[] ZippoStat_Array;
    public Stat[] EilgosStat_Array;
    public Stat[] HwaseonStat_Array;
    public Stat[] EnemyAStat_Array;
    public Stat[] EnemyBStat_Array;
    public Stat[] EnemyCStat_Array;

    void Awake()
    {
        StartParsing();
    }
    
    void Start()
    {
        List<Dictionary<string, object>> Veritydata = CSVReader.Read("LevelDesignDataList.xlsx - VerityStatus");
        List<Dictionary<string, object>> Eremediumdata = CSVReader.Read("LevelDesignDataList.xlsx - EremediumStatus");
        List<Dictionary<string, object>> Isabelladata = CSVReader.Read("LevelDesignDataList.xlsx - IsabellaStatus");
        List<Dictionary<string, object>> Hwaseondata = CSVReader.Read("LevelDesignDataList.xlsx - HwaseonStatus");
        //List<Dictionary<string, object>> Angelusdata = CSVReader.Read("Angelus_Stat_Table");
        //List<Dictionary<string, object>> Asherdata = CSVReader.Read("Asher_Stat_Table");
        List<Dictionary<string, object>> Kuendata = CSVReader.Read("LevelDesignDataList.xlsx - KuenStatus");
        //List<Dictionary<string, object>> Noahdata = CSVReader.Read("Noah_Stat_Table");
        List<Dictionary<string, object>> Pardodata = CSVReader.Read("LevelDesignDataList.xlsx - PardoStatus");
        //List<Dictionary<string, object>> Paydata = CSVReader.Read("Pay_Stat_Table");
        //List<Dictionary<string, object>> Sophiadata = CSVReader.Read("Sophia_Stat_Table");
        List<Dictionary<string, object>> Wratihdata = CSVReader.Read("LevelDesignDataList.xlsx - WraithStatus");
        List<Dictionary<string, object>> Zippodata = CSVReader.Read("LevelDesignDataList.xlsx - ZippoStatus");
        //List<Dictionary<string, object>> Eilgosdata = CSVReader.Read("Eilgos_Stat_Table");
        List<Dictionary<string, object>> EnemyA = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_AStatus");
        List<Dictionary<string, object>> EnemyB = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_BStatus");
        List<Dictionary<string, object>> EnemyC = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_CStatus");

        for(int i = 0; i < 15; i++)
        {
            StatUpdate(VeriyStat_Array[i], i, Veritydata);
            //StatUpdate(_AngelusStat, _saveData.AngelusLevel, Angelusdata);
            //StatUpdate(_AsherStat, _saveData.AsherLevel, Asherdata);
            StatUpdate(EremediumStat_Array[i], i, Eremediumdata);
            StatUpdate(HwaseonStat_Array[i], i, Hwaseondata);
            StatUpdate(IsabellaStat_Array[i], i, Isabelladata);
            StatUpdate(KuenStat_Array[i], i, Kuendata);
            //StatUpdate(_NoahStat, _saveData.NoahLevel, Noahdata);
            StatUpdate(PardoStat_Array[i],i, Pardodata);
            //StatUpdate(_PayStat, _saveData.PayLevel, Paydata);
            //StatUpdate(_SophiaStat, _saveData.SophiaLevel, Sophiadata);
            StatUpdate(WraithStat_Array[i], i, Wratihdata);
            StatUpdate(ZippoStat_Array[i], i, Zippodata);
            //StatUpdate(_EilgosStat, _saveData.EilgosLevel, Eilgosdata);
            StatUpdate_Enemy(EnemyAStat_Array[i], i, EnemyA);
            StatUpdate_Enemy(EnemyBStat_Array[i], i, EnemyB);
            StatUpdate_Enemy(EnemyCStat_Array[i], i, EnemyC);
        }


    }

    
    public void StartParsing()
    {
        
        string StringSavedata = File.ReadAllText(Application.dataPath + "/SaveData.json");
        SaveData _saveData = JsonUtility.FromJson<SaveData>(StringSavedata);
        

        List<Dictionary<string, object>> Veritydata = CSVReader.Read("LevelDesignDataList.xlsx - VerityStatus");
        List<Dictionary<string, object>> Eremediumdata = CSVReader.Read("LevelDesignDataList.xlsx - EremediumStatus");
        List<Dictionary<string, object>> Isabelladata = CSVReader.Read("LevelDesignDataList.xlsx - IsabellaStatus");
        List<Dictionary<string, object>> Hwaseondata = CSVReader.Read("LevelDesignDataList.xlsx - HwaseonStatus");
        //List<Dictionary<string, object>> Angelusdata = CSVReader.Read("Angelus_Stat_Table");
        //List<Dictionary<string, object>> Asherdata = CSVReader.Read("Asher_Stat_Table");
        List<Dictionary<string, object>> Kuendata = CSVReader.Read("LevelDesignDataList.xlsx - KuenStatus");
        //List<Dictionary<string, object>> Noahdata = CSVReader.Read("Noah_Stat_Table");
        List<Dictionary<string, object>> Pardodata = CSVReader.Read("LevelDesignDataList.xlsx - PardoStatus");
        //List<Dictionary<string, object>> Paydata = CSVReader.Read("Pay_Stat_Table");
        //List<Dictionary<string, object>> Sophiadata = CSVReader.Read("Sophia_Stat_Table");
        List<Dictionary<string, object>> Wratihdata = CSVReader.Read("LevelDesignDataList.xlsx - WraithStatus");
        List<Dictionary<string, object>> Zippodata = CSVReader.Read("LevelDesignDataList.xlsx - ZippoStatus");
        //List<Dictionary<string, object>> Eilgosdata = CSVReader.Read("Eilgos_Stat_Table");
        List<Dictionary<string, object>> EnemyA = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_AStatus");
        List<Dictionary<string, object>> EnemyB = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_BStatus");
        List<Dictionary<string, object>> EnemyC = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_CStatus");

        Stat _VeriyStat = new Stat();
        Stat _AngelusStat = new Stat();
        Stat _AsherStat = new Stat();
        Stat _EremediumStat = new Stat();
        Stat _HwaseonStat = new Stat();
        Stat _IsabellaStat = new Stat();
        Stat _KuenStat = new Stat();
        Stat _NoahStat = new Stat();
        Stat _PardoStat = new Stat();
        Stat _PayStat = new Stat();
        Stat _SophiaStat = new Stat();
        Stat _WratihStat = new Stat();
        Stat _ZippoStat = new Stat();
        Stat _EilgosStat = new Stat();
        Stat _EnemyA = new Stat();
        Stat _EnemyB = new Stat();
        Stat _EnemyC = new Stat();

        StatUpdate(_VeriyStat, _saveData.VertyLevel, Veritydata);
        //StatUpdate(_AngelusStat, _saveData.AngelusLevel, Angelusdata);
        //StatUpdate(_AsherStat, _saveData.AsherLevel, Asherdata);
        StatUpdate(_EremediumStat, _saveData.EremediumLevel, Eremediumdata);
        StatUpdate(_HwaseonStat, _saveData.HwaseonLevel, Hwaseondata);
        StatUpdate(_IsabellaStat, _saveData.IsabellaLevel, Isabelladata);
        StatUpdate(_KuenStat, _saveData.KuenLevel, Kuendata);
        //StatUpdate(_NoahStat, _saveData.NoahLevel, Noahdata);
        StatUpdate(_PardoStat, _saveData.PardoLevel, Pardodata);
        //StatUpdate(_PayStat, _saveData.PayLevel, Paydata);
        //StatUpdate(_SophiaStat, _saveData.SophiaLevel, Sophiadata);
        StatUpdate(_WratihStat, _saveData.WratihLevel, Wratihdata);
        StatUpdate(_ZippoStat, _saveData.ZippoLevel, Zippodata);
        //StatUpdate(_EilgosStat, _saveData.EilgosLevel, Eilgosdata);
        StatUpdate_Enemy(_EnemyA, 2, EnemyA);
        StatUpdate_Enemy(_EnemyB, 2, EnemyB);
        StatUpdate_Enemy(_EnemyC, 2, EnemyC);


        HwaseonStat = _HwaseonStat;
        VeriyStat = _VeriyStat;
        AngelusStat = _AngelusStat;
        AsherStat = _AsherStat;
        EremediumStat = _EremediumStat;
        IsabellaStat = _IsabellaStat;
        KuenStat = _KuenStat;
        NoahStat = _NoahStat;
        PardoStat = _PardoStat;
        PayStat = _PayStat;
        SophiaStat = _SophiaStat;
        WraithStat = _WratihStat;
        ZippoStat = _ZippoStat;
        EilgosStat = _EilgosStat;
        EnemyAStat = _EnemyA;
        EnemyBStat = _EnemyB;
        EnemyCStat = _EnemyC;
        
    }
    // Start is called before the first frame update
    public void StatUpdate(Stat charactor, int charactorlevel, List<Dictionary<string, object>> ListData)
    {
        charactor.Name = ListData[charactorlevel]["Name"].ToString();
        charactor.Grade = ListData[charactorlevel]["Grade"].ToString();
        charactor.Class = ListData[charactorlevel]["Class"].ToString();
        charactor.Brood = ListData[charactorlevel]["Brood"].ToString();
        charactor.Belong = ListData[charactorlevel]["Belong"].ToString();
        charactor.HP = float.Parse(ListData[charactorlevel]["Hp"].ToString());
        charactor.HPUP = float.Parse(ListData[charactorlevel]["HpUp"].ToString());
        charactor.Def = float.Parse(ListData[charactorlevel]["Def"].ToString());
        charactor.DefUP = float.Parse(ListData[charactorlevel]["DefUp"].ToString());
        charactor.Atk = float.Parse(ListData[charactorlevel]["Atk"].ToString());
        charactor.AtkAdd = float.Parse(ListData[charactorlevel]["AtkAdd"].ToString());
        charactor.AtkSpeed = float.Parse(ListData[charactorlevel]["AtkSpeed"].ToString());
        charactor.AtkSpeedUp = float.Parse(ListData[charactorlevel]["AtkSpeedUp"].ToString());
        charactor.MoveSpeed = float.Parse(ListData[charactorlevel]["MoveSpeed"].ToString());
        charactor.AtkRange = float.Parse(ListData[charactorlevel]["AtkRange"].ToString());
        charactor.CognitiveRange = float.Parse(ListData[charactorlevel]["CognitiveRange"].ToString());
        charactor.UpgradeCost = float.Parse(ListData[charactorlevel]["UpgradeCost"].ToString());
        charactor.BuyCost = float.Parse(ListData[charactorlevel]["BuyCost"].ToString());
        charactor.CellCost = float.Parse(ListData[charactorlevel]["CellCost"].ToString());
    }
    public void StatUpdate_Enemy(Stat charactor, int charactorlevel, List<Dictionary<string, object>> ListData)
    {
        charactor.Name = ListData[charactorlevel]["Name"].ToString();
        charactor.Grade = ListData[charactorlevel]["Grade"].ToString();
        charactor.Class = ListData[charactorlevel]["Class"].ToString();
        charactor.HP = float.Parse(ListData[charactorlevel]["Hp"].ToString());
        charactor.Def = float.Parse(ListData[charactorlevel]["Def"].ToString());
        charactor.Atk = float.Parse(ListData[charactorlevel]["Atk"].ToString());
        charactor.AtkSpeed = float.Parse(ListData[charactorlevel]["AtkSpeed"].ToString());
        charactor.MoveSpeed = float.Parse(ListData[charactorlevel]["MoveSpeed"].ToString());
        charactor.AtkRange = float.Parse(ListData[charactorlevel]["AtkRange"].ToString());
        charactor.CognitiveRange = float.Parse(ListData[charactorlevel]["CognitiveRange"].ToString());
        charactor.RewardCost = float.Parse(ListData[charactorlevel]["RewardCost"].ToString());
    }


    public Stat Call_Stat_Array(string name, int level)
    {
        Stat errerStat = new Stat();
        if (name == "Verity")
        {
            return VeriyStat_Array[level-1];
        }

        if (name == "Angelus")
        {
            return AngelusStat_Array[level - 1];
        }
        if (name == "Asher")
        {
            return AsherStat_Array[level - 1];
        }
        if (name == "Eremedium")
        {
            return EremediumStat_Array[level - 1];
        }

        if (name == "Isabella")
        {
            return IsabellaStat_Array[level - 1];
        }

        if (name == "Kuen")
        {
            return KuenStat_Array[level - 1];
        }

        if (name == "Noah")
        {
            return NoahStat_Array[level - 1];
        }

        if (name == "Pardo")
        {
            return PardoStat_Array[level - 1];
        }
        if (name == "Sophia")
        {
            return SophiaStat_Array[level - 1];
        }

        if (name == "Wraith")
        {
            return WraithStat_Array[level - 1];
        }
        if (name == "Zippo")
        {
            return ZippoStat_Array[level - 1];
        }
        if (name == "Eilgos")
        {
            return EilgosStat_Array[level - 1];
        }
        if (name == "Hwaseon")
        {
            return HwaseonStat_Array[level - 1];
        }

        if (name == "Enemy1")
        {
            return EnemyAStat_Array[level - 1];
        }
        if (name == "Enemy2")
        {
            return EnemyBStat_Array[level - 1];
        }
        if (name == "Enemy3")
        {
            return EnemyCStat_Array[level - 1];
        }

        return errerStat;

    }

    public Stat Call_Stat_CSV(string name, int level)
    {

        Stat errerStat = new Stat();


        if (name == "Verity")
        {
            List<Dictionary<string, object>> Veritydata = CSVReader.Read("LevelDesignDataList.xlsx - VerityStatus");
            StatUpdate(VeriyStat, level - 1, Veritydata);
            return VeriyStat;
        }
        if (name == "Angelus")
        {
            List<Dictionary<string, object>> Angelusdata = CSVReader.Read("LevelDesignDataList.xlsx - AngelusStatus");
            StatUpdate(AngelusStat, level - 1, Angelusdata);
            return AngelusStat;
        }
        if (name == "Asher")
        {
            List<Dictionary<string, object>> Asherdata = CSVReader.Read("LevelDesignDataList.xlsx - AsherStatus");
            StatUpdate(AsherStat, level - 1, Asherdata);
            return AsherStat;


        }
        if (name == "Eremedium")
        {
            List<Dictionary<string, object>> Eremediumdata = CSVReader.Read("LevelDesignDataList.xlsx - EremediumStatus");
            StatUpdate(EremediumStat, level - 1, Eremediumdata);
            return EremediumStat;
        }

        if (name == "Isabella")
        {
            List<Dictionary<string, object>> Isabelladata = CSVReader.Read("LevelDesignDataList.xlsx - IsabellaStatus");
            StatUpdate(IsabellaStat, level - 1, Isabelladata);
            return IsabellaStat;
        }
            
        if (name == "Kuen")
        {
            List<Dictionary<string, object>> Kuendata = CSVReader.Read("LevelDesignDataList.xlsx - KuenStatus");
            StatUpdate(KuenStat, level - 1, Kuendata);
            return KuenStat;
        }

        if (name == "Noah")
        {
            List<Dictionary<string, object>> Noahdata = CSVReader.Read("LevelDesignDataList.xlsx - NoahStatus");
            StatUpdate(NoahStat, level - 1, Noahdata);
            return NoahStat;
        }

        if (name == "Pardo")
        {
            List<Dictionary<string, object>> Pardodata = CSVReader.Read("LevelDesignDataList.xlsx - PardoStatus");
            StatUpdate(PardoStat, level - 1, Pardodata);
            return PardoStat;
        }
        if (name == "Sophia")
        {
            List<Dictionary<string, object>> Sophiadata = CSVReader.Read("LevelDesignDataList.xlsx - SophiaStatus");
            StatUpdate(SophiaStat, level - 1, Sophiadata);
            return SophiaStat;
        }

        if (name == "Wraith")
        {
            List<Dictionary<string, object>> Wraithdata = CSVReader.Read("LevelDesignDataList.xlsx - WraithStatus");
            StatUpdate(WraithStat, level - 1, Wraithdata);
            return WraithStat;
        }
        if (name == "Zippo")
        {
            List<Dictionary<string, object>> Zippodata = CSVReader.Read("LevelDesignDataList.xlsx - ZippoStatus");
            StatUpdate(ZippoStat, level - 1, Zippodata);
            return ZippoStat;
        }
        if (name == "Eilgos")
        {
            List<Dictionary<string, object>> Eilgosdata = CSVReader.Read("LevelDesignDataList.xlsx - EilgosStatus");
            StatUpdate(EilgosStat, level - 1, Eilgosdata);
            return EilgosStat;
        }
        if (name == "Hwaseon")
        {
            List<Dictionary<string, object>> Hwaseondata = CSVReader.Read("LevelDesignDataList.xlsx - HwaseonStatus");
            StatUpdate(HwaseonStat, level - 1, Hwaseondata);
            return HwaseonStat;
        }
        
        if (name == "Enemy1")
        { 
           List<Dictionary<string, object>> EnemyAdata = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_AStatus");
            StatUpdate_Enemy(EnemyAStat, level - 1, EnemyAdata);
            return EnemyAStat;
        }
        if (name == "Enemy2")
        {
            
            List<Dictionary<string, object>> EnemyBdata = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_BStatus");
            StatUpdate_Enemy(EnemyBStat, level - 1, EnemyBdata);
            return EnemyBStat;
        }
        if (name == "Enemy3")
        {
            List<Dictionary<string, object>> EnemyCdata = CSVReader.Read("LevelDesignDataList.xlsx - Enemy_CStatus");
            StatUpdate_Enemy(EnemyCStat, level - 1, EnemyCdata);
            return EnemyCStat;
        }

        Debug.Log("Call_Stat_이름 잘못입력함");
        return errerStat;
    }

    // Update is called once per frame
    void Update()
    {

    }


}
