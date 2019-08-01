using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToInteractable : Interactable
{
	public MediumMovement movement { get; set; }
	public AITarget target { get; set; }

	public override Interaction[] interactions
	{
		get
		{
			return new Interaction[]
			{
				new Interaction("Teleport", TeleportAction, true)
			};
		}
	}

	private void TeleportAction()
	{
		Vector2 disp = transform.position - target.transform.position;
		Vector2 dest = target.transform.position;
		movement.TeleportTo(dest + disp.normalized);
	}
}
