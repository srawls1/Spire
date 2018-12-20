using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject == CharacterController.instance.gameObject ||
			collision.gameObject == CharacterController.instance.controlledMovement.gameObject)
		{
			PerformPickupAction();
		}
	}

	protected virtual void PerformPickupAction()
	{
		Destroy(gameObject);
	}
}
