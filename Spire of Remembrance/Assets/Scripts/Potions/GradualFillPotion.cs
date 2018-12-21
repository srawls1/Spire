using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GradualFillPotion : Potion
{
	private Sprite partiallyFullSprite;
	private Sprite fullSprite;

	public override Sprite sprite
	{
		get
		{
			if (portionFull == 1f)
			{
				return fullSprite;
			}
			else
			{
				return partiallyFullSprite;
			}
		}
	}

	public float portionFull
	{
		get; protected set;
	}

	public GradualFillPotion(Sprite partial, Sprite full)
	{
		partiallyFullSprite = partial;
		fullSprite = full;
	}

	// Returns the overfill amount
	public float Fill(float portion)
	{
		portionFull += portion;
		if (portionFull > 1f)
		{
			float ret = portionFull - 1f;
			portionFull = 1f;
			return ret;
		}

		return 0f;
	}

	protected void ClearIfEmpty(Bottle container)
	{
		if (portionFull < .01f)
		{
			container.containedPotion = null;
		}
	}
}
