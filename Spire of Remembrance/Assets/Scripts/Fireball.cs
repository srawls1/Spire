using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : AoEProjectile
{
	[SerializeField] private float damagePerSecond;
	[SerializeField] private float burnDuration;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		DealDamage(collision);
	}

	protected override void DealDamage(Collider2D hitBox)
	{
		base.DealDamage(hitBox);
		// TODO add burn status effect
		// TODO check if its a torch, if so, light it
	}
}
