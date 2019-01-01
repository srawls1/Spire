using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingCursor : MonoBehaviour
{
	[SerializeField] private Color transparentColor;
	[SerializeField] private Color fullColor = Color.white;
	[SerializeField] private float frequency = 1f;

	private Image image;

	void Awake()
	{
		image = GetComponent<Image>();
	}

	void OnEnable()
	{
		StartCoroutine(Flash());
	}

	private IEnumerator Flash()
	{
		float timePassed = 0f;

		while (enabled)
		{
			yield return null;
			timePassed += Time.unscaledDeltaTime * 2 * Mathf.PI * frequency;
			float alpha = (Mathf.Sin(timePassed) + 1f) / 2f;
			image.color = Color.Lerp(transparentColor, fullColor, alpha);
		}
	}
}
