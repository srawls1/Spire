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

	protected override void PerformPickupAction()
	{
		while (fillAmount > 0)
		{
			Bottle container = InventoryManager.instance.GetAvailableBottle(potionType);
			if (container == null)
			{
				break;
			}
			BodyPotion bodyPotion = container.containedPotion as BodyPotion;
			fillAmount = bodyPotion.Fill(fillAmount);
		}

		if (fillAmount == 0)
		{
			base.PerformPickupAction();
		}
	}
}
