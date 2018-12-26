using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritPotionPickup : GradualFillPotionPickup
{
	[SerializeField] private float healScale;
	[SerializeField] private Sprite partial;
	[SerializeField] private Sprite full;

	protected override Type potionType
	{
		get
		{
			return typeof(SpiritPotion);
		}
	}

	protected override GradualFillPotion GenerateNew()
	{
		return new SpiritPotion(partial, full, healScale);
	}

	protected override void PerformPickupAction()
	{
		EnemyHealth health = SpiritPotion.GetSpiritHealth(CharacterController.instance);
		fillAmount = HealthPotion.HealTarget(fillAmount, healScale, health);
		base.PerformPickupAction();
	}
}
