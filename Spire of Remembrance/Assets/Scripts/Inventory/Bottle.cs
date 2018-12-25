using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBottleAction : ItemAction
{
	private Bottle bottle
	{
		get
		{
			return item as Bottle;
		}
	}

	public EmptyBottleAction(InventoryItem item, InventoryManager manager)
		: base(item, manager)
	{
		actionString = "Empty";
	}

	public override void Perform()
	{
		bottle.containedPotion = null;
	}
}

public class UsePotionAction : ItemAction
{
	private Bottle bottle
	{
		get
		{
			return item as Bottle;
		}
	}

	public UsePotionAction(InventoryItem item, InventoryManager manager)
		: base(item, manager)
	{
		actionString = "Use";
	}

	public override void Perform()
	{
		bottle.containedPotion.Use(manager.GetComponent<Controller>(), bottle);
	}
}

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

	public override List<ItemAction> actions
	{
		get
		{
			List<ItemAction> acts = new List<ItemAction>();
			if (containedPotion != null)
			{
				acts.Add(new UsePotionAction(this, manager));
				acts.Add(new EmptyBottleAction(this, manager));
			}
			else
			{
				acts.Add(new DropItemAction(this, manager));
			}

			return acts;
		}
	}
}
