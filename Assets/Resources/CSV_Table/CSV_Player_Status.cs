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

}

public class CSV_Player_Status: MonoBehaviour
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
    public Stat WratihStat;
    public Stat ZippoStat;
    public Stat EilgosStat;

    // Start is called before the first frame update
    void Start()
    {
        string StringSavedata = File.ReadAllText(Application.dataPath + "/SaveData.json");
        SaveData _saveData = JsonUtility.FromJson<SaveData>(StringSavedata);

        List<Dictionary<string, object>> Veritydata = CSVReader.Read("LevelDesignDataList_20220511.xlsx - VerityStatus");
        //List<Dictionary<string, object>> Angelusdata = CSVReader.Read("Angelus_Stat_Table");
        //List<Dictionary<string, object>> Asherdata = CSVReader.Read("Asher_Stat_Table");
        List<Dictionary<string, object>> Eremediumdata = CSVReader.Read("LevelDesignDataList_20220511.xlsx - EremediumStatus");
        //List<Dictionary<string, object>> Hwaseondata = CSVReader.Read("Hwaseon_Stat_Table");
        List<Dictionary<string, object>> Isabelladata = CSVReader.Read("LevelDesignDataList_20220511.xlsx - IsabellaStatus");
        //List<Dictionary<string, object>> Kuendata = CSVReader.Read("Kuen_Stat_Table");
        //List<Dictionary<string, object>> Noahdata = CSVReader.Read("Noah_Stat_Table");
        //List<Dictionary<string, object>> Pardodata = CSVReader.Read("Pardo_Stat_Table");
        //List<Dictionary<string, object>> Paydata = CSVReader.Read("Pay_Stat_Table");
        //List<Dictionary<string, object>> Sophiadata = CSVReader.Read("Sophia_Stat_Table");
        //List<Dictionary<string, object>> Wratihdata = CSVReader.Read("Wratih_Stat_Table");
        //List<Dictionary<string, object>> Zippodata = CSVReader.Read("Zippo_Stat_Table");
        //List<Dictionary<string, object>> Eilgosdata = CSVReader.Read("Eilgos_Stat_Table");
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


        StatUpdate(_VeriyStat, _saveData.VertyLevel, Veritydata);
        //StatUpdate(_AngelusStat, _saveData.AngelusLevel, Angelusdata);
        //StatUpdate(_AsherStat, _saveData.AsherLevel, Asherdata);
        StatUpdate(_EremediumStat, _saveData.EremediumLevel, Eremediumdata);
        //StatUpdate(_HwaseonStat, _saveData.HwaseonLevel, Hwaseondata);
        StatUpdate(_IsabellaStat, _saveData.IsabellaLevel, Isabelladata);
        //StatUpdate(_KuenStat, _saveData.KuenLevel, Kuendata);
        //StatUpdate(_NoahStat, _saveData.NoahLevel, Noahdata);
        //StatUpdate(_PardoStat, _saveData.PardoLevel, Pardodata);
        //StatUpdate(_PayStat, _saveData.PayLevel, Paydata);
        //StatUpdate(_SophiaStat, _saveData.SophiaLevel, Sophiadata);
        //StatUpdate(_WratihStat, _saveData.WratihLevel, Wratihdata);
        //StatUpdate(_ZippoStat, _saveData.ZippoLevel, Zippodata);
        //StatUpdate(_EilgosStat, _saveData.EilgosLevel, Eilgosdata);

         VeriyStat = _VeriyStat;
         AngelusStat = _AngelusStat;
         AsherStat = _AsherStat;
         EremediumStat = _EremediumStat;
         IsabellaStat = _IsabellaStat;
         KuenStat = _KuenStat;
         NoahStat = _NoahStat;
         PardoStat = _PardoStat;
         PayStat =_PayStat;
         SophiaStat = _SophiaStat;
         WratihStat = _WratihStat;
         ZippoStat = _ZippoStat;
         EilgosStat = _EilgosStat;

    }

    void StatUpdate(Stat charactor, int charactorlevel, List<Dictionary<string, object>> ListData)
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



    // Update is called once per frame
    void Update()
    {
        
    }


}
