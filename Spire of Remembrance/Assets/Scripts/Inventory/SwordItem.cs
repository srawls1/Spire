using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSwordAction : ItemAction
{
	private SwordItem sword
	{
		get
		{
			return item as SwordItem;
		}
	}

	public EquipSwordAction(InventoryItem item, InventoryManager manager)
		: base(item, manager)
	{
		actionString = "Equip";
	}

	public override void Perform()
	{
		manager.equippedSword = sword;
	}
}

public class UnequipSwordAction : ItemAction
{
	private SwordItem sword
	{
		get
		{
			return item as SwordItem;
		}
	}

	public UnequipSwordAction(InventoryItem item, InventoryManager manager)
		: base(item, manager)
	{
		actionString = "Unequip";
	}

	public override void Perform()
	{
		manager.equippedSword = manager.defaultSword;
	}
}

public class SwordItem : InventoryItem
{
	public WeaponData data
	{
		get; private set;
	}

	public SwordItem(Sprite spr, WeaponData weaponData)
		: base(spr)
	{
		data = weaponData;
	}

	public override string name
	{
		get
		{
			return data.name;
		}
	}

	public override string description
	{
		get
		{
			return data.description;
		}
	}

	public override List<ItemAction> actions
	{
		get
		{
			List<ItemAction> ret = new List<ItemAction>();

			if (manager.equippedSword == this)
			{
				ret.Add(new UnequipSwordAction(this, manager));
			}
			else
			{
				ret.Add(new EquipSwordAction(this, manager));
				ret.Add(new DropItemAction(this, manager));
			}

			return ret;
		}
	}
}
