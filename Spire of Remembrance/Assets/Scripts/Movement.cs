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

	[SerializeField]
	private float maxWalkSpeed;
	[SerializeField]
	private float walkAcceleration;
	[SerializeField]
	private float walkDecceleration;

	#endregion // Editor Fields

	#region Non-Editor Fields

	protected Rigidbody2D rigidBody;
	protected Animator animator;
	protected Controller m_current;
	private Facing m_facing;

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
		get
		{
			return m_facing;
		}
		protected set
		{
			m_facing = value;
			animator.SetInteger("Facing", (int)m_facing);
		}
	}

	#endregion // Properties

	#region Events

	public event Action<GameObject> OnEnteredInteractable;
	public event Action<GameObject> OnExitedInteractable;

	#endregion // Events

	#region Unity Functions

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		GameObject obj = canInteract(other.gameObject);
		if (obj != null)
		{
			if (OnEnteredInteractable != null)
			{
				OnEnteredInteractable(obj);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		GameObject obj = canInteract(other.gameObject);
		if (obj != null)
		{
			if (OnExitedInteractable != null)
			{
				OnExitedInteractable(obj);
			}
		}
	}

	#endregion // Unity Functions

	#region Public Functions

	public void Walk(Vector2 input)
	{
		if (input.magnitude > 1f)
		{
			input.Normalize();
		}
		Vector2 goalVelocity = input * maxWalkSpeed;
		float changeFactor = goalVelocity.magnitude < rigidBody.velocity.magnitude || Vector2.Dot(rigidBody.velocity, goalVelocity) < 0f ?
			walkDecceleration : walkAcceleration;

		rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, goalVelocity, Time.deltaTime * changeFactor);

		setFacing();
		animator.SetFloat("Velocity", rigidBody.velocity.magnitude);
	}

	public void Interact(GameObject interactable)
	{
		if (interactable != null)
		{
			StartCoroutine(interact(interactable));
		}
	}

	public abstract void Attack();

	#endregion // Public Functions

	#region Private Functions

	protected abstract GameObject canInteract(GameObject obj);

	protected abstract IEnumerator interact(GameObject obj);

	protected void setFacing()
	{
		Vector2 velocity = rigidBody.velocity;
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
