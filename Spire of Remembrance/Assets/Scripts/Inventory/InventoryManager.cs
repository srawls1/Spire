using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
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

	private List<InventoryItem> m_items;
	public List<InventoryItem> items
	{
		get
		{
			return m_items;
		}
	}

	private void Awake()
	{
		m_items = new List<InventoryItem>();
	}

	public void Add(InventoryItem item)
	{
		item.manager = this;
		m_items.Add(item);
	}

	public void Remove(InventoryItem item)
	{
		item.manager = null;
		m_items.Remove(item);
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
}
