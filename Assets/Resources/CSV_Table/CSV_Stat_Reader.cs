using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class Stat
{
    public string Name;
    public string Class;
    public string Brood;
    public string Belong;
    public float HP;
    public float Def;
    public float Atk;
    public float AtkSpeed;
    public float MoveSpeed;
    public float AtkRange;
    public float CognitiveRange;

}

public class CSV_Stat_Reader : MonoBehaviour
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

        List<Dictionary<string, object>> Veritydata = CSVReader.Read("Verity_Stat_Table");
        List<Dictionary<string, object>> Angelusdata = CSVReader.Read("Angelus_Stat_Table");
        List<Dictionary<string, object>> Asherdata = CSVReader.Read("Asher_Stat_Table");
        List<Dictionary<string, object>> Eremediumdata = CSVReader.Read("Eremedium_Stat_Table");
        List<Dictionary<string, object>> Hwaseondata = CSVReader.Read("Hwaseon_Stat_Table");
        List<Dictionary<string, object>> Isabelladata = CSVReader.Read("Isabella_Stat_Table");
        List<Dictionary<string, object>> Kuendata = CSVReader.Read("Kuen_Stat_Table");
        List<Dictionary<string, object>> Noahdata = CSVReader.Read("Noah_Stat_Table");
        List<Dictionary<string, object>> Pardodata = CSVReader.Read("Pardo_Stat_Table");
        List<Dictionary<string, object>> Paydata = CSVReader.Read("Pay_Stat_Table");
        List<Dictionary<string, object>> Sophiadata = CSVReader.Read("Sophia_Stat_Table");
        List<Dictionary<string, object>> Wratihdata = CSVReader.Read("Wratih_Stat_Table");
        List<Dictionary<string, object>> Zippodata = CSVReader.Read("Zippo_Stat_Table");
        List<Dictionary<string, object>> Eilgosdata = CSVReader.Read("Eilgos_Stat_Table");
        //List<Dictionary<string, object>> Veritydata = CSVReader.Read("Verity_Stat_Table");
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

        _EilgosStat.Name = Eilgosdata[_saveData.EilgosLevel]["Name"].ToString();
        _EilgosStat.Class = Eilgosdata[_saveData.EilgosLevel]["Class"].ToString();
        _EilgosStat.Brood = Eilgosdata[_saveData.EilgosLevel]["Brood"].ToString();
        _EilgosStat.Belong = Eilgosdata[_saveData.EilgosLevel]["Belong"].ToString();
        _EilgosStat.HP = float.Parse(Eilgosdata[_saveData.EilgosLevel]["Hp"].ToString());
        _EilgosStat.Def = float.Parse(Eilgosdata[_saveData.EilgosLevel]["Def"].ToString());
        _EilgosStat.Atk = float.Parse(Eilgosdata[_saveData.EilgosLevel]["Atk"].ToString());
        _EilgosStat.AtkSpeed = float.Parse(Eilgosdata[_saveData.EilgosLevel]["AtkSpeed"].ToString());
        _EilgosStat.MoveSpeed = float.Parse(Eilgosdata[_saveData.EilgosLevel]["MoveSpeed"].ToString());
        _EilgosStat.AtkRange = float.Parse(Eilgosdata[_saveData.EilgosLevel]["AtkRange"].ToString());
        _EilgosStat.CognitiveRange = float.Parse(Eilgosdata[_saveData.EilgosLevel]["CognitiveRange"].ToString());


        _VeriyStat.Name = Veritydata[_saveData.VertyLevel]["Name"].ToString();
        _VeriyStat.Class = Veritydata[_saveData.VertyLevel]["Class"].ToString();
        _VeriyStat.Brood = Veritydata[_saveData.VertyLevel]["Brood"].ToString();
        _VeriyStat.Belong = Veritydata[_saveData.VertyLevel]["Belong"].ToString();
        _VeriyStat.HP = float.Parse(Veritydata[_saveData.VertyLevel]["Hp"].ToString());
        _VeriyStat.Def= float.Parse(Veritydata[_saveData.VertyLevel]["Def"].ToString());
        _VeriyStat.Atk = float.Parse(Veritydata[_saveData.VertyLevel]["Atk"].ToString());
        _VeriyStat.AtkSpeed = float.Parse(Veritydata[_saveData.VertyLevel]["AtkSpeed"].ToString());
        _VeriyStat.MoveSpeed = float.Parse(Veritydata[_saveData.VertyLevel]["MoveSpeed"].ToString());
        _VeriyStat.AtkRange = float.Parse(Veritydata[_saveData.VertyLevel]["AtkRange"].ToString());
        _VeriyStat.CognitiveRange = float.Parse(Veritydata[_saveData.VertyLevel]["CognitiveRange"].ToString());


        _AngelusStat.Name = Angelusdata[_saveData.AngelusLevel]["Name"].ToString();
        _AngelusStat.Class = Angelusdata[_saveData.AngelusLevel]["Class"].ToString();
        _AngelusStat.Brood = Angelusdata[_saveData.AngelusLevel]["Brood"].ToString();
        _AngelusStat.Belong = Angelusdata[_saveData.AngelusLevel]["Belong"].ToString();
        _AngelusStat.HP = float.Parse(Angelusdata[_saveData.AngelusLevel]["Hp"].ToString());
        _AngelusStat.Def = float.Parse(Angelusdata[_saveData.AngelusLevel]["Def"].ToString());
        _AngelusStat.Atk = float.Parse(Angelusdata[_saveData.AngelusLevel]["Atk"].ToString());
        _AngelusStat.AtkSpeed = float.Parse(Angelusdata[_saveData.AngelusLevel]["AtkSpeed"].ToString());
        _AngelusStat.MoveSpeed = float.Parse(Angelusdata[_saveData.AngelusLevel]["MoveSpeed"].ToString());
        _AngelusStat.AtkRange = float.Parse(Angelusdata[_saveData.AngelusLevel]["AtkRange"].ToString());
        _AngelusStat.CognitiveRange = float.Parse(Angelusdata[_saveData.AngelusLevel]["CognitiveRange"].ToString());

        _AsherStat.Name = Asherdata[_saveData.AsherLevel]["Name"].ToString();
        _AsherStat.Class = Asherdata[_saveData.AsherLevel]["Class"].ToString();
        _AsherStat.Brood = Asherdata[_saveData.AsherLevel]["Brood"].ToString();
        _AsherStat.Belong = Asherdata[_saveData.AsherLevel]["Belong"].ToString();
        _AsherStat.HP = float.Parse(Asherdata[_saveData.AsherLevel]["Hp"].ToString());
        _AsherStat.Def = float.Parse(Asherdata[_saveData.AsherLevel]["Def"].ToString());
        _AsherStat.Atk = float.Parse(Asherdata[_saveData.AsherLevel]["Atk"].ToString());
        _AsherStat.AtkSpeed = float.Parse(Asherdata[_saveData.AsherLevel]["AtkSpeed"].ToString());
        _AsherStat.MoveSpeed = float.Parse(Asherdata[_saveData.AsherLevel]["MoveSpeed"].ToString());
        _AsherStat.AtkRange = float.Parse(Asherdata[_saveData.AsherLevel]["AtkRange"].ToString());
        _AsherStat.CognitiveRange = float.Parse(Asherdata[_saveData.AsherLevel]["CognitiveRange"].ToString());

        _EremediumStat.Name = Eremediumdata[_saveData.EremediumLevel]["Name"].ToString();
        _EremediumStat.Class = Eremediumdata[_saveData.EremediumLevel]["Class"].ToString();
        _EremediumStat.Brood = Eremediumdata[_saveData.EremediumLevel]["Brood"].ToString();
        _EremediumStat.Belong = Eremediumdata[_saveData.EremediumLevel]["Belong"].ToString();
        _EremediumStat.HP = float.Parse(Eremediumdata[_saveData.EremediumLevel]["Hp"].ToString());
        _EremediumStat.Def = float.Parse(Eremediumdata[_saveData.EremediumLevel]["Def"].ToString());
        _EremediumStat.Atk = float.Parse(Eremediumdata[_saveData.EremediumLevel]["Atk"].ToString());
        _EremediumStat.AtkSpeed = float.Parse(Eremediumdata[_saveData.EremediumLevel]["AtkSpeed"].ToString());
        _EremediumStat.MoveSpeed = float.Parse(Eremediumdata[_saveData.EremediumLevel]["MoveSpeed"].ToString());
        _EremediumStat.AtkRange = float.Parse(Eremediumdata[_saveData.EremediumLevel]["AtkRange"].ToString());
        _EremediumStat.CognitiveRange = float.Parse(Eremediumdata[_saveData.EremediumLevel]["CognitiveRange"].ToString());

        _HwaseonStat.Name = Hwaseondata[_saveData.HwaseonLevel]["Name"].ToString();
        _HwaseonStat.Class = Hwaseondata[_saveData.HwaseonLevel]["Class"].ToString();
        _HwaseonStat.Brood = Hwaseondata[_saveData.HwaseonLevel]["Brood"].ToString();
        _HwaseonStat.Belong = Hwaseondata[_saveData.HwaseonLevel]["Belong"].ToString();
        _HwaseonStat.HP = float.Parse(Hwaseondata[_saveData.HwaseonLevel]["Hp"].ToString());
        _HwaseonStat.Def = float.Parse(Hwaseondata[_saveData.HwaseonLevel]["Def"].ToString());
        _HwaseonStat.Atk = float.Parse(Hwaseondata[_saveData.HwaseonLevel]["Atk"].ToString());
        _HwaseonStat.AtkSpeed = float.Parse(Hwaseondata[_saveData.HwaseonLevel]["AtkSpeed"].ToString());
        _HwaseonStat.MoveSpeed = float.Parse(Hwaseondata[_saveData.HwaseonLevel]["MoveSpeed"].ToString());
        _HwaseonStat.AtkRange = float.Parse(Hwaseondata[_saveData.HwaseonLevel]["AtkRange"].ToString());
        _HwaseonStat.CognitiveRange = float.Parse(Hwaseondata[_saveData.HwaseonLevel]["CognitiveRange"].ToString());

        _IsabellaStat.Name = Isabelladata[_saveData.IsabellaLevel]["Name"].ToString();
        _IsabellaStat.Class = Isabelladata[_saveData.IsabellaLevel]["Class"].ToString();
        _IsabellaStat.Brood = Isabelladata[_saveData.IsabellaLevel]["Brood"].ToString();
        _IsabellaStat.Belong = Isabelladata[_saveData.IsabellaLevel]["Belong"].ToString();
        _IsabellaStat.HP = float.Parse(Isabelladata[_saveData.IsabellaLevel]["Hp"].ToString());
        _IsabellaStat.Def = float.Parse(Isabelladata[_saveData.IsabellaLevel]["Def"].ToString());
        _IsabellaStat.Atk = float.Parse(Isabelladata[_saveData.IsabellaLevel]["Atk"].ToString());
        _IsabellaStat.AtkSpeed = float.Parse(Isabelladata[_saveData.IsabellaLevel]["AtkSpeed"].ToString());
        _IsabellaStat.MoveSpeed = float.Parse(Isabelladata[_saveData.IsabellaLevel]["MoveSpeed"].ToString());
        _IsabellaStat.AtkRange = float.Parse(Isabelladata[_saveData.IsabellaLevel]["AtkRange"].ToString());
        _IsabellaStat.CognitiveRange = float.Parse(Isabelladata[_saveData.IsabellaLevel]["CognitiveRange"].ToString());

        _KuenStat.Name = Kuendata[_saveData.KuenLevel]["Name"].ToString();
        _KuenStat.Class = Kuendata[_saveData.KuenLevel]["Class"].ToString();
        _KuenStat.Brood = Kuendata[_saveData.KuenLevel]["Brood"].ToString();
        _KuenStat.Belong = Kuendata[_saveData.KuenLevel]["Belong"].ToString();
        _KuenStat.HP = float.Parse(Kuendata[_saveData.KuenLevel]["Hp"].ToString());
        _KuenStat.Def = float.Parse(Kuendata[_saveData.KuenLevel]["Def"].ToString());
        _KuenStat.Atk = float.Parse(Kuendata[_saveData.KuenLevel]["Atk"].ToString());
        _KuenStat.AtkSpeed = float.Parse(Kuendata[_saveData.KuenLevel]["AtkSpeed"].ToString());
        _KuenStat.MoveSpeed = float.Parse(Kuendata[_saveData.KuenLevel]["MoveSpeed"].ToString());
        _KuenStat.AtkRange = float.Parse(Kuendata[_saveData.KuenLevel]["AtkRange"].ToString());
        _KuenStat.CognitiveRange = float.Parse(Kuendata[_saveData.KuenLevel]["CognitiveRange"].ToString());

        _NoahStat.Name = Noahdata[_saveData.NoahLevel]["Name"].ToString();
        _NoahStat.Class = Noahdata[_saveData.NoahLevel]["Class"].ToString();
        _NoahStat.Brood = Noahdata[_saveData.NoahLevel]["Brood"].ToString();
        _NoahStat.Belong = Noahdata[_saveData.NoahLevel]["Belong"].ToString();
        _NoahStat.HP = float.Parse(Noahdata[_saveData.NoahLevel]["Hp"].ToString());
        _NoahStat.Def = float.Parse(Noahdata[_saveData.NoahLevel]["Def"].ToString());
        _NoahStat.Atk = float.Parse(Noahdata[_saveData.NoahLevel]["Atk"].ToString());
        _NoahStat.AtkSpeed = float.Parse(Noahdata[_saveData.NoahLevel]["AtkSpeed"].ToString());
        _NoahStat.MoveSpeed = float.Parse(Noahdata[_saveData.NoahLevel]["MoveSpeed"].ToString());
        _NoahStat.AtkRange = float.Parse(Noahdata[_saveData.NoahLevel]["AtkRange"].ToString());
        _NoahStat.CognitiveRange = float.Parse(Noahdata[_saveData.NoahLevel]["CognitiveRange"].ToString());

        _PardoStat.Name = Pardodata[_saveData.PardoLevel]["Name"].ToString();
        _PardoStat.Class = Pardodata[_saveData.PardoLevel]["Class"].ToString();
        _PardoStat.Brood = Pardodata[_saveData.PardoLevel]["Brood"].ToString();
        _PardoStat.Belong = Pardodata[_saveData.PardoLevel]["Belong"].ToString();
        _PardoStat.HP = float.Parse(Pardodata[_saveData.PardoLevel]["Hp"].ToString());
        _PardoStat.Def = float.Parse(Pardodata[_saveData.PardoLevel]["Def"].ToString());
        _PardoStat.Atk = float.Parse(Pardodata[_saveData.PardoLevel]["Atk"].ToString());
        _PardoStat.AtkSpeed = float.Parse(Pardodata[_saveData.PardoLevel]["AtkSpeed"].ToString());
        _PardoStat.MoveSpeed = float.Parse(Pardodata[_saveData.PardoLevel]["MoveSpeed"].ToString());
        _PardoStat.AtkRange = float.Parse(Pardodata[_saveData.PardoLevel]["AtkRange"].ToString());
        _PardoStat.CognitiveRange = float.Parse(Pardodata[_saveData.PardoLevel]["CognitiveRange"].ToString());

        _PayStat.Name = Paydata[_saveData.PayLevel]["Name"].ToString();
        _PayStat.Class = Paydata[_saveData.PayLevel]["Class"].ToString();
        _PayStat.Brood = Paydata[_saveData.PayLevel]["Brood"].ToString();
        _PayStat.Belong = Paydata[_saveData.PayLevel]["Belong"].ToString();
        _PayStat.HP = float.Parse(Paydata[_saveData.PayLevel]["Hp"].ToString());
        _PayStat.Def = float.Parse(Paydata[_saveData.PayLevel]["Def"].ToString());
        _PayStat.Atk = float.Parse(Paydata[_saveData.PayLevel]["Atk"].ToString());
        _PayStat.AtkSpeed = float.Parse(Paydata[_saveData.PayLevel]["AtkSpeed"].ToString());
        _PayStat.MoveSpeed = float.Parse(Paydata[_saveData.PayLevel]["MoveSpeed"].ToString());
        _PayStat.AtkRange = float.Parse(Paydata[_saveData.PayLevel]["AtkRange"].ToString());
        _PayStat.CognitiveRange = float.Parse(Paydata[_saveData.PayLevel]["CognitiveRange"].ToString());

        _SophiaStat.Name = Sophiadata[_saveData.SophiaLevel]["Name"].ToString();
        _SophiaStat.Class = Sophiadata[_saveData.SophiaLevel]["Class"].ToString();
        _SophiaStat.Brood = Sophiadata[_saveData.SophiaLevel]["Brood"].ToString();
        _SophiaStat.Belong = Sophiadata[_saveData.SophiaLevel]["Belong"].ToString();
        _SophiaStat.HP = float.Parse(Sophiadata[_saveData.SophiaLevel]["Hp"].ToString());
        _SophiaStat.Def = float.Parse(Sophiadata[_saveData.SophiaLevel]["Def"].ToString());
        _SophiaStat.Atk = float.Parse(Sophiadata[_saveData.SophiaLevel]["Atk"].ToString());
        _SophiaStat.AtkSpeed = float.Parse(Sophiadata[_saveData.SophiaLevel]["AtkSpeed"].ToString());
        _SophiaStat.MoveSpeed = float.Parse(Sophiadata[_saveData.SophiaLevel]["MoveSpeed"].ToString());
        _SophiaStat.AtkRange = float.Parse(Sophiadata[_saveData.SophiaLevel]["AtkRange"].ToString());
        _SophiaStat.CognitiveRange = float.Parse(Sophiadata[_saveData.SophiaLevel]["CognitiveRange"].ToString());

        _WratihStat.Name = Wratihdata[_saveData.WratihLevel]["Name"].ToString();
        _WratihStat.Class = Wratihdata[_saveData.WratihLevel]["Class"].ToString();
        _WratihStat.Brood = Wratihdata[_saveData.WratihLevel]["Brood"].ToString();
        _WratihStat.Belong = Wratihdata[_saveData.WratihLevel]["Belong"].ToString();
        _WratihStat.HP = float.Parse(Wratihdata[_saveData.WratihLevel]["Hp"].ToString());
        _WratihStat.Def = float.Parse(Wratihdata[_saveData.WratihLevel]["Def"].ToString());
        _WratihStat.Atk = float.Parse(Wratihdata[_saveData.WratihLevel]["Atk"].ToString());
        _WratihStat.AtkSpeed = float.Parse(Wratihdata[_saveData.WratihLevel]["AtkSpeed"].ToString());
        _WratihStat.MoveSpeed = float.Parse(Wratihdata[_saveData.WratihLevel]["MoveSpeed"].ToString());
        _WratihStat.AtkRange = float.Parse(Wratihdata[_saveData.WratihLevel]["AtkRange"].ToString());
        _WratihStat.CognitiveRange = float.Parse(Wratihdata[_saveData.WratihLevel]["CognitiveRange"].ToString());

        _ZippoStat.Name = Zippodata[_saveData.ZippoLevel]["Name"].ToString();
        _ZippoStat.Class = Zippodata[_saveData.ZippoLevel]["Class"].ToString();
        _ZippoStat.Brood = Zippodata[_saveData.ZippoLevel]["Brood"].ToString();
        _ZippoStat.Belong = Zippodata[_saveData.ZippoLevel]["Belong"].ToString();
        _ZippoStat.HP = float.Parse(Zippodata[_saveData.ZippoLevel]["Hp"].ToString());
        _ZippoStat.Def = float.Parse(Zippodata[_saveData.ZippoLevel]["Def"].ToString());
        _ZippoStat.Atk = float.Parse(Zippodata[_saveData.ZippoLevel]["Atk"].ToString());
        _ZippoStat.AtkSpeed = float.Parse(Zippodata[_saveData.ZippoLevel]["AtkSpeed"].ToString());
        _ZippoStat.MoveSpeed = float.Parse(Zippodata[_saveData.ZippoLevel]["MoveSpeed"].ToString());
        _ZippoStat.AtkRange = float.Parse(Zippodata[_saveData.ZippoLevel]["AtkRange"].ToString());
        _ZippoStat.CognitiveRange = float.Parse(Zippodata[_saveData.ZippoLevel]["CognitiveRange"].ToString());


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

    Debug.Log("VeriyStat 불러오기 완료");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
