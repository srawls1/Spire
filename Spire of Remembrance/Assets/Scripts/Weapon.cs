using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public int damage { get; set; }
	public float attackForce { get; set; }

	public void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision.gameObject);

		if (collision.transform == transform.parent)
		{
			return;
		}

		Damageable damageable = collision.GetComponent<Damageable>();
		if (damageable == null)
		{
			return;
		}

		Debug.Log(string.Format("Dealing damage {0} and knocking back {1}", damage, attackForce));
		damageable.TakeDamage(damage, transform.position, attackForce);
	}
}
