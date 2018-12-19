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
	[SerializeField] protected int m_maxHealth;

	#endregion // Editor Fields

	#region Non-Editor Fields

	EntityAnimations animations;
	Movement movement;
	Rigidbody2D rigidBody;
	private int m_currentHealth;

	#endregion // Non-Editor Fields

	#region Properties

	public int maxHealth
	{
		get
		{
			return m_maxHealth;
		}
	}

	public int currentHealth
	{
		get
		{
			return m_currentHealth;
		}
		private set
		{
			m_currentHealth = value;
		}
	}

	#endregion // Properties

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
		rigidBody.AddForce(positionDif * -knockback, ForceMode2D.Impulse);
		HealthChanged(currentHealth, maxHealth);
	}

	public void Heal(int healAmount)
	{
		currentHealth += healAmount;
		if (currentHealth > maxHealth)
		{
			currentHealth = maxHealth;
		}
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

	protected virtual void Die()
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
