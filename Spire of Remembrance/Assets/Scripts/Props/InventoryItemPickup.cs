using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemPickup : Pickup
{
	[SerializeField] private SerializedInventoryItem serialized;

	public InventoryItem item;

	private void Awake()
	{
		InventoryItem temp = serialized.ToItem();
		if (temp != null)
		{
			item = temp;
		}
	}

	protected override void PerformPickupAction()
	{
		InventoryManager.playerInventory.Add(item);
		base.PerformPickupAction();
	}
}
