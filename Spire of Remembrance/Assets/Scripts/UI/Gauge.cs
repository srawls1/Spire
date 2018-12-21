using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] protected Image fullBackground;
	[SerializeField] protected Image filledPortion;
	[SerializeField] protected Text text;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private RectTransform fullBackgroundRect;
	private RectTransform filledPortionRect;

	#endregion // Non-Editor Fields

	#region Unity Functions

	protected void Awake()
	{
		fullBackgroundRect = fullBackground.transform as RectTransform;
		filledPortionRect = filledPortion.transform as RectTransform;
	}

	#endregion // Unity Functions

	#region Public Functions

	public void UpdateUI(float current, float max)
	{
		if (text != null)
		{
			text.text = string.Format("{0}/{1}", current, max);
		}

		float portionFilled = (float)current / max;
		float totalWidth = fullBackgroundRect.rect.width;

		filledPortionRect.sizeDelta = new Vector2(portionFilled * totalWidth, filledPortionRect.sizeDelta.y);
		filledPortionRect.anchoredPosition = new Vector2(totalWidth * portionFilled * 0.5f, 0);
	}

	#endregion // Public Functions
}
