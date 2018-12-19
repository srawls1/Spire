using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion
{
	public abstract void Use(Controller controller, Bottle container);

	public abstract Sprite sprite
	{
		get;
	}
}
