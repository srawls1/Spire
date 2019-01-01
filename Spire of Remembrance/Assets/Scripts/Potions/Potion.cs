using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion
{
	public virtual IEnumerator GetTarget()
	{
		yield break;
	}

	public abstract void Use(Controller controller, Bottle container);

	public virtual bool canPerform
	{
		get
		{
			return true;
		}
	}

	public abstract Sprite sprite
	{
		get;
	}

	public abstract string name
	{
		get;
	}

	public abstract string description
	{
		get;
	}
}
