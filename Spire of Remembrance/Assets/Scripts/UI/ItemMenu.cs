using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemMenu : MonoBehaviour
{
	[SerializeField] private ItemButton itemButtonPrefab;

	public abstract void OnItemHover(ItemButton button);
	public abstract void OnItemClick(ItemButton button);

	protected void PopulateInventoryPane(RectTransform pane, List<InventoryItem> items)
	{
		int i = 0;
		for (; i < items.Count && i < pane.childCount; ++i)
		{
			ItemButton button = pane.GetChild(i).GetComponent<ItemButton>();
			button.item = items[i];
		}
		for (; i < items.Count; ++i)
		{
			ItemButton button = Instantiate(itemButtonPrefab);
			button.transform.SetParent(pane);
			button.item = items[i];
		}
		for (; i < pane.childCount; ++i)
		{
			pane.GetChild(i).gameObject.SetActive(false);
		}
	}
}
