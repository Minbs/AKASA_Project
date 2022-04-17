using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Ranges
{
	Self,
	OneUnit,
	Node,
	Infinite,
}


public enum Notes
{
	StatChange,
	Condition, //�����̻�
	EnhanceNextBaseAttack //���� ��Ÿ ��ȭ
}

public enum Dependencies //����
{
	None
}

[CreateAssetMenu(fileName = " SkillAbility", menuName = "ScriptableObject/SkillAbility", order = int.MaxValue)]
public class SkillAbilityType : ScriptableObject
{
	public Ranges rangeType;
	public Dependencies dependency;
	public Notes note;
	public StatusEffect statusEffect;
}