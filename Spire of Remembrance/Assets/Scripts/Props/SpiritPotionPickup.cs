using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritPotionPickup : GradualFillPotionPickup
{
	[SerializeField] private float healScale;

	protected override Type potionType
	{
		get
		{
			return typeof(SpiritPotion);
		}
	}

	protected override void PerformPickupAction()
	{
		EnemyHealth health = SpiritPotion.GetSpiritHealth(CharacterController.instance);
		fillAmount = HealthPotion.HealTarget(fillAmount, healScale, health);
		base.PerformPickupAction();
	}
}
