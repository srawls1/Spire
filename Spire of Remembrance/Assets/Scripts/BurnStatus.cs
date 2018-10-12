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

	public static void InflictBurnStatus(GameObject obj, float burnDuration, float damagePerSecond)
	{
		FreezeStatus freeze = obj.GetComponent<FreezeStatus>();
		if (freeze != null)
		{
			freeze.duration = 0f;
			return;
		}

		BurnStatus burn = obj.GetComponent<BurnStatus>();
		if (burn != null)
		{
			if (burnDuration > burn.duration)
			{
				burn.duration = burnDuration;
				burn.damagePerSecond = damagePerSecond;
			}
		}
		else
		{
			burn = obj.AddComponent<BurnStatus>();
			burn.duration = burnDuration;
			burn.damagePerSecond = damagePerSecond;
		}
	}
}
