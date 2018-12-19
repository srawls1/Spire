using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkedStatus : MonoBehaviour
{
	private float radius;
	private float duration;

	void Start()
	{
		StartCoroutine(MarkRoutine());
	}
	
	private IEnumerator MarkRoutine()
	{
		Damageable damageable = GetComponent<Damageable>();
		if (damageable == null)
		{
			yield break;
		}

		// TODO - Flashing particle effect

		while (duration > 0)
		{
			duration -= Time.deltaTime;

			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
			for (int i = 0; i < colliders.Length; ++i)
			{
				AIController controller = colliders[i].GetComponent<AIController>();
				if (controller != null && controller.gameObject != gameObject)
				{
					controller.target = damageable;
				}
			}

			yield return null;
		}

		// TODO - Destroy flashing effect
		Destroy(this);
	}

	public static void InflictMarkedStatus(GameObject obj, float effectRadius, float effectDuration)
	{
		MarkedStatus status = obj.GetComponent<MarkedStatus>();
		if (status == null)
		{
			status = obj.AddComponent<MarkedStatus>();
		}

		status.radius = Mathf.Max(status.radius, effectRadius);
		status.duration = Mathf.Max(status.duration, effectDuration);
	}
}
