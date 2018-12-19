using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritPotion : HealthPotion
{
	public SpiritPotion(Sprite partial, Sprite full)
		: base(partial, full)
	{ }

	protected override EnemyHealth GetHealthTarget(Controller controller)
	{
		return controller.GetComponent<SpiritHealth>();
	}
}
