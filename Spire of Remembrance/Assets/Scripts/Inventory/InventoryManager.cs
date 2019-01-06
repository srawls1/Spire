using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	#region Significant Static Instances

	private static InventoryManager m_playerInventory;
	public static InventoryManager playerInventory
	{
		get
		{
			if (m_playerInventory == null)
			{
				m_playerInventory = CharacterController.instance.GetComponent<InventoryManager>();
			}
			return m_playerInventory;
		}
	}

	public static InventoryManager bodyInventory
	{
		get
		{
			Movement m = CharacterController.instance.controlledMovement;
			InventoryManager inventory = m.GetComponent<InventoryManager>();
			if (inventory != playerInventory)
			{
				return inventory;
			}
			return null;
		}
	}

	#endregion // Significant Static Instances

	#region Editor Fields

	[SerializeField] private WeaponData defaultSwordData;
	[SerializeField] private ProjectileShooterData defaultStaffData;
	[SerializeField] private ProjectileShooterData defaultBowData;

	#endregion // Editor Fields

	#region Properties

	private List<InventoryItem> m_items;
	public List<InventoryItem> items
	{
		get
		{
			return m_items;
		}
	}

	public SwordItem defaultSword
	{
		get; private set;
	}

	public event Action<SwordItem> OnNewSwordEquipped;
	private SwordItem m_equippedSword;
	public SwordItem equippedSword
	{
		get
		{
			return m_equippedSword;
		}
		set
		{
			m_equippedSword = value;
			if (OnNewSwordEquipped != null)
			{
				OnNewSwordEquipped(value);
			}
		}
	}

	public ShooterItem defaultStaff
	{
		get; private set;
	}

	public event Action<ShooterItem> OnNewStaffEquipped;
	private ShooterItem m_equippedStaff;
	public ShooterItem equippedStaff
	{
		get
		{
			return m_equippedStaff;
		}
		set
		{
			m_equippedStaff = value;
			if (OnNewStaffEquipped != null)
			{
				OnNewStaffEquipped(value);
			}
		}
	}

	public ShooterItem defaultBow
	{
		get; private set;
	}

	public event Action<ShooterItem> OnNewBowEquipped;
	private ShooterItem m_equippedBow;
	public ShooterItem equippedBow
	{
		get
		{
			return m_equippedBow;
		}
		set
		{
			m_equippedBow = value;
			if (OnNewBowEquipped != null)
			{
				OnNewBowEquipped(value);
			}
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Awake()
	{
		m_items = new List<InventoryItem>();

		if (defaultSwordData != null)
		{
			defaultSword = new SwordItem(defaultSwordData.weaponSprite, defaultSwordData);
			defaultSword.manager = this;
			equippedSword = defaultSword;
		}
		if (defaultStaffData != null)
		{
			defaultStaff = new ShooterItem(defaultStaffData.shooterSprite, defaultStaffData, true);
			defaultStaff.manager = this;
			equippedStaff = defaultStaff;
		}
		if (defaultBowData != null)
		{
			defaultBow = new ShooterItem(defaultBowData.shooterSprite, defaultBowData, false);
			defaultBow.manager = this;
			equippedBow = defaultBow;
		}
	}

	#endregion // Unity Functions

	#region Public Functions

	public void Add(InventoryItem item)
	{
		item.manager = this;
		m_items.Add(item);
		//if (OnInventoryChanged != null)
		//{
		//	OnInventoryChanged();
		//}
	}

	public void Remove(InventoryItem item)
	{
		item.manager = null;
		m_items.Remove(item);
		//if (OnInventoryChanged != null)
		//{
		//	OnInventoryChanged();
		//}
	}

	public Bottle GetAvailableBottle(Type potionType)
	{
		if (potionType.IsSubclassOf(typeof(GradualFillPotion)))
		{
			for (int i = 0; i < m_items.Count; ++i)
			{
				Bottle bottle = m_items[i] as Bottle;
				if (bottle != null)
				{
					if (bottle.containedPotion != null &&
						bottle.containedPotion.GetType() == potionType)
					{
						return bottle;
					}
				}
			}
		}

		for (int i = 0; i < m_items.Count; ++i)
		{
			Bottle bottle = m_items[i] as Bottle;
			if (bottle != null)
			{
				if (bottle.containedPotion == null)
				{
					return bottle;
				}
			}
		}

		return null;
	}

	public List<SwordItem> GetAllSwords()
	{
		List<SwordItem> swords = new List<SwordItem>();

		for (int i = 0; i < m_items.Count; ++i)
		{
			SwordItem sword = m_items[i] as SwordItem;
			if (sword != null)
			{
				swords.Add(sword);
			}
		}

		return swords;
	}

	public List<ShooterItem> GetAllStaves()
	{
		List<ShooterItem> staves = new List<ShooterItem>();

		for (int i = 0; i < m_items.Count; ++i)
		{
			ShooterItem staff = m_items[i] as ShooterItem;
			if (staff != null && staff.isStaff)
			{
				staves.Add(staff);
			}
		}

		return staves;
	}

	public List<ShooterItem> GetAllBows()
	{
		List<ShooterItem> bows = new List<ShooterItem>();

		for (int i = 0; i < m_items.Count; ++i)
		{
			ShooterItem bow = m_items[i] as ShooterItem;
			if (bow != null && !bow.isStaff)
			{
				bows.Add(bow);
			}
		}

		return bows;
	}

	// TODO - GetAllTalismans

	#endregion // Public Functions
}
