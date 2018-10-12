using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : Controller
{
	
	void Update()
	{
		controlledMovement.Walk(Vector2.zero);
	}
}
