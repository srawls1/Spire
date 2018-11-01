using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatTunnel : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private RatTunnelNode[] endPoints;

	#endregion // Editor Fields

	#region Public Functions

	public bool IsAtEntry(Vector2 position)
	{
		return getEntryIndex(position) >= 0;
	}

	public RatTunnelNode GetEntryNode(Vector2 position, Vector2 direction)
	{
		int entryIndex = getEntryIndex(position);
		if (entryIndex < 0)
		{
			return null;
		}

		if (endPoints[entryIndex].ClosestDirectionIndex(direction) >= 0)
		{
			return endPoints[entryIndex];
		}
		else
		{
			return null;
		}
	}

	public bool IsExiting(Vector2 position, Vector2 direction)
	{
		int entryIndex = getEntryIndex(position);
		if (entryIndex < 0)
		{
			return false;
		}

		return endPoints[entryIndex].ClosestDirectionIndex(direction) < -0.1f;
	}

	public Vector2 MoveInDirection(ref RatTunnelNode currentNode, Vector2 position, Vector2 movement)
	{
		// Handle entry
		if (currentNode == null)
		{
			currentNode = GetEntryNode(position, movement);
			if (currentNode == null)
			{
				return position;
			}
		}

		// Handle exit
		if (IsExiting(position, movement))
		{
			currentNode = null;
			return position;
		}

		Vector2 nodePosition = currentNode.transform.position;

		// Handle being at a node
		if (currentNode.IsInNode(position))
		{
			int connectionIndex = currentNode.ClosestDirectionIndex(movement);
			if (connectionIndex < 0)
			{
				return position;
			}

			position += movement;
			float disp = Vector2.Dot(position - nodePosition, currentNode.GetConnectingDirection(connectionIndex));
			return currentNode.GetTowardConnectionPosition(connectionIndex, disp);
		}
		// Handle being between nodes
		else
		{
			Vector2 disp = position - nodePosition;
			float dist = disp.magnitude;
			int connectionIndex = currentNode.ClosestDirectionIndex(disp);
			Vector2 dir = currentNode.GetConnectingDirection(connectionIndex);

			dist += Vector2.Dot(movement, dir);
			Vector2 newPosition = currentNode.GetTowardConnectionPosition(connectionIndex, dist);

			if (currentNode.IsInConnectedNode(newPosition, connectionIndex))
			{
				currentNode = currentNode.GetConnectedNode(connectionIndex);
			}

			return newPosition;
		}
	}

	#endregion // Public Functions

	#region Private Functions

	private int getEntryIndex(Vector2 position)
	{
		for (int i = 0; i < endPoints.Length; ++i)
		{
			if (endPoints[i].IsInNode(position))
			{
				return i;
			}
		}

		return -1;
	}

	#endregion // Private Functions
}
