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
	[SerializeField] private float teleportWarmUp;
	[SerializeField] private float teleportCoolDown;

	#endregion // Editor Fields

	#region Public Functions

	public Coroutine AnimateTeleport(Vector2 destination)
	{
		return StartCoroutine(teleportRoutine(destination));
	}

	#endregion // Public Functions

	#region Overrides

	protected override IEnumerator AttackRoutine(Animations state)
	{
		Debug.Log("Attacking");
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

	private IEnumerator teleportRoutine(Vector2 destination)
	{
		movement.enabled = false;
		animator.Play("Teleport");
		yield return new WaitForSeconds(teleportWarmUp);
		transform.position = destination;
		yield return new WaitForSeconds(teleportCoolDown);
		movement.enabled = true;
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
