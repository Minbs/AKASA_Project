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
    // Start is called before the first frame update
    void Start()
    {
        string StringSavedata = File.ReadAllText(Application.dataPath + "/SaveData.json");
        SaveData _saveData = JsonUtility.FromJson<SaveData>(StringSavedata);

        List<Dictionary<string, object>> Veritydata = CSVReader.Read("Verity_Stat_Table");
        Stat _VeriyStat = new Stat();

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

        VeriyStat = _VeriyStat;
        Debug.Log("VeriyStat 불러오기 완료");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
