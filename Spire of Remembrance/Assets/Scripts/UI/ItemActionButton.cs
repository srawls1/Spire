using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionButton : MonoBehaviour
{
	private Text actionText;
	private Button button;
	private InventoryMenu menu;

	private ItemAction m_action;
	public ItemAction action
	{
		get
		{
			return m_action;
		}
		set
		{
			m_action = value;
			if (m_action == null)
			{
				gameObject.SetActive(false);
			}
			else
			{
				gameObject.SetActive(true);
				actionText.text = m_action.actionString;
				button.interactable = m_action.canPerform;
			}
		}
	}

	void Awake()
	{
		actionText = GetComponentInChildren<Text>();
		button = GetComponent<Button>();
		menu = GetComponentInParent<InventoryMenu>();
	}

	public void PerformAction()
	{
		StartCoroutine(PerformActionRoutine());
	}

	private IEnumerator PerformActionRoutine()
	{
		yield return StartCoroutine(action.GetTarget());
		action.Perform();
		menu.OnEnable(); // Refresh the menu screen, since we may have just
		//	removed this item from our inventory or otherwise changed it
	}
}
