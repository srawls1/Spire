using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritPotion : HealthPotion
{
	public SpiritPotion(Sprite partial, Sprite full, float scale)
		: base(partial, full, scale)
	{ }

	public override string name
	{
		get
		{
			string fullness = Mathf.Approximately(portionFull, 1f) ? "Full" :
				string.Format("{0:0.2} Full", portionFull);
			return string.Format("Spirit Health Potion ({0})", fullness);
		}
	}

	public override string description
	{
		get
		{
			return "Restores health to the spirit";
		}
	}

	protected override EnemyHealth GetHealthTarget(Controller controller)
	{
		return GetSpiritHealth(controller);
	}

	public static EnemyHealth GetSpiritHealth(Controller controller)
	{
		return controller.GetComponent<SpiritHealth>();
	}
}
