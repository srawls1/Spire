using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : AoEProjectile
{
	[SerializeField] private float freezeDuration;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		DealDamage(collision);
	}

	protected override void DealDamage(Collider2D hitBox)
	{
		base.DealDamage(hitBox);
		// TODO add freeze status effect
		// TODO check if its a torch, if so, put it out
	}
}
