using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Shooter", menuName = "Custom/Projectile Shooter", order = 1)]
public class ProjectileShooterData : ScriptableObject
{
	#region Editor Fields

	[SerializeField] private Projectile m_projectilePrefab;
	[SerializeField] private Sprite m_shooterSprite;
	[SerializeField] private float m_attackReleaseTime;
	[SerializeField] private float m_attackDuration;

	#endregion // Editor Fields

	#region Properties

	public Projectile projectilePrefab
	{
		get
		{
			return m_projectilePrefab;
		}
	}

	public Sprite shooterSprite
	{
		get
		{
			return m_shooterSprite;
		}
	}

	public float attackReleaseTime
	{
		get
		{
			return m_attackReleaseTime;
		}
	}

	public float attackDuration
	{
		get
		{
			return m_attackDuration;
		}
	}

	#endregion // Properties
}
