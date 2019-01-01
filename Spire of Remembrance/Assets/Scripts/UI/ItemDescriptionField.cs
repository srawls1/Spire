using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionField : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private Text itemNameText;
	[SerializeField] private Text itemDescText;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private InventoryItem m_item;

	#endregion // Non-Editor Fields

	#region Properties

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
				itemNameText.text = string.Empty;
				itemDescText.text = string.Empty;
			}
			else
			{
				itemNameText.text = item.name;
				itemDescText.text = item.description;
			}
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Start()
	{
		item = null;
	}

	#endregion // Unity Functions
}
