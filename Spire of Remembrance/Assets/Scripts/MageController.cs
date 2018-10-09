using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : Controller {

	private void Update()
	{
		controlledMovement.Walk(Vector2.zero);
	}
}
