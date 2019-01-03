using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantInteractable : Interactable
{
	#region Editor Fields

	[SerializeField] private SerializedInventoryItem[] inventory;
	[SerializeField] private List<int> prices;
	[SerializeField] private string[] dialogue;
	[SerializeField] private float markRadius;
	[SerializeField] private float markDuration;
	[SerializeField] private ShopMenu menu;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private InventoryManager manager;

	#endregion // Non-Editor Fields

	#region Properties

	public override Interaction[] interactions
	{
		get
		{
			return new Interaction[]
			{
				new Interaction("Shop", () => StartCoroutine(ShopRoutine()),
					CharacterController.instance.controlledMovement.GetComponent<MarkedStatus>() == null)
			};
			throw new NotImplementedException();
		}
	}

	#endregion // Properties

	#region Unity Functions

	protected void Awake()
	{
		manager = GetComponent<InventoryManager>();
		for (int i = 0; i < inventory.Length; ++i)
		{
			manager.Add(new MarkedInventoryItem(inventory[i].ToItem(), markRadius, markDuration));
		}
	}

	#endregion // Unity Functions

	#region Public Functions

	public bool BuyIfPossible(InventoryItem item)
	{
		List<InventoryItem> items = manager.items;
		int index = items.IndexOf(item);
		int price = prices[index];

		if (CharacterController.instance.numSpiritOrbs >= price)
		{
			CharacterController.instance.useSpiritOrbs(price);
			manager.Remove(item);
			prices.RemoveAt(index);
			MarkedInventoryItem marked = item as MarkedInventoryItem;
			InventoryManager.playerInventory.Add(marked.innerItem);
			menu.SetMerchantInfo(this, manager.items, prices);
			return true;
		}
		return false;
	}

	#endregion // Public Functions

	#region Private Functions

	private IEnumerator ShopRoutine()
	{
		Time.timeScale = 0f;
		yield return DialogueBox.instance.GoThroughDialogue(dialogue);
		menu.gameObject.SetActive(true);
		menu.SetMerchantInfo(this, manager.items, prices);
		// TODO - It's coming time to implement an overall UI manager
		yield return StartCoroutine(WaitForCloseInput());
		menu.gameObject.SetActive(false);
	}

	private IEnumerator WaitForCloseInput()
	{
		while (true)
		{
			yield return null;
			if (Input.GetButtonDown("Pause"))
			{
				break;
			}
		}
	}

	#endregion // Private Functions
}
