using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDamageArgs
{
	public float duration;
	public int damage;

	public IceDamageArgs(float dur, int dmg = 0)
	{
		duration = dur;
		damage = dmg;
	}
}

public class IceBall : AoEProjectile
{
	[SerializeField] private float freezeDuration;

	protected override void DealDamage(Collider2D hitBox)
	{
		AITarget target = hitBox.GetComponentInParent<AITarget>();
		if (target != null && !AITarget.FactionsHostile(instigatorAlignment, target.alignment))
		{
			return;
		}
		base.DealDamage(hitBox);
		hitBox.gameObject.SendMessage("OnIceDamage",
			new IceDamageArgs(freezeDuration, damage),
			SendMessageOptions.DontRequireReceiver);
	}
}
