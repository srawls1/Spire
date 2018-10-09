using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAnimations : EntityAnimations
{
	private ProjectileShooter weapon;

	[SerializeField] private float attackDuration;
	[SerializeField] private float projectileReleaseTime;

	new private void Awake()
	{
		base.Awake();
		weapon = GetComponentInChildren<ProjectileShooter>();
		weapon.gameObject.SetActive(false);
	}

	protected override IEnumerator AttackRoutine(Animations state)
	{
		animator.Play(state.ToString());
		weapon.transform.parent.localRotation = Quaternion.Euler(0f, 0f, getAngle(state));
		weapon.gameObject.SetActive(true);

		yield return new WaitForSeconds(projectileReleaseTime);

		weapon.SpawnProjectile().instigator = gameObject;

		yield return new WaitForSeconds(attackDuration - projectileReleaseTime);

		weapon.gameObject.SetActive(false);
	}
}
