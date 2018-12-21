using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritPotion : HealthPotion
{
	public SpiritPotion(Sprite partial, Sprite full, float scale)
		: base(partial, full, scale)
	{ }

	protected override EnemyHealth GetHealthTarget(Controller controller)
	{
		return GetSpiritHealth(controller);
	}

	public static EnemyHealth GetSpiritHealth(Controller controller)
	{
		return controller.GetComponent<SpiritHealth>();
	}
}
