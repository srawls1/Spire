using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkedItemAction : ItemAction
{
	private ItemAction innerAction;
	private float markRadius;
	private float markDuration;

	public MarkedItemAction(ItemAction action, float radius, float duration,
		InventoryItem item, InventoryManager manager)
		: base(item, manager)
	{
		innerAction = action;
		actionString = innerAction.actionString;
		markRadius = radius;
		markDuration = duration;
	}

	public override void Perform()
	{
		manager.Remove(item);
		manager.Add(innerAction.item);
		innerAction.Perform();
		GameObject target = CharacterController.instance.controlledMovement.gameObject;
		MarkedStatus.InflictMarkedStatus(target, markRadius, markDuration);
	}
}

public class MarkedInventoryItem : InventoryItem
{
	public InventoryItem innerItem { get; private set; }
	private float markRadius;
	private float markDuration;

	public MarkedInventoryItem(InventoryItem item, float radius, float duration)
		: base(item.sprite)
	{
		innerItem = item;
		markRadius = radius;
		markDuration = duration;
	}

	public override List<ItemAction> actions
	{
		get
		{
			innerItem.manager = this.manager;
			List<ItemAction> innerActions = innerItem.actions;
			List<ItemAction> ret = new List<ItemAction>(innerActions.Count);
			for (int i = 0; i < innerActions.Count; ++i)
			{
				ret.Add(new MarkedItemAction(innerActions[i], markRadius,
					markDuration, this, manager));
			}
			return ret;
		}
	}

	public override string description
	{
		get
		{
			return innerItem.description;
		}
	}

	public override string name
	{
		get
		{
			return innerItem.name;
		}
	}
}
