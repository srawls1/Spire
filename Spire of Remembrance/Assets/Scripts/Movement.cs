using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing
{
	right, up, left, down
}

public abstract class Movement : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private float maxWalkSpeed;
	[SerializeField] private float walkAcceleration;
	[SerializeField] private float walkDecceleration;
	[SerializeField] private float interactionDistance;

	#endregion // Editor Fields

	#region Non-Editor Fields

	protected Rigidbody2D rigidBody;
	protected EntityAnimations animator;
	protected Controller m_current;
	private GameObject currentInteractionHit;

	#endregion // Non-Editor Fields

	#region Properties

	public Controller CurrentController
	{
		get
		{
			return m_current;
		}
		set
		{
			if (m_current != null)
			{
				m_current.enabled = false;
			}
			m_current = value;
			if (m_current != null)
			{
				m_current.enabled = true;
			}
		}
	}

	public Facing Facing
	{
		get; protected set;
	}

	#endregion // Properties

	#region Events

	public event Action<Interaction[]> OnNewInteractables;

	#endregion // Events

	#region Unity Functions

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<EntityAnimations>();
	}

	private void Update()
	{
		Vector2 direction = Vector2.zero;
		switch (Facing)
		{
			case Facing.left:
				direction = Vector2.left;
				break;
			case Facing.right:
				direction = Vector2.right;
				break;
			case Facing.up:
				direction = Vector2.up;
				break;
			case Facing.down:
				direction = Vector2.down;
				break;
		}

		Debug.DrawRay(transform.position, direction * interactionDistance);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, interactionDistance, getInteractionLayermask());
		Collider2D c = hit.collider;
		GameObject obj = c != null ? c.gameObject : null;
		if (obj != currentInteractionHit)
		{
			currentInteractionHit = obj;
			Interactable interactable = obj != null ? obj.GetComponent<Interactable>() : null;
			if (interactable == null)
			{
				if (OnNewInteractables != null)
				{
					OnNewInteractables(null);
				}
			}
			else
			{
				if (OnNewInteractables != null)
				{
					OnNewInteractables(interactable.interactions);
				}
			}
		}
	}

	#endregion // Unity Functions

	#region Public Functions

	public void Walk(Vector2 input)
	{
		Vector2 velocity = rigidBody.velocity;
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
		if (Mathf.Approximately(input.x, 0f) && velocity.x * rigidBody.velocity.x < 0f)
		{
			velocity.x = 0f;
		}
		if (Mathf.Approximately(input.y, 0f) && velocity.y * rigidBody.velocity.y < 0f)
		{
			velocity.y = 0f;
		}

		rigidBody.velocity = velocity;
		setFacing(input);
		animator.UpdateMovementAnim(Facing, velocity.magnitude);
	}

	public void RefreshInteracable()
	{
		currentInteractionHit = null;
	}

	public void Attack()
	{
		animator.Attack(Facing);
	}

	#endregion // Public Functions

	#region Private Functions

	protected abstract int getInteractionLayermask();

	bool DecelerationRequired(float input, float velocity)
	{
		// Test if they have opposite signs, i.e. opposite directions
		if (input * velocity < 0)
		{
			return true;
		}

		return (Mathf.Abs(input) < Mathf.Abs(velocity));
	}

	protected void setFacing(Vector2 velocity)
	{
		if (velocity.magnitude < 0.1f)
		{
			return;
		}

		if (Math.Abs(velocity.x) > Math.Abs(velocity.y))
		{
			if (velocity.x > 0.1f)
			{
				Facing = Facing.right;
			}
			else if (velocity.x < -0.1f)
			{
				Facing = Facing.left;
			}
		}
		else
		{
			if (velocity.y > 0.1f)
			{
				Facing = Facing.up;
			}
			else if (velocity.y < -0.1f)
			{
				Facing = Facing.down;
			}
		}
	}

	#endregion // Private Functions
}
