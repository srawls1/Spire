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
}
