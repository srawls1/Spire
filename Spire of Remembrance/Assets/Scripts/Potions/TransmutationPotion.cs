using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmutationPotion : Potion
{
	private Sprite m_sprite;
	private InventoryMenu m_menu;
	private InventoryItem targetItem;

	public TransmutationPotion(Sprite spr, InventoryMenu menu)
	{
		m_sprite = spr;
		m_menu = menu;
	}

	public override bool canPerform
	{
		get
		{
			return InventoryManager.bodyInventory != null;
		}
	}

	public override string description
	{
		get
		{
			return "Converts any one item from the possessed enemy's inventory over to the spirit's permanent inventory";
		}
	}

	public override string name
	{
		get
		{
			return "Transmutation Potion";
		}
	}

	public override Sprite sprite
	{
		get
		{
			return m_sprite;
		}
	}

	public override IEnumerator GetTarget()
	{
		if (m_menu == null)
		{
			m_menu = MonoBehaviour.FindObjectOfType<InventoryMenu>();
		}
		if (m_menu == null)
		{
			yield break;
		}
		m_menu.SetTransmutationMode((item) =>
		{
			targetItem = item;
			m_menu.SetBaseMode();
		});
		while (m_menu.enabled && m_menu.transmutationMode)
		{
			yield return null;
		}
	}

	public override void Use(Controller controller, Bottle container)
	{
		if (targetItem == null)
		{
			return;
		}

		InventoryManager.bodyInventory.Remove(targetItem);
		InventoryManager.playerInventory.Add(targetItem);
		container.containedPotion = null;
	}
}
