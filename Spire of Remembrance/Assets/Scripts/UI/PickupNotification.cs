using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupNotification : MonoBehaviour
{
	private Text text;

	public string message
	{
		get
		{
			return text.text;
		}
		set
		{
			text.text = value;
		}
	}

	private void Awake()
	{
		text = GetComponentInChildren<Text>();
	}
}
