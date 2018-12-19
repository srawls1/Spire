using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private Damageable m_target;
	[SerializeField] private Image fullBackground;
	[SerializeField] private Image filledPortion;
	[SerializeField] private Text healthText;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private RectTransform fullBackgroundRect;
	private RectTransform filledPortionRect;

	#endregion // Non-Editor Fields

	#region Properties

	public Damageable target
	{
		get
		{
			return m_target;
		}
		set
		{
			if (m_target != null)
			{
				m_target.OnHealthChanged -= UpdateHealthBar;
			}

			m_target = value;
			if (m_target == null)
			{
				fullBackground.enabled = false;
				filledPortion.enabled = false;
				if (healthText != null)
				{
					healthText.enabled = false;
				}
			}
			else
			{
				fullBackground.enabled = true;
				filledPortion.enabled = true;
				if (healthText != null)
				{
					healthText.enabled = true;
				}

				m_target.OnHealthChanged += UpdateHealthBar;

				// Do dummy damage to trigger the health changed event and update the bar
				m_target.TakeDamage(0, m_target.transform.position, 0f);
			}
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Start()
	{
		fullBackgroundRect = fullBackground.transform as RectTransform;
		filledPortionRect = filledPortion.transform as RectTransform;
		target = target;
	}

	#endregion // Unity Functions

	#region Private Functions

	private void UpdateHealthBar(int current, int max)
	{
		if (healthText != null)
		{
			healthText.text = string.Format("{0}/{1}", current, max);
		}

		float portionFilled = (float)current / max;
		float totalWidth = fullBackgroundRect.rect.width;

		filledPortionRect.sizeDelta = new Vector2(portionFilled * totalWidth, filledPortionRect.sizeDelta.y);
		filledPortionRect.anchoredPosition = new Vector2(totalWidth * portionFilled * 0.5f, 0);
	}

	#endregion // Private Functions
}
