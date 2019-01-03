using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : ItemMenu
{
	#region Editor Fields

	[SerializeField] private RectTransform spiritInventoryContent;
	[SerializeField] private RectTransform bodyInventoryContent;
	[SerializeField] private RectTransform wholeScreenInputBlocker;
	[SerializeField] private RectTransform spiritInventoryInputBlocker;
	[SerializeField] private RectTransform selectionCursor;
	[SerializeField] private ItemDescriptionField descriptionField;
	[SerializeField] private RectTransform itemActionMenu;
	[SerializeField] private ItemActionButton itemActionButtonPrefab;

	#endregion // Editor Fields

	#region Properties/Non-Editor Fields

	public bool transmutationMode
	{
		get; private set;
	}
	private Action<InventoryItem> transmutationCallback;

	#endregion // Properties/Non-Editor Fields

	#region Public Functions

	public override void OnItemHover(ItemButton item)
	{
		descriptionField.item = item.item;
		selectionCursor.gameObject.SetActive(true);
		selectionCursor.SetParent(item.transform);
		selectionCursor.anchorMin = Vector2.zero;
		selectionCursor.anchorMax = new Vector2(1f, 1f);
		selectionCursor.sizeDelta = Vector2.zero;
		selectionCursor.anchoredPosition = Vector2.zero;
	}

	public override void OnItemClick(ItemButton item)
	{
		if (!transmutationMode)
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
		else
		{
			transmutationCallback(item.item);
		}
	}

	public void HideItemMenu()
	{
		wholeScreenInputBlocker.gameObject.SetActive(false);
		itemActionMenu.gameObject.SetActive(false);
	}

	public void SetTransmutationMode(Action<InventoryItem> callback)
	{
		HideItemMenu();
		transmutationMode = true;
		transmutationCallback = callback;
		spiritInventoryInputBlocker.gameObject.SetActive(true);
		// TODO - maybe instruction text too?
	}

	public void SetBaseMode()
	{
		transmutationMode = false;
		transmutationCallback = null;
		spiritInventoryInputBlocker.gameObject.SetActive(false);
		// TODO - maybe instruction text too?
	}

	public void PerformAction(ItemAction action)
	{
		StartCoroutine(PerformActionRoutine(action));
	}

	#endregion // Public Functions

	#region Unity Functions

	private void OnEnable()
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

	#endregion // Unity Functions

	#region Private Functions

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

	private IEnumerator PerformActionRoutine(ItemAction action)
	{
		yield return StartCoroutine(action.GetTarget());
		action.Perform();
		OnEnable(); // Refresh the screen
	}

	

	#endregion // Private Functions
}
