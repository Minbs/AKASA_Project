using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class StatusEffect
{

	public enum StatusEffects //�����̻�
    {
		None,
		Poison
    }

	public StatusEffects statusEffect;
}