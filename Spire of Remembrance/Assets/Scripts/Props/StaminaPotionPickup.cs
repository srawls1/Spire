using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPotionPickup : GradualFillPotionPickup
{
	[SerializeField] private float restoreScale;

	protected override Type potionType
	{
		get
		{
			return typeof(StaminaPotion);
		}
	}

	protected override void PerformPickupAction()
	{
		fillAmount = StaminaPotion.RestoreCharacterStamina(fillAmount, restoreScale);
		base.PerformPickupAction();
	}
}
