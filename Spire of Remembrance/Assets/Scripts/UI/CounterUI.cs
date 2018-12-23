using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterUI : MonoBehaviour
{
	private Text countText;

	private void Awake()
	{
		countText = GetComponentInChildren<Text>();
	}

	public void SetCount(int count)
	{
		countText.text = "x" + count;
	}
}
