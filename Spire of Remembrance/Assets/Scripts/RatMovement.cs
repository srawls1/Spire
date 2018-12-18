using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatMovement : Movement
{
	#region Non-Editor Fields

	private RatTunnel currentTunnel;
	private RatTunnelNode currentNode;
	private Vector2 currentVelocity;
	private Vector2 lastInput;

	#endregion // Non-Editor Fields

	#region Unity Functions

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (currentNode != null)
		{
			return;
		}

		RatTunnel tunnel = collision.GetComponent<RatTunnel>();
		if (tunnel == null)
		{
			return;
		}

		RatTunnelNode entry = tunnel.GetEntryNode(transform.position, lastInput);
		if (entry != null)
		{
			currentNode = entry;
			currentTunnel = tunnel;
			rigidBody.bodyType = RigidbodyType2D.Static;
		}
	}

	#endregion // Unity Functions

	#region Public Functions

	public override void Walk(Vector2 input)
	{
		lastInput = input;

		if (currentNode == null)
		{
			base.Walk(input);
			currentVelocity = rigidBody.velocity;
		}
		else
		{
			currentVelocity = maxWalkSpeed * input;
			setFacing(input);
			animator.UpdateMovementAnim(Facing, currentVelocity.magnitude);
			transform.position = currentTunnel.MoveInDirection(ref currentNode,
				transform.position, currentVelocity * Time.deltaTime);
			if (currentNode == null)
			{
				currentTunnel = null;
				rigidBody.bodyType = RigidbodyType2D.Dynamic;
			}
		}
	}

	#endregion // Public Functions
}
