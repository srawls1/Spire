using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemPickup : Pickup
{
	public InventoryItem item;

	protected override void PerformPickupAction()
	{
		InventoryManager.playerInventory.Add(item);
		base.PerformPickupAction();
	}
}
