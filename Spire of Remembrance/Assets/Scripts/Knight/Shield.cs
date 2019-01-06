using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
	[SerializeField] private float defenseIncrease;
	[SerializeField] private float poiseIncrease;

	private EnemyHealth owningHealth;
	private float baseDefense;
	private float basePoise;

	private void Awake()
	{
		owningHealth = GetComponentInParent<EnemyHealth>();
	}

	private void OnEnable()
	{
		baseDefense = owningHealth.frontArmor;
		basePoise = owningHealth.frontPoise;
		owningHealth.frontArmor = Mathf.Min(owningHealth.frontArmor + defenseIncrease, 1f);
		owningHealth.frontPoise = owningHealth.frontPoise + poiseIncrease;
	}

	private void OnDisable()
	{
		owningHealth.frontArmor = baseDefense;
		owningHealth.frontPoise = basePoise;
	}
}
