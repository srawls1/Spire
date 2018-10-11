using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnStatus : MonoBehaviour
{
	public float damagePerSecond;
	public float duration;

	void Start()
	{
		StartCoroutine(DamageRoutine());
	}
	
	private IEnumerator DamageRoutine()
	{
		yield return null;

		Damageable damageable = GetComponent<Damageable>();
		if (damageable == null)
		{
			Destroy(this);
			yield break;
		}

		// TODO - fire particle effects?

		float damage = 0f;

		while (duration > 0f)
		{
			duration -= Time.deltaTime;
			damage += damagePerSecond * Time.deltaTime;
			if (damage > 1f)
			{
				int damageInt = Mathf.RoundToInt(damage);
				damage -= damageInt;
				damageable.TakeDamage(damageInt, transform.position, 0f);
			}

			yield return null;
		}

		// TODO - destroy particle effects

		Destroy(this);
	}
}
