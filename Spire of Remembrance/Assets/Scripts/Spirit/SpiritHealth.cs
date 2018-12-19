using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritHealth : EnemyHealth
{
	#region Editor Fields
	
	[SerializeField, Tooltip("Scaled damage/second from light. Light is measured on a scale of [0, 1]")]
	private AnimationCurve damageFromLight;
	[SerializeField] private float lightDamageMultiplier;
	[SerializeField] private float bodyDamagePortion;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private EnemyHealth m_possessedBody;
	private int bodysLastHealth;
	private Coroutine currentLightDamage;

	#endregion // Non-Editor Fields

	#region Properties

	public EnemyHealth possessedBody
	{
		get
		{
			return m_possessedBody;
		}
		set
		{
			if (m_possessedBody != null)
			{
				m_possessedBody.OnHealthChanged -= TakeDamageFromBody;
			}

			m_possessedBody = value;
			if (m_possessedBody == null)
			{
				currentLightDamage = StartCoroutine(lightDamage());
			}
			else
			{
				bodysLastHealth = m_possessedBody.currentHealth;
				m_possessedBody.OnHealthChanged += TakeDamageFromBody;
			}
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Start()
	{
		currentLightDamage = StartCoroutine(lightDamage());
	}

	#endregion // Unity Functions

	#region Private Functions

	private IEnumerator lightDamage()
	{
		while (possessedBody == null)
		{
			float lightLevel = 0f; // TODO - Implement this Black box
			float damage = damageFromLight.Evaluate(lightLevel) * Time.deltaTime * lightDamageMultiplier;
			TakeDamage(Mathf.RoundToInt(damage), transform.position, 0f);
			yield return null;
		}
	}

	private void TakeDamageFromBody(int currentHealth, int maxHealth)
	{
		int bodyDamage = bodysLastHealth - currentHealth;
		TakeDamage(Mathf.RoundToInt(bodyDamage * bodyDamagePortion), transform.position, 0f);
	}

	protected override void Die()
	{
		// TODO trigger game over screen
		base.Die();
	}

	#endregion // Private Functions
}
