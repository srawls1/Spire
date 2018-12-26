using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPotionPickup : GradualFillPotionPickup
{
	[SerializeField] private float restoreScale;
	[SerializeField] private Sprite partial;
	[SerializeField] private Sprite full;

	protected override Type potionType
	{
		get
		{
			return typeof(StaminaPotion);
		}
	}

	protected override GradualFillPotion GenerateNew()
	{
		return new StaminaPotion(partial, full, restoreScale);
	}

	protected override void PerformPickupAction()
	{
		fillAmount = StaminaPotion.RestoreCharacterStamina(fillAmount, restoreScale);
		base.PerformPickupAction();
	}
}
