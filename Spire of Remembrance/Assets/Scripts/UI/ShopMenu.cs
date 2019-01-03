using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : ItemMenu
{
	#region Editor Fields

	[SerializeField] private RectTransform selectionCursor;
	[SerializeField] private RectTransform itemsContentPane;
	[SerializeField] private PricedItemDescriptionField description;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private MerchantInteractable merchant;
	private Dictionary<InventoryItem, int> priceByItem;

	#endregion // Non-Editor Fields

	#region Public Functions

	public void SetMerchantInfo(MerchantInteractable merch, List<InventoryItem> items, List<int> prices)
	{
		merchant = merch;
		priceByItem = new Dictionary<InventoryItem, int>();

		for (int i = 0; i < items.Count; ++i)
		{
			priceByItem.Add(items[i], prices[i]);
		}
		PopulateInventoryPane(itemsContentPane, items);
	}

	public override void OnItemClick(ItemButton button)
	{
		merchant.BuyIfPossible(button.item);
	}

	public override void OnItemHover(ItemButton button)
	{
		description.item = button.item;
		description.price = priceByItem[button.item];
		selectionCursor.gameObject.SetActive(true);
		selectionCursor.SetParent(button.transform);
		selectionCursor.anchorMin = Vector2.zero;
		selectionCursor.anchorMax = new Vector2(1f, 1f);
		selectionCursor.sizeDelta = Vector2.zero;
		selectionCursor.anchoredPosition = Vector2.zero;
	}

	#endregion // Public Functions
}
