using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float destroyAfterSeconds;

	public GameObject instigator
	{
		get; set;
	}

	protected void Start()
	{
		Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
		if (rigidBody != null)
		{
			rigidBody.velocity = transform.right * speed;
		}
		else
		{
			StartCoroutine(ManualMovement());
		}

		if (destroyAfterSeconds > 0)
		{
			Destroy(gameObject, destroyAfterSeconds);
		}
	}

	private IEnumerator ManualMovement()
	{
		while (true)
		{
			transform.position += transform.right * speed * Time.deltaTime;
			yield return null;
		}
	}
}
