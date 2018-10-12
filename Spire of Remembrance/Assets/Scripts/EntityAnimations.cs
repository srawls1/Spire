using System.Collections;
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
		AttackDown 
	}

	#endregion // Local Enum

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
		if (isAttackState(currentState))
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
				return 0f;
			case Animations.IdleUp:
			case Animations.WalkingUp:
			case Animations.AttackUp:
				return 90f;
			case Animations.IdleLeft:
			case Animations.WalkingLeft:
			case Animations.AttackLeft:
				return 180f;
			case Animations.IdleDown:
			case Animations.WalkingDown:
			case Animations.AttackDown:
				return 270f;
			default:
				return 0f;
		}
	}

	#endregion // Protected Functions
}
