using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPotionPickup : GradualFillPotionPickup
{
	[SerializeField] private float healScale;

	protected override Type potionType
	{
		get
		{
			return typeof(BodyPotion);
		}
	}

	protected override void PerformPickupAction()
	{
		EnemyHealth health = BodyPotion.GetBodyHealth(CharacterController.instance);
		fillAmount = HealthPotion.HealTarget(fillAmount, healScale, health);
		base.PerformPickupAction();
	}
}
