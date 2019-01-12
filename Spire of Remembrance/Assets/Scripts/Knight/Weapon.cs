using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	#region Non-Editor Fields

	private new PolygonCollider2D collider;
	private new SpriteRenderer renderer;
	private TrailRenderer trail;
	private HashSet<Damageable> alreadyHitTargets;
	private WeaponData m_data;

	#endregion // Non-Editor Fields

	#region Properties

	public int damage
	{
		get; protected set;
	}

	public float attackForce
	{
		get; protected set;
	}

	public string attackAnimation
	{
		get; protected set;
	}

	public float attackSweepAngle
	{
		get; protected set;
	}

	public float attackDuration
	{
		get; protected set;
	}

	public WeaponData data
	{
		get
		{
			return m_data;
		}
		set
		{
			m_data = value;
			damage = value.damage;
			attackForce = value.attackforce;
			attackAnimation = value.attackAnimation;
			attackSweepAngle = value.attackSweepAngle;
			attackDuration = value.attackDuration;
			renderer.sprite = value.weaponSprite;
			trail.startColor = value.trailColor;
			trail.endColor = value.trailColor;
			RefreshPolygonCollider();
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Awake()
	{
		collider = GetComponent<PolygonCollider2D>();
		renderer = GetComponent<SpriteRenderer>();
		trail = GetComponentInChildren<TrailRenderer>();
		alreadyHitTargets = new HashSet<Damageable>();
	}

	private void OnEnable()
	{
		alreadyHitTargets.Clear();
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		AITarget self = GetComponentInParent<AITarget>();
		Alignment instigatorAlignment = self != null ? self.alignment : Alignment.Wildcard;
		AITarget target = collision.GetComponentInParent<AITarget>();
		if (target != null && !AITarget.FactionsHostile(instigatorAlignment, target.alignment))
		{
			return;
		}

		Damageable damageable = collision.GetComponent<Damageable>();
		if (damageable == null || alreadyHitTargets.Contains(damageable))
		{
			return;
		}

		alreadyHitTargets.Add(damageable);

		damageable.TakeDamage(damage, transform.position, attackForce);
	}

	private void RefreshPolygonCollider()
	{
		Sprite sprite = renderer.sprite;
		collider.SetPath(0, sprite.vertices);
	}

	#endregion // Unity Functions
}
