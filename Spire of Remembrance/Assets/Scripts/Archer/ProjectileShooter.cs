using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
	[SerializeField] private Projectile projectile;
	[SerializeField] private Transform projectileSpawnPoint;

	public Projectile SpawnProjectile()
	{
		return Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation) as Projectile;
	}
}
