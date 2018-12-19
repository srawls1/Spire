using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : InventoryItem
{
	public Bottle(Sprite spr, Potion potion = null)
		: base(spr)
	{
		containedPotion = potion;
	}

	public override Sprite sprite
	{
		get
		{
			if (containedPotion != null)
			{
				return containedPotion.sprite;
			}
			return base.sprite;
		}
	}

	public Potion containedPotion
	{
		get; set;
	}
}
