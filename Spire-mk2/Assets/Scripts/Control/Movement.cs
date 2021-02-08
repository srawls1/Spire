using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Movement : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] protected float maxWalkSpeed;
	[SerializeField] protected float walkAcceleration;
	[SerializeField] protected float walkDecceleration;

	#endregion // Editor Fields

	#region Non-Editor Fields

	new protected Rigidbody2D rigidbody;
	new protected Collider2D collider;
	protected Animator animator;
	protected Vector2 moveInput;
	protected Vector2 lookInput;
	
	//private GameObject currentInteractionHit;

	#endregion // Non-Editor Fields

	#region Properties

	public virtual bool canAttack
	{
		get
		{
			return true;
		}
	}

	#endregion // Properties

	#region Unity Functions

	protected void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		collider = GetComponent<Collider2D>();
	}

	protected void Update()
	{
		Walk(moveInput);
		Look(lookInput);
	}

	#endregion // Unity Functions

	#region Input Wrappers

	public virtual void Walk(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}

	public virtual void Look(InputAction.CallbackContext context)
	{
		lookInput = context.ReadValue<Vector2>();
	}

	public virtual void Attack(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				if (context.interaction is TapInteraction)
				{
					Attack();
				}
				else if (context.interaction is HoldInteraction)
				{
					SecondaryAttack();
				}
				break;
		}
	}

	#endregion // Input Wrappers

	#region Implementations

	public virtual void Walk(Vector2 input)
	{
		if (!enabled)
		{
			rigidbody.velocity = Vector2.zero;
			return;
		}
		
		Vector2 velocity = getUpdatedVelocity(rigidbody.velocity, input);
		rigidbody.velocity = velocity;

		animator.SetFloat("Speed", velocity.magnitude);
		Look(input);
	}

	public void Look(Vector2 input)
	{
		if (input.magnitude > 0.5f)
		{
			float facing = Mathf.Rad2Deg * Mathf.Atan2(input.y, input.x);
			if (facing < -22.5f) facing += 360;
			animator.SetFloat("Facing", facing);
		}
	}

	public void Attack()
	{
		if (!enabled)
		{
			return;
		}

		animator.SetTrigger("Attack");
	}

	public void SecondaryAttack()
	{
		if (!enabled)
		{
			return;
		}

		animator.SetTrigger("SecondaryAttack");
	}

	#endregion // Implementations

	#region Private Functions

	protected Vector2 getUpdatedVelocity(Vector2 currentVelocity, Vector2 input)
	{
		Vector2 velocity = currentVelocity;
		if (input.magnitude > 1f)
		{
			input.Normalize();
		}

		Vector2 acceleration = input * walkAcceleration;

		if (DecelerationRequired(input.x, velocity.x / maxWalkSpeed))
		{
			acceleration.x = -Mathf.Sign(velocity.x) * walkDecceleration;
		}
		if (DecelerationRequired(input.y, velocity.y / maxWalkSpeed))
		{
			acceleration.y = -Mathf.Sign(velocity.y) * walkDecceleration;
		}

		velocity += acceleration * Time.deltaTime;

		// Cap it at max speed
		if (velocity.x > maxWalkSpeed)
		{
			velocity.x = maxWalkSpeed;
		}
		if (velocity.y > maxWalkSpeed)
		{
			velocity.y = maxWalkSpeed;
		}

		// If input is zero and our velocity has crossed zero, no acceleration
		if (Mathf.Approximately(input.x, 0f) && velocity.x * rigidbody.velocity.x < 0f)
		{
			velocity.x = 0f;
		}
		if (Mathf.Approximately(input.y, 0f) && velocity.y * rigidbody.velocity.y < 0f)
		{
			velocity.y = 0f;
		}

		return velocity;
	}

	protected virtual int getInteractionLayermask()
	{
		return Physics2D.GetLayerCollisionMask(gameObject.layer) | LayerMask.NameToLayer("Interactable");
	}

	bool DecelerationRequired(float input, float velocity)
	{
		// Test if they have opposite signs, i.e. opposite directions
		if (input * velocity < 0)
		{
			return true;
		}

		return (Mathf.Abs(input) < Mathf.Abs(velocity));
	}

	#endregion // Private Functions
}
