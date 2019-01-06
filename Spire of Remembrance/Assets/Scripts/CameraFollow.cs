using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private float maxDistance;

	private void Update()
	{
		float distance = Vector2.Distance(target.position, transform.position);
		if (distance > maxDistance)
		{
			transform.position += (target.position - transform.position).normalized * (distance - maxDistance);
		}
	}
}
