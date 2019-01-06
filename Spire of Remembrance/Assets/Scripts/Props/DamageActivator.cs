using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageActivationMode
{
	Permanent,
	Toggle,
	Timed
}

public class DamageActivator : Damageable
{
	[SerializeField] private int minDamage = 0;
	[SerializeField] private float minForce = 0f;
	[SerializeField] private DamageActivationMode mode;
	[SerializeField] private float timeLimit = 0f;

	private Activator activator;
	private bool activated;

	private void Awake()
	{
		activator = GetComponent<Activator>();
	}

	public override void TakeDamage(int damage, Vector3 damagerPosition, float force)
	{
		if (damage >= minDamage && force >= minForce)
		{
			if (mode == DamageActivationMode.Toggle)
			{
				if (activated)
				{
					activated = false;
					activator.Deactivate();
				}
				else
				{
					activated = true;
					activator.Activate();
				}
			}
			else if (!activated)
			{
				activator.Activate();
				activated = true;
				if (mode == DamageActivationMode.Timed)
				{
					StartCoroutine(DeactivateAfterTimeLimit());
				}
			}
			
		}
	}

	private IEnumerator DeactivateAfterTimeLimit()
	{
		yield return new WaitForSeconds(timeLimit);
		activator.Deactivate();
		activated = false;
	}
}
