using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeStatus : MonoBehaviour
{
	public float duration;

	void Start()
	{
		StartCoroutine(FreezeRoutine());
	}

	private IEnumerator FreezeRoutine()
	{
		yield return null;

		Movement movement = GetComponent<Movement>();
		Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
		if (movement == null)
		{
			Destroy(this);
			yield break;
		}

		// TODO - freeze shader effects?

		movement.enabled = false;

		while (duration > 0f)
		{
			duration -= Time.deltaTime;
			rigidBody.velocity = Vector2.zero;

			yield return null;
		}

		movement.enabled = true;

		// TODO - undo freeze shader effects

		Destroy(this);
	}

	public static void InflictFreezeStatus(GameObject obj, float freezeDuration)
	{
		BurnStatus burn = obj.GetComponent<BurnStatus>();
		if (burn != null)
		{
			burn.duration = 0f;
			return;
		}

		FreezeStatus freeze = obj.GetComponent<FreezeStatus>();
		if (freeze != null)
		{
			freeze.duration = Mathf.Max(freeze.duration, freezeDuration);
			return;
		}
		else
		{
			freeze = obj.AddComponent<FreezeStatus>();
			freeze.duration = freezeDuration;
		}
	}
}
