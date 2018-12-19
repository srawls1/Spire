using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeepUICentered : MonoBehaviour
{
	private RectTransform rect;

	private void Awake()
	{
		rect = transform as RectTransform;
	}

	private void Update()
	{
		rect.anchoredPosition = Vector2.zero;
	}
}
