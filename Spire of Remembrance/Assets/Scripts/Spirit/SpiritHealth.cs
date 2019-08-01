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
	[SerializeField] private GameOver gameOverScreen;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private EnemyHealth m_possessedBody;
	private int bodysLastHealth;

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
				//m_possessedBody.OnHealthChanged -= TakeDamageFromBody;
			}

			var prevPossessed = m_possessedBody;
			m_possessedBody = value;
			if (m_possessedBody == null)
			{
				if (prevPossessed != null)
				{
					StartCoroutine(lightDamage());
				}
			}
			else
			{
				//bodysLastHealth = m_possessedBody.currentHealth;
				//m_possessedBody.OnHealthChanged += TakeDamageFromBody;
			}
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Start()
	{
		StartCoroutine(lightDamage());
	}

	private void OnDestroy()
	{
		gameOverScreen.gameObject.SetActive(true);
	}

	#endregion // Unity Functions

	#region Public Functions

	public void ResistLight(float duration)
	{
		StartCoroutine(resistLightRoutine(duration));
	}

	#endregion // Public Functions

	#region Private Functions

	private IEnumerator lightDamage()
	{
		Debug.Log("Starting light damage");

		while (possessedBody == null && currentHealth > 0)
		{
			float lightLevel = LightLevel.GetLightLevel(transform.position);
			Debug.Log("LightLevel=" + lightLevel);
			float damage = damageFromLight.Evaluate(lightLevel) * Time.deltaTime * lightDamageMultiplier;
			Debug.Log("Damage=" + damage);
			TakeDamage(damage, transform.position, 0f);
			yield return null;
		}

		Debug.Log("Stopping light damage");
	}

	private IEnumerator resistLightRoutine(float duration)
	{
		float defaultLightDamage = lightDamageMultiplier;
		lightDamageMultiplier = 0f;
		yield return new WaitForSeconds(duration);
		lightDamageMultiplier = defaultLightDamage;
	}

	//private void TakeDamageFromBody(int currentHealth, int maxHealth)
	//{
	//	int bodyDamage = bodysLastHealth - currentHealth;
	//	TakeDamage(Mathf.RoundToInt(bodyDamage * bodyDamagePortion), transform.position, 0f);
	//}

	protected override void Die()
	{
		gameOverScreen.gameObject.SetActive(true);
		base.Die();
	}

	#endregion // Private Functions
}
