using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : Controller
{

	private void Update()
	{
		controlledMovement.Walk(Vector2.zero);
	}
}
