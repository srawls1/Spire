using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EntityAnimations : MonoBehaviour
{
	#region Local Enum

	protected enum Animations
	{
		IdleRight,
		IdleUp,
		IdleLeft,
		IdleDown,
		WalkingRight,
		WalkingUp,
		WalkingLeft,
		WalkingDown,
		AttackRight,
		AttackUp,
		AttackLeft,
		AttackDown,
		StaggerRight,
		StaggerUp,
		StaggerLeft,
		StaggerDown,
		DieRight,
		DieUp,
		DieLeft,
		DieDown,
		FallInPit
	}

	#endregion // Local Enum

	#region Editor Fields

	[SerializeField] protected float deathAnimDuration;
	[SerializeField] protected float fallAnimDuration;

	#endregion // Editor Fields

	#region Non-Editor Fields

	protected Movement movement;
	protected Animator animator;
	protected Animations currentState;
	protected Animations? queuedAction;
	protected Animations returnToState;

	#endregion // Non-Editor Fields

	#region Unity Functions

	protected void Awake()
	{
		movement = GetComponent<Movement>();
		animator = GetComponent<Animator>();
		queuedAction = null;
	}

	#endregion // Unity Functions

	#region Public Functions

	public void UpdateMovementAnim(Facing facing, float speed)
	{
		// Don't interrupt attacks
		if (isAttackState(currentState) || isDeathState(currentState))
		{
			return;
		}

		bool moving = speed > 0.1f;
		if (moving)
		{
			switch (facing)
			{
				case Facing.right:
					PlayAnimation(Animations.WalkingRight);
					break;
				case Facing.up:
					PlayAnimation(Animations.WalkingUp);
					break;
				case Facing.left:
					PlayAnimation(Animations.WalkingLeft);
					break;
				case Facing.down:
					PlayAnimation(Animations.WalkingDown);
					break;
			}
		}
		else
		{
			switch (facing)
			{
				case Facing.right:
					PlayAnimation(Animations.IdleRight);
					break;
				case Facing.up:
					PlayAnimation(Animations.IdleUp);
					break;
				case Facing.left:
					PlayAnimation(Animations.IdleLeft);
					break;
				case Facing.down:
					PlayAnimation(Animations.IdleDown);
					break;
			}
		}
	}

	public void Stagger(Facing facing)
	{
		switch (facing)
		{
			case Facing.right:
				PlayAnimation(Animations.StaggerRight);
				break;
			case Facing.up:
				PlayAnimation(Animations.StaggerUp);
				break;
			case Facing.left:
				PlayAnimation(Animations.StaggerLeft);
				break;
			case Facing.down:
				PlayAnimation(Animations.StaggerDown);
				break;
		}
	}

	public void Attack(Facing facing)
	{
		switch (facing)
		{
			case Facing.right:
				PlayAnimation(Animations.AttackRight);
				break;
			case Facing.up:
				PlayAnimation(Animations.AttackUp);
				break;
			case Facing.left:
				PlayAnimation(Animations.AttackLeft);
				break;
			case Facing.down:
				PlayAnimation(Animations.AttackDown);
				break;
		}
	}

	public void Die(Facing facing)
	{
		switch (facing)
		{
			case Facing.right:
				PlayAnimation(Animations.DieRight);
				break;
			case Facing.up:
				PlayAnimation(Animations.DieUp);
				break;
			case Facing.left:
				PlayAnimation(Animations.DieLeft);
				break;
			case Facing.down:
				PlayAnimation(Animations.DieDown);
				break;
		}
		movement.enabled = false;
		Destroy(gameObject, deathAnimDuration);
	}

	public virtual void CleanUpInventoryEvents(InventoryManager playerInventory, InventoryManager bodyInventory)
	{
	}

	public virtual List<InventoryItem> UpdateInventoryWeapons(InventoryManager playerInventory, InventoryManager bodyInventory)
	{
		List<InventoryItem> ret = new List<InventoryItem>();
		WeaponSelectionUI.instance.SetAvailableWeapons(ret);
		WeaponSelectionUI.instance.ShowSelectedWeapon(null);
		return ret;
	}

	public virtual void FallInPit()
	{
		PlayAnimation(Animations.FallInPit);
		movement.enabled = false;
		Destroy(gameObject, fallAnimDuration);
	}

	#endregion // Public Functions

	#region Protected Functions

	protected void PlayAnimation(Animations state)
	{
		if (state == currentState)
		{
			return;
		}

		if (isAttackState(currentState))
		{
			if (isAttackState(state))
			{
				queuedAction = state;
			}
			else
			{
				returnToState = state;
			}
		}
		else
		{
			if (isAttackState(state))
			{
				StartCoroutine(AttackRoutine(state));
			}
			else
			{
				currentState = state;
				animator.Play(state.ToString());
			}
		}
	}

	protected bool isAttackState(Animations state)
	{
		return state == Animations.AttackRight || state == Animations.AttackUp ||
			state == Animations.AttackLeft || state == Animations.AttackDown;
	}

	protected bool isDeathState(Animations state)
	{
		return state == Animations.DieRight || state == Animations.DieUp ||
			state == Animations.DieLeft || state == Animations.DieDown ||
			state == Animations.FallInPit;
	}

	protected virtual IEnumerator AttackRoutine(Animations state)
	{
		yield break;
	}

	protected float getAngle(Animations state)
	{
		switch (state)
		{
			case Animations.IdleRight:
			case Animations.WalkingRight:
			case Animations.AttackRight:
			case Animations.DieRight:
				return 0f;
			case Animations.IdleUp:
			case Animations.WalkingUp:
			case Animations.AttackUp:
			case Animations.DieUp:
				return 90f;
			case Animations.IdleLeft:
			case Animations.WalkingLeft:
			case Animations.AttackLeft:
			case Animations.DieLeft:
				return 180f;
			case Animations.IdleDown:
			case Animations.WalkingDown:
			case Animations.AttackDown:
			case Animations.DieDown:
				return 270f;
			default:
				return 0f;
		}
	}

	#endregion // Protected Functions
}
