using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPotion : HealthPotion
{
	public BodyPotion(Sprite partial, Sprite full, float scale)
		: base(partial, full, scale)
	{ }

	protected override EnemyHealth GetHealthTarget(Controller controller)
	{
		return GetBodyHealth(controller);
	}

	public override string name
	{
		get
		{
			string fullness = Mathf.Approximately(portionFull, 1f) ? "Full" :
				string.Format("{0:0.2} Full", portionFull);
			return string.Format("Body Health Potion ({0})", fullness);
		}
	}

	public override string description
	{
		get
		{
			return "Restores health to the body you are currently possessing";
		}
	}

	public static EnemyHealth GetBodyHealth(Controller controller)
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
