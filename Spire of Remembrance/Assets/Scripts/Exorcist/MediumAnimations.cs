using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumAnimations : EntityAnimations
{
	#region Editor Fields

	[SerializeField] private float attackRadius;
	[SerializeField] private float attackTime;
	[SerializeField] private float attackDuration;

	#endregion // Editor Fields

	#region Overrides

	protected override IEnumerator AttackRoutine(Animations state)
	{
		animator.Play(state.ToString());
		showWarningRing();
		movement.enabled = false;

		yield return new WaitForSeconds(attackTime);

		showAttackRing();
		doAttack();

		yield return new WaitForSeconds(attackDuration - attackTime);

		movement.enabled = true;
		if (queuedAction.HasValue)
		{
			PlayAnimation(queuedAction.Value);
			queuedAction = null;
		}
	}

	#endregion // Overrides

	#region Helper Functions

	private void doAttack()
	{
		Collider2D[] intersectedColliders = Physics2D.OverlapCircleAll(transform.position, attackRadius, Physics2D.AllLayers);
		for (int i = 0; i < intersectedColliders.Length; ++i)
		{
			Movement movement = intersectedColliders[i].GetComponent<Movement>();
			if (movement == null)
			{
				continue;
			}

			CharacterController character = movement.CurrentController as CharacterController;
			if (character == null)
			{
				continue;
			}

			character.TurnPhysical();
		}
	}

	private void showAttackRing()
	{
		Debug.Log("Showing attack ring");
		// TODO
	}

	private void showWarningRing()
	{
		Debug.Log("Showing warning ring");
		// TODO
	}

	#endregion // Helper Functions
}
