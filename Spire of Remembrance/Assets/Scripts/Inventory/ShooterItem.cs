using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipShooterAction : ItemAction
{
	private ShooterItem shooter
	{
		get
		{
			return item as ShooterItem;
		}
	}

	public EquipShooterAction(InventoryItem item, InventoryManager manager)
		: base(item, manager)
	{
		actionString = "Equip";
	}

	public override void Perform()
	{
		if (shooter.isStaff)
		{
			manager.equippedStaff = shooter;
		}
		else
		{
			manager.equippedBow = shooter;
		}
	}
}

public class UnequipShooterAction : ItemAction
{
	private ShooterItem shooter
	{
		get
		{
			return item as ShooterItem;
		}
	}

	public UnequipShooterAction(InventoryItem item, InventoryManager manager)
		: base(item, manager)
	{
		actionString = "Unequip";
	}

	public override void Perform()
	{
		if (shooter.isStaff)
		{
			manager.equippedStaff = manager.defaultStaff;
		}
		else
		{
			manager.equippedBow = manager.defaultBow;
		}
	}
}

public class ShooterItem : InventoryItem
{
	public ProjectileShooterData weaponData
	{
		get; private set;
	}
	public bool isStaff
	{
		get; private set;
	}

	public ShooterItem(Sprite spr, ProjectileShooterData data, bool staff)
		: base(spr)
	{
		this.weaponData = data;
		isStaff = staff;
	}

	public override List<ItemAction> actions
	{
		get
		{
			List<ItemAction> ret = new List<ItemAction>();

			if ((isStaff && manager.equippedStaff == this) ||
				(!isStaff && manager.equippedBow == this))
			{
				ret.Add(new UnequipShooterAction(this, manager));
			}
			else
			{
				ret.Add(new EquipShooterAction(this, manager));
				ret.Add(new DropItemAction(this, manager));
			}

			return ret;
		}
	}
}
