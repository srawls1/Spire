using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing
{
	right, up, left, down
}

public class Movement : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] protected float maxWalkSpeed;
	[SerializeField] protected float walkAcceleration;
	[SerializeField] protected float walkDecceleration;
	[SerializeField] private float interactionDistance;
	//[SerializeField] private Collider2D interactionZone;
	[SerializeField] private Transform shield;

	#endregion // Editor Fields

	#region Non-Editor Fields

	protected Rigidbody2D rigidBody;
	new protected Collider2D collider;
	protected EntityAnimations animator;
	protected Controller m_current;
	//private GameObject currentInteractionHit;

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

	private Facing m_Facing;
	public Facing Facing
	{
		get
		{
			return m_Facing;
		}
		protected set
		{
			m_Facing = value;
			if (shield != null)
			{
				float angle = 0f;
				switch (m_Facing)
				{
					case Facing.right: angle = 0f; break;
					case Facing.up: angle = 90f; break;
					case Facing.left: angle = 180f; break;
					case Facing.down: angle = 270f; break;
				}
				shield.localRotation = Quaternion.Euler(0, 0, angle);
			}
		}
	}

	public bool canAttack
	{
		get
		{
			return true;
		}
	}

	#endregion // Properties

	#region Events

	public event Action<Interactable> OnNewInteractable;

	#endregion // Events

	#region Unity Functions

	protected void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<EntityAnimations>();
		collider = GetComponent<Collider2D>();
	}

	protected void Update()
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

		Vector2 position = transform.position;
		Collider2D[] c = Physics2D.OverlapCircleAll(position + direction * interactionDistance / 2, interactionDistance / 2, getInteractionLayermask());
		Debug.DrawLine(position, position + direction * interactionDistance);
		Interactable interactable = null;
		for (int i = 0; i < c.Length; ++i)
		{
			interactable = c[i].GetComponent<Interactable>();
			if (interactable != null)
			{
				//if (Physics2D.Raycast(transform.position, direction, interactionDistance, getInteractionLayermask()).collider != c[i])
				//{
				//	interactable = null;
				//}
			}
			if (interactable != null)
			{
				break;
			}
		}
		//RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, interactionDistance, getInteractionLayermask());
		//Interactable interactable = hit.collider != null ?
		//	hit.collider.GetComponent<Interactable>() : null;
		NewInteractable(interactable);
	}

	#endregion // Unity Functions

	#region Public Functions

	public virtual void Walk(Vector2 input)
	{
		if (!enabled)
		{
			rigidBody.velocity = Vector2.zero;
			return;
		}

		//RaycastHit2D hit = Physics2D.Raycast(transform.position, input,
		//	collider.bounds.extents.x + 0.05f,
		//	Physics2D.GetLayerCollisionMask(gameObject.layer));
		//if (hit.collider != null && !hit.collider.isTrigger)
		//{
		//	input = Vector3.ProjectOnPlane(input, hit.normal);
		//}
		
		Vector2 velocity = getUpdatedVelocity(rigidBody.velocity, input);
		rigidBody.velocity = velocity;
		setFacing(input);
		animator.UpdateMovementAnim(Facing, velocity.magnitude);
	}

	public void Attack()
	{
		if (!enabled)
		{
			return;
		}

		animator.Attack(Facing);
	}

	public List<InventoryItem> UpdateInventoryWeapons(InventoryManager playerInventory, InventoryManager bodyInventory)
	{
		return animator.UpdateInventoryWeapons(playerInventory, bodyInventory);
	}

	public void CleanUpInventoryEvents(InventoryManager playerInventory, InventoryManager bodyInventory)
	{
		animator.CleanUpInventoryEvents(playerInventory, bodyInventory);
	}

	//public void NewInteractables(List<Interactable> interactables)
	//{
	//	if (OnNewInteractables != null)
	//	{
	//		OnNewInteractables(interactables);
	//	}
	//}

	#endregion // Public Functions

	#region Private Functions

	protected void NewInteractable(Interactable interactable)
	{
		if (OnNewInteractable != null)
		{
			OnNewInteractable(interactable);
		}
	}

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
		if (Mathf.Approximately(input.x, 0f) && velocity.x * rigidBody.velocity.x < 0f)
		{
			velocity.x = 0f;
		}
		if (Mathf.Approximately(input.y, 0f) && velocity.y * rigidBody.velocity.y < 0f)
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
