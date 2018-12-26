using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAnimations : EntityAnimations
{
	#region Editor Fields

	[SerializeField] private ProjectileShooterData m_shooterData;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private ProjectileShooter weapon;

	#endregion // Non-Editor Fields

	#region Properties

	public ProjectileShooterData shooterData
	{
		get
		{
			return weapon.data;
		}
		set
		{
			weapon.data = value;
		}
	}

	#endregion // Properties

	new private void Awake()
	{
		base.Awake();
		weapon = GetComponentInChildren<ProjectileShooter>();
	}

	private void Start()
	{
		shooterData = m_shooterData;
		weapon.gameObject.SetActive(false);
	}

	protected override IEnumerator AttackRoutine(Animations state)
	{
		animator.Play(state.ToString());
		weapon.transform.parent.localRotation = Quaternion.Euler(0f, 0f, getAngle(state));
		weapon.gameObject.SetActive(true);
		movement.enabled = false;

		yield return new WaitForSeconds(weapon.releaseTime);

		weapon.SpawnProjectile().instigator = gameObject;

		yield return new WaitForSeconds(weapon.attackDuration - weapon.releaseTime);

		weapon.gameObject.SetActive(false);
		movement.enabled = true;
		currentState = returnToState;
		if (queuedAction.HasValue)
		{
			PlayAnimation(queuedAction.Value);
			queuedAction = null;
		}
	}
}
