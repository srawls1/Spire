using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumController : AIController
{
	private MediumMovement movement
	{
		get { return controlledMovement as MediumMovement; }
	}

	public override void SetTarget(AITarget targ)
	{
		Debug.Log("Setting target");
		bool targetChange = target != targ;

		base.SetTarget(targ);
		Vector2 dest = GetDestination();

		if (movement.CurrentController == this)
		{
			if (targetChange)
			{
				movement.TeleportTo(dest);
			}
		}
		else
		{
			Debug.Log("Allowing teleport interaction");
			movement.MakeTeleportInteractable(targ);
		}
	}
}
