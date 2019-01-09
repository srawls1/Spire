using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLabel : MonoBehaviour
{
	[SerializeField] private Color enabledButtonColor;
	[SerializeField] private Color disabledButtonColor;

	private Text actionText;
	private Image buttonIcon;

	public string action
	{
		get
		{
			return actionText.text;
		}
		set
		{
			actionText.text = value;
			if (string.IsNullOrEmpty(value))
			{
				buttonIcon.color = disabledButtonColor;
			}
			else
			{
				buttonIcon.color = enabledButtonColor;
			}
		}
	}

	private void Awake()
	{
		actionText = GetComponentInChildren<Text>();
		buttonIcon = GetComponentInChildren<Image>();
		action = string.Empty;
	}
}
