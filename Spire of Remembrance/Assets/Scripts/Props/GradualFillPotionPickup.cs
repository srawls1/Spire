using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GradualFillPotionPickup : Pickup
{
	[SerializeField] protected float fillAmount;

	protected abstract Type potionType
	{
		get;
	}

	protected abstract GradualFillPotion GenerateNew();

	protected override void PerformPickupAction()
	{
		while (fillAmount > 0)
		{
			Bottle container = InventoryManager.playerInventory.GetAvailableBottle(potionType);
			if (container == null)
			{
				break;
			}
			if (container.containedPotion == null)
			{
				container.containedPotion = GenerateNew();
			}

			GradualFillPotion potion = container.containedPotion as GradualFillPotion;
			fillAmount = potion.Fill(fillAmount);
		}

		if (fillAmount < 0.01f)
		{
			base.PerformPickupAction();
		}
	}
}
