using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SkillStatus
{
    public string Name;
    public float Level;
    public float Cooltime;
    public float SkillCoefficient;
    public float BasicDamage;
}
public class CSV_SkillStatus : MonoBehaviour
{
    public SkillStatus ChargingShot_Level1;
    public SkillStatus ChargingShot_Level2;
    public SkillStatus ChargingShot_Level3;
    public SkillStatus ChargingShot_Level4;
    public SkillStatus ChargingShot_Level5;
    public SkillStatus ChargingShot_Level6;
    public SkillStatus ChargingShot_Level7;
    public SkillStatus ChargingShot_Level8;
    public SkillStatus ChargingShot_Level9;
    public SkillStatus ChargingShot_Level10;
    public SkillStatus ChargingShot_Level11;
    public SkillStatus ChargingShot_Level12;
    public SkillStatus ChargingShot_Level13;
    public SkillStatus ChargingShot_Level14;
    public SkillStatus ChargingShot_Level15;


    // Start is called before the first frame update
    void Start()
    {

        List<Dictionary<string, object>> ChargingShotList = CSVReader.Read("LevelDesignDataList.xlsx - VeritySkillStatus");
        
        SkillStutus(ChargingShot_Level1, ChargingShotList, 0);
        SkillStutus(ChargingShot_Level2, ChargingShotList, 1);
        SkillStutus(ChargingShot_Level3, ChargingShotList, 2);
        SkillStutus(ChargingShot_Level4, ChargingShotList, 3);
        SkillStutus(ChargingShot_Level5, ChargingShotList, 4);
        SkillStutus(ChargingShot_Level6, ChargingShotList, 5);
        SkillStutus(ChargingShot_Level7, ChargingShotList, 6);
        SkillStutus(ChargingShot_Level8, ChargingShotList, 7);
        SkillStutus(ChargingShot_Level9, ChargingShotList, 8);
        SkillStutus(ChargingShot_Level10, ChargingShotList, 9);
        SkillStutus(ChargingShot_Level11, ChargingShotList, 10);
        SkillStutus(ChargingShot_Level12, ChargingShotList, 11);
        SkillStutus(ChargingShot_Level13, ChargingShotList, 12);
        SkillStutus(ChargingShot_Level14, ChargingShotList, 13);
        SkillStutus(ChargingShot_Level15, ChargingShotList, 14);
        
    }
    
    void SkillStutus(SkillStatus skill, List<Dictionary<string, object>> SkillList, int levelNum)
    {
        skill.Name = SkillList[levelNum]["Name"].ToString();
        skill.Level = float.Parse(SkillList[levelNum]["Level"].ToString());
        skill.Cooltime = float.Parse(SkillList[levelNum]["Cooltime"].ToString());
        skill.SkillCoefficient = float.Parse(SkillList[levelNum]["SkillCoefficient"].ToString());
        skill.BasicDamage = float.Parse(SkillList[levelNum]["BasicDamage"].ToString());

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
