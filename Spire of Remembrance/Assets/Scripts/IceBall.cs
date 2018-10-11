using System;
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
		InflictFreezeStatus(hitBox.gameObject);
		Torch torch = hitBox.GetComponent<Torch>();
		if (torch != null)
		{
			torch.lit = false;
		}
	}

	private void InflictFreezeStatus(GameObject obj)
	{
		BurnStatus burn = obj.GetComponent<BurnStatus>();
		if (burn != null)
		{
			burn.duration = 0f;
			return;
		}

		FreezeStatus freeze = obj.GetComponent<FreezeStatus>();
		if (freeze != null)
		{
			freeze.duration = Mathf.Max(freeze.duration, freezeDuration);
			return;
		}
		else
		{
			freeze = obj.AddComponent<FreezeStatus>();
			freeze.duration = freezeDuration;
		}
	}
}
