using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerProjectile : Projectile
{
	[SerializeField] private float effectRadius;
	[SerializeField] private float effectDuration;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.IsChildOf(instigator.transform))
		{
			return;
		}

		Damageable target = collision.GetComponent<Damageable>();
		if (target != null)
		{
			MarkedStatus.InflictMarkedStatus(target.gameObject, effectRadius, effectDuration);
		}

		Destroy(gameObject);
	}
}
