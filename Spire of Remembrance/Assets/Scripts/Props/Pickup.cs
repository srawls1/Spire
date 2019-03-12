using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	[SerializeField] private GameObject pickupRoot;
	[SerializeField] private string message;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject == CharacterController.instance.gameObject ||
			collider.gameObject == CharacterController.instance.controlledMovement.gameObject)
		{
			PerformPickupAction();
		}
	}

	protected virtual void PerformPickupAction()
	{
		InGameUIManager.instance.ShowPickup(message);
		Destroy(pickupRoot);
	}
}
