using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float destroyAfterSeconds;

	protected Alignment instigatorAlignment;
	private GameObject m_instigator;
	public GameObject instigator
	{
		get
		{
			return m_instigator;
		}
		set
		{
			m_instigator = value;
			AITarget ait = null;
			if (m_instigator != null)
			{
				ait = m_instigator.GetComponent<AITarget>();
			}
			if (ait != null)
			{
				instigatorAlignment = ait.alignment;
			}
			else
			{
				instigatorAlignment = Alignment.Wildcard;
			}
		}
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
