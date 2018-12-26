using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private Transform projectileSpawnPoint;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private new SpriteRenderer renderer;
	private ProjectileShooterData m_data;

	#endregion // Non-Editor Fields

	#region Properties

	public Projectile projectile
	{
		get; protected set;
	}

	public float releaseTime
	{
		get; protected set;
	}

	public float attackDuration
	{
		get; protected set;
	}

	public ProjectileShooterData data
	{
		get
		{
			return m_data;
		}
		set
		{
			m_data = value;
			projectile = value.projectilePrefab;
			releaseTime = value.attackReleaseTime;
			attackDuration = value.attackDuration;
			renderer.sprite = value.shooterSprite;
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
	}

	#endregion // Unity Functions

	#region Public Functions

	public Projectile SpawnProjectile()
	{
		return Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation) as Projectile;
	}

	#endregion // Public Functions
}
