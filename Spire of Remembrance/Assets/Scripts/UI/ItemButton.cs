using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour
{
	[SerializeField] private Image itemImage;

	private ItemMenu menu;

	private InventoryItem m_item;
	public InventoryItem item
	{
		get
		{
			return m_item;
		}
		set
		{
			m_item = value;
			if (m_item == null)
			{
				gameObject.SetActive(false);
			}
			else
			{
				gameObject.SetActive(true);
				itemImage.sprite = m_item.sprite;
			}
		}
	}

	private void Awake()
	{
		menu = GetComponentInParent<ItemMenu>();
		EventTrigger trigger = GetComponent<EventTrigger>();
	}

	public void ShowSelected()
	{
		menu.OnItemHover(this);
	}

	public void Select()
	{
		menu.OnItemClick(this);
	}
}
