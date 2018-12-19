using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatTunnelNode : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private float distancePermission;
	[SerializeField] private RatTunnelNode[] connectingNodes;

	#endregion // Editor Fields

	#region Public Functions

	public bool IsInNode(Vector2 position)
	{
		return GetDistanceFrom(position) < distancePermission;
	}

	public bool IsInConnectedNode(Vector2 position, int connectionIndex)
	{
		if (connectionIndex < 0 || connectionIndex > connectingNodes.Length)
		{
			return false;
		}

		return connectingNodes[connectionIndex].IsInNode(position);
	}

	public RatTunnelNode GetConnectedNode(int connectionIndex)
	{
		if (connectionIndex < 0 || connectionIndex > connectingNodes.Length)
		{
			return null;
		}

		return connectingNodes[connectionIndex];
	}

	public float GetDistanceFrom(Vector2 position)
	{
		return Vector2.Distance(transform.position, position);
	}

	// Returns the index of the connecting node whose direction from
	// this node is closest to the direction of the given vector
	public int ClosestDirectionIndex(Vector2 direction)
	{
		int closestIndex = -1;
		float maxDotProduct = 0; // This corresponds to a maximum angle of 90 deg

		for (int i = 0; i < connectingNodes.Length; ++i)
		{
			Vector2 dir = GetConnectingDirection(i);
			float dotProduct = Vector2.Dot(direction, dir);
			if (dotProduct > maxDotProduct)
			{
				maxDotProduct = dotProduct;
				closestIndex = i;
			}
		}

		return closestIndex;
	}

	public Vector2 GetConnectingDirection(int connectionIndex)
	{
		if (connectionIndex < 0 || connectionIndex > connectingNodes.Length)
		{
			throw new ArgumentException("Invalid connection index: " + connectionIndex);
		}

		Vector2 diff = connectingNodes[connectionIndex].transform.position -
			transform.position;
		return diff.normalized;
	}

	public Vector2 GetTowardConnectionPosition(int connectionIndex, float displacement)
	{
		Vector2 direction = GetConnectingDirection(connectionIndex);
		float maxDistance = Vector2.Distance(transform.position,
			connectingNodes[connectionIndex].transform.position);
		Vector2 startingPosition = transform.position;
		return startingPosition + direction * Mathf.Clamp(displacement, 0f, maxDistance);
	}

	#endregion // Public Functions
}
