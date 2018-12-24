using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoEProjectile : Projectile
{
	[SerializeField] private float detonateAfterSeconds;
	[SerializeField] private float radius;
	[SerializeField] protected int damage;
	[SerializeField] private int force;

	new IEnumerator Start()
	{
		base.Start();

		yield return new WaitForSeconds(detonateAfterSeconds);

		Animator animator = GetComponent<Animator>();
		animator.Play("Detonate");
		Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
		if (rigidBody != null)
		{
			rigidBody.velocity = Vector2.zero;
		}

		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, Physics2D.GetLayerCollisionMask(gameObject.layer));
		for (int i = 0; i < colliders.Length; ++i)
		{
			DealDamage(colliders[i]);
		}
	}

	protected virtual void DealDamage(Collider2D hitBox)
	{
		// A shooter won't damage themself
		if (hitBox.transform.IsChildOf(instigator.transform))
		{
			return;
		}

		Damageable damageable = hitBox.GetComponent<Damageable>();
		if (damageable == null)
		{
			return;
		}

		damageable.TakeDamage(damage, transform.position, force);
	}
}
