using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimations : EntityAnimations
{
	#region Editor Fields

	[SerializeField] private WeaponData m_weaponData;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private Weapon weapon;

	#endregion // Non-Editor Fields

	#region Properties

	public WeaponData weaponData
	{
		get
		{
			return m_weaponData;
		}
		set
		{
			weapon.data = value;
		}
	}

	#endregion // Properties

	#region Unity Functions

	private new void Awake()
	{
		base.Awake();
		weapon = GetComponentInChildren<Weapon>();
	}

	private void Start()
	{
		weaponData = weaponData;
		weapon.gameObject.SetActive(false);
	}

	#endregion // Unity Functions

	#region Private Functions

	private string directionStringForState(Animations state)
	{
		switch (state)
		{
			case Animations.AttackRight:
				return "Right";
			case Animations.AttackUp:
				return "Up";
			case Animations.AttackLeft:
				return "Left";
			case Animations.AttackDown:
				return "Down";
			default:
				return "Right";
		}
	}

	protected override IEnumerator AttackRoutine(Animations state)
	{
		animator.Play(weapon.attackAnimation + directionStringForState(state));
		weapon.gameObject.SetActive(true);
		movement.enabled = false;

		float centralAngle = getAngle(state);
		float minAngle = centralAngle - weapon.attackSweepAngle / 2;
		float maxAngle = centralAngle + weapon.attackSweepAngle / 2;

		float timePassed = 0f;
		while (timePassed < 1f)
		{
			timePassed += Time.deltaTime / weapon.attackDuration;
			float currentAngle = Mathf.Lerp(minAngle, maxAngle, timePassed);
			weapon.transform.parent.localRotation = Quaternion.Euler(0, 0, currentAngle);
			yield return null;
		}

		weapon.gameObject.SetActive(false);
		movement.enabled = true;
		currentState = returnToState;
		if (queuedAction.HasValue)
		{
			PlayAnimation(queuedAction.Value);
			queuedAction = null;
		}
	}

	#endregion // Private Functions
}
