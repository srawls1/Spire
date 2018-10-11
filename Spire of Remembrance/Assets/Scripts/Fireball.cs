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
		InflictBurnStatus(hitBox.gameObject);
		Torch torch = hitBox.GetComponent<Torch>();
		if (torch != null)
		{
			torch.lit = true;
		}
	}

	private void InflictBurnStatus(GameObject obj)
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
