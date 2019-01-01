using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Tab : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private float inactiveXShrink;
	[SerializeField] private float inactiveYShrink;
	[SerializeField] private Color activeColor;
	[SerializeField] private Color inactiveColor;
	[SerializeField] private GameObject pane;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private Image image;
	private RectTransform rect;
	private bool active;

	#endregion // Non-Editor Fields

	#region Unity Functions

	private void Awake()
	{
		image = GetComponent<Image>();
		rect = transform as RectTransform;
		active = true;
	}

	#endregion // Unity Functions

	#region Public Functions

	public void SetActive()
	{
		if (pane != null)
		{
			pane.SetActive(true);
		}

		image.color = activeColor;
		if (!active)
		{
			active = true;
			rect.sizeDelta = new Vector2(rect.sizeDelta.x + inactiveXShrink,
				rect.sizeDelta.y + inactiveYShrink);
			rect.anchoredPosition = new Vector2(rect.anchoredPosition.x,
				rect.anchoredPosition.y + inactiveYShrink / 2);
		}
	}

	public void SetInactive()
	{
		if (pane != null)
		{
			pane.SetActive(false);
		}

		image.color = inactiveColor;
		if (active)
		{
			active = false;
			rect.sizeDelta = new Vector2(rect.sizeDelta.x - inactiveXShrink,
				rect.sizeDelta.y - inactiveYShrink);
			rect.anchoredPosition = new Vector2(rect.anchoredPosition.x,
				rect.anchoredPosition.y - inactiveYShrink / 2);
		}
	}

	#endregion // Public Functions
}
