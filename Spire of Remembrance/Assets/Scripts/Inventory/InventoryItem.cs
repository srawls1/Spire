using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemAction
{
	protected InventoryManager manager;
	protected InventoryItem item;
	public string actionString { get; protected set; }

	public ItemAction(InventoryItem item, InventoryManager manager)
	{
		this.manager = manager;
		this.item = item;
	}

	public virtual bool canPerform
	{
		get
		{
			return true;
		}
	}

	public virtual IEnumerator GetTarget()
	{
		yield break;
	}

	public abstract void Perform();
}

public class DropItemAction : ItemAction
{
	private bool enabled;

	public DropItemAction(InventoryItem item, InventoryManager manager, bool enable = true)
		: base(item, manager)
	{
		enabled = enable;
		actionString = "Drop";
	}

	public override bool canPerform
	{
		get
		{
			return enabled;
		}
	}

	public override void Perform()
	{
		manager.Remove(item);
	}
}

public abstract class InventoryItem
{
	public InventoryManager manager;

	private Sprite m_sprite;

	public InventoryItem(Sprite spr)
	{
		m_sprite = spr;
	}

	public virtual Sprite sprite
	{
		get
		{
			return m_sprite;
		}
	}

	public abstract List<ItemAction> actions
	{
		get;
	}
}
