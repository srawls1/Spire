using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Damageable
{
	#region Editor Fields

	[SerializeField] private float frontArmor;
	[SerializeField] private float backArmor;
	[SerializeField] private float frontPoise;
	[SerializeField] private float backPoise;
	[SerializeField] private int maxHealth;

	#endregion // Editor Fields

	#region Non-Editor Fields

	EntityAnimations animations;
	Movement movement;
	Rigidbody2D rigidBody;
	int currentHealth;

	#endregion // Non-Editor Fields

	#region Unity Functions

	private void Awake()
	{
		currentHealth = maxHealth;
		animations = GetComponent<EntityAnimations>();
		movement = GetComponent<Movement>();
		rigidBody = GetComponent<Rigidbody2D>();
	}

	#endregion // Unity Functions

	#region Override Functions

	public override void TakeDamage(int damage, Vector3 damagerPosition, float force)
	{
		Vector3 positionDif = damagerPosition - transform.position;

		bool fromFront = isInFront(positionDif);
		float armor = fromFront ? frontArmor : backArmor;
		float poise = fromFront ? frontPoise : backPoise;

		armor *= damage;
		damage -= Mathf.RoundToInt(armor);

		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			Die();
		}
		if (currentHealth > maxHealth)
		{
			currentHealth = maxHealth;
		}

		float knockback = Mathf.Max(force - poise, 0f);
		if (knockback > 0)
		{
			animations.Stagger(movement.Facing);
		}
		rigidBody.velocity = positionDif * -knockback;
		HealthChanged(currentHealth, maxHealth);
	}

	#endregion // Override Functions

	#region Private Functions

	private bool isInFront(Vector3 positionDif)
	{
		switch (movement.Facing)
		{
			case Facing.up:
				return positionDif.y > 0f;
			case Facing.down:
				return positionDif.y < 0f;
			case Facing.left:
				return positionDif.x < 0;
			case Facing.right:
				return positionDif.x > 0;
			default:
				return false;
		}
	}

	private void Die()
	{
		currentHealth = 0;
		animations.Die(movement.Facing);

		if (movement.CurrentController is CharacterController)
		{
			CharacterController controller = movement.CurrentController as CharacterController;
			controller.Deposess();
		}
	}

	#endregion // Private Functions
}
