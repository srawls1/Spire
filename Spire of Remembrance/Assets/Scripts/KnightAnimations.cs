using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimations : EntityAnimations
{
	private KnightMovement movement;

	private new void Awake()
	{
		base.Awake();
		movement = GetComponent<KnightMovement>();
	}

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
		Weapon weapon = movement.weapon;

		animator.Play(weapon.attackAnimation + directionStringForState(state));
		Debug.Log("Doing the attack animation");
		weapon.gameObject.SetActive(true);

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
		currentState = returnToState;
		if (queuedAction.HasValue)
		{
			PlayAnimation(queuedAction.Value);
			queuedAction = null;
		}
	}
}
