using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class StatusEffect
{

	public enum StatusEffects //상태이상
    {
		None,
		Poison
    }

	public StatusEffects statusEffect;
}