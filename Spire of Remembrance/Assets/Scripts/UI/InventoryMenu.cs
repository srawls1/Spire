using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
	[SerializeField] private RectTransform spiritInventoryContent;
	[SerializeField] private RectTransform bodyInventoryContent;
	[SerializeField] private RectTransform wholeScreenInputBlocker;
	[SerializeField] private RectTransform spiritInventoryInputBlocker;
	[SerializeField] private RectTransform selectionCursor;
	[SerializeField] private ItemDescriptionField descriptionField;
	[SerializeField] private RectTransform itemActionMenu;
	[SerializeField] private ItemButton itemButtonPrefab;
	[SerializeField] private ItemActionButton itemActionButtonPrefab;

	bool transmutationMode;

	public void ShowSelectedItem(ItemButton item)
	{
		descriptionField.item = item.item;
		selectionCursor.gameObject.SetActive(true);
		selectionCursor.SetParent(item.transform);
		selectionCursor.anchorMin = Vector2.zero;
		selectionCursor.anchorMax = new Vector2(1f, 1f);
		selectionCursor.sizeDelta = Vector2.zero;
		selectionCursor.anchoredPosition = Vector2.zero;
	}

	public void ShowItemMenu(ItemButton item)
	{
		wholeScreenInputBlocker.gameObject.SetActive(true);
		itemActionMenu.gameObject.SetActive(true);

		List<ItemAction> actions = item.item.actions;
		int i = 0;
		for (; i < actions.Count && i < itemActionMenu.childCount; ++i)
		{
			ItemActionButton button = itemActionMenu.GetChild(i).GetComponent<ItemActionButton>();
			button.action = actions[i];
		}
		for (; i < actions.Count; ++i)
		{
			ItemActionButton button = Instantiate(itemActionButtonPrefab);
			button.transform.SetParent(itemActionMenu);
			button.action = actions[i];
		}
		for (; i < itemActionMenu.childCount; ++i)
		{
			itemActionMenu.GetChild(i).gameObject.SetActive(false);
		}

		StartCoroutine(RepositionMenu(itemActionMenu, item.transform as RectTransform));
	}

	private IEnumerator RepositionMenu(RectTransform menu, RectTransform nearObject)
	{
		yield return null;
		float menuHeight = menu.rect.height;
		float nearObjectHeight = nearObject.rect.height;
		float y = nearObject.position.y - (menuHeight + nearObjectHeight) / 2;
		RectTransform container = transform as RectTransform;
		float minY = container.rect.yMin;
		if (y - menuHeight / 2 < minY)
		{
			y = nearObject.position.y + (menuHeight + nearObjectHeight) / 2;
		}

		float menuWidth = menu.rect.width;
		float x = nearObject.position.x;
		float minX = container.rect.xMin;
		float maxX = container.rect.xMax;
		if (x - menuWidth / 2 < minX)
		{
			x = minX + menuWidth / 2;
		}
		if (x + menuWidth / 2 > maxX)
		{
			x = maxX - menuWidth / 2;
		}
		
		menu.position = new Vector2(x, y);
	}

	public void HideItemMenu()
	{
		wholeScreenInputBlocker.gameObject.SetActive(false);
		itemActionMenu.gameObject.SetActive(false);
	}

	public void SetTransmutationMode()
	{
		HideItemMenu();
		transmutationMode = true;
		spiritInventoryInputBlocker.gameObject.SetActive(true);
		// TODO - maybe instruction text too?
	}

	public void SetBaseMode()
	{
		transmutationMode = false;
		spiritInventoryInputBlocker.gameObject.SetActive(false);
		// TODO - maybe instruction text too?
	}

	public void OnEnable()
	{
		List<InventoryItem> spiritItems = InventoryManager.playerInventory.items;
		PopulateInventoryPane(spiritInventoryContent, spiritItems);

		InventoryManager bodyInventory = InventoryManager.bodyInventory;
		if (bodyInventory != null)
		{
			List<InventoryItem> bodyItems = bodyInventory.items;
			PopulateInventoryPane(bodyInventoryContent, bodyItems);
		}
		else
		{
			List<InventoryItem> dummyItems = new List<InventoryItem>();
			PopulateInventoryPane(bodyInventoryContent, dummyItems);
		}
		HideItemMenu();
		SetBaseMode();
		selectionCursor.gameObject.SetActive(false);
		descriptionField.item = null;
	}

	private void PopulateInventoryPane(RectTransform pane, List<InventoryItem> items)
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
