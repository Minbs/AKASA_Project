using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    None,
    ATK,
    DEF,
    AttackSpeed,
    MoveSpeed
}

[System.Serializable]
public class SkillAbility 
{
    public SkillAbilityType abilityType;
    public float power;
    public float duration;
    public StatType statType;
    public List<Node> rangeNodes = new List<Node>();
    public bool excludeSelf;

}
