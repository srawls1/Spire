using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageMovement : Movement
{
	protected override int getInteractionLayermask()
	{
		return ~LayerMask.GetMask(new string[]
		{
			"Spirit"
		});
	}
}
