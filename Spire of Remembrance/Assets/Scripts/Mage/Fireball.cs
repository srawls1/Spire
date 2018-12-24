using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamageArgs
{
	public float duration;
	public float dps;
	public int damage;

	public FireDamageArgs(float dur, float dot, int dmg = 0)
	{
		duration = dur;
		dps = dot;
		damage = dmg;
	}
}

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
		hitBox.gameObject.SendMessage("OnFireDamage",
			new FireDamageArgs(burnDuration, damagePerSecond, damage),
			SendMessageOptions.DontRequireReceiver);
	}
}
