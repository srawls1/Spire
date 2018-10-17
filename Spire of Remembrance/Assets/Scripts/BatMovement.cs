using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : Movement
{
	public override bool canPassPit
	{
		get
		{
			return true;
		}
	}
}
