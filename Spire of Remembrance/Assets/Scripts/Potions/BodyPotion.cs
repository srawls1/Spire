using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPotion : HealthPotion
{
	public BodyPotion(Sprite partial, Sprite full)
		: base(partial, full)
	{ }

	protected override EnemyHealth GetHealthTarget(Controller controller)
	{
		if (controller.controlledMovement == null ||
			controller.controlledMovement == controller.GetComponent<Movement>())
		{
			return null;
		}
		EnemyHealth health = controller.controlledMovement.GetComponent<EnemyHealth>();
		if (health is SpiritHealth)
		{
			return null;
		}

		return health;
	}
}
