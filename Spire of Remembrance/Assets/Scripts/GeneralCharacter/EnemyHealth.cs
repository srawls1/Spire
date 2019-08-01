using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Damageable
{
	#region Editor Fields

	[SerializeField] private float m_frontArmor;
	[SerializeField] private float backArmor;
	[SerializeField] private float m_frontPoise;
	[SerializeField] private float backPoise;
	[SerializeField] protected int m_maxHealth;

	#endregion // Editor Fields

	#region Non-Editor Fields

	Possessable possessable;
	EntityAnimations animations;
	Movement movement;
	Rigidbody2D rigidBody;
	private float m_currentHealth;

	#endregion // Non-Editor Fields

	#region Properties

	public int maxHealth
	{
		get
		{
			return m_maxHealth;
		}
	}

	public float currentHealth
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

	public float frontArmor
	{
		get
		{
			return m_frontArmor;
		}
		set
		{
			m_frontArmor = value;
		}
	}

	public float frontPoise
	{
		get
		{
			return m_frontPoise;
		}
		set
		{
			m_frontPoise = value;
		}
	}

	#endregion // Properties

	#region Unity Functions

	protected void Awake()
	{
		possessable = GetComponentInChildren<Possessable>();
		currentHealth = maxHealth;
		animations = GetComponent<EntityAnimations>();
		movement = GetComponent<Movement>();
		rigidBody = GetComponent<Rigidbody2D>();
	}

	#endregion // Unity Functions

	#region Override Functions

	public override void TakeDamage(float damage, Vector3 damagerPosition, float force)
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
		HealthChanged(Mathf.RoundToInt(currentHealth), maxHealth);
	}

	public void OnFireDamage(FireDamageArgs args)
	{
		FreezeStatus freeze = GetComponent<FreezeStatus>();
		if (freeze != null)
		{
			freeze.duration = 0f;
			return;
		}

		BurnStatus burn = GetComponent<BurnStatus>();
		if (burn != null)
		{
			if (args.duration > burn.duration)
			{
				burn.duration = args.duration;
				burn.damagePerSecond = args.dps;
			}
		}
		else
		{
			burn = gameObject.AddComponent<BurnStatus>();
			burn.duration = args.duration;
			burn.damagePerSecond = args.dps;
		}
	}

	public void OnIceDamage(IceDamageArgs args)
	{
		BurnStatus burn = GetComponent<BurnStatus>();
		if (burn != null)
		{
			burn.duration = 0f;
			return;
		}

		FreezeStatus freeze = GetComponent<FreezeStatus>();
		if (freeze != null)
		{
			freeze.duration = Mathf.Max(freeze.duration, args.duration);
			return;
		}
		else
		{
			freeze = gameObject.AddComponent<FreezeStatus>();
			freeze.duration = args.duration;
		}
	}

	public void Heal(int healAmount)
	{
		currentHealth += healAmount;
		if (currentHealth > maxHealth)
		{
			currentHealth = maxHealth;
		}
		HealthChanged(Mathf.RoundToInt(currentHealth), maxHealth);
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
		if (movement.CurrentController is CharacterController)
		{
			CharacterController controller = movement.CurrentController as CharacterController;
			controller.Deposess();
		}
		if (possessable != null)
		{
			Destroy(possessable);
		}
		animations.Die(movement.Facing);
	}

	#endregion // Private Functions
}
