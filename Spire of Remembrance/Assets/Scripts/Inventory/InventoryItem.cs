using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
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
}
