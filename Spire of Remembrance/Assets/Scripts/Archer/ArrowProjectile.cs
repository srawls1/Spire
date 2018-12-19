﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : Projectile
{
	#region Editor Fields

	[SerializeField] private int baseDamage;
	[SerializeField] private float force;
	[SerializeField] private int burningDamage;
	[SerializeField] private float burningDuration;
	[SerializeField] private float burningDamagePerSecond;

	#endregion // Editor Fields

	#region Other Fields

	private bool m_burning;
	private bool burning
	{
		get
		{
			return m_burning;
		}
		set
		{
			m_burning = value;
			// TODO - fire particle effects
		}
	}

	#endregion // Other Fields

	#region Unity Functions

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.IsChildOf(instigator.transform))
		{
			return;
		}

		Torch torch = collision.GetComponent<Torch>();
		if (torch != null)
		{
			if (torch.lit)
			{
				burning = true;
				return;
			}
			else if (burning)
			{
				torch.lit = true;
			}
			return;
		}

		Damageable damageable = collision.GetComponent<Damageable>();
		if (damageable == null)
		{
			Destroy(gameObject);
			return;
		}

		damageable.TakeDamage(burning ? burningDamage : baseDamage,
			transform.position, force);
		if (burning)
		{
			BurnStatus.InflictBurnStatus(damageable.gameObject, burningDuration, burningDamagePerSecond);
		}
		Destroy(gameObject);
	}

	#endregion // Unity Functions
}