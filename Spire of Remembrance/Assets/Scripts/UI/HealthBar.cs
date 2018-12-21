using UnityEngine;
using UnityEngine.UI;

public class HealthBar : Gauge
{
	#region Editor Fields

	[SerializeField] private Damageable m_target;

	#endregion // Editor Fields

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
				if (text != null)
				{
					text.enabled = false;
				}
			}
			else
			{
				fullBackground.enabled = true;
				filledPortion.enabled = true;
				if (text != null)
				{
					text.enabled = true;
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
		target = target;
	}

	#endregion // Unity Functions

	#region Private Functions

	private void UpdateHealthBar(int current, int max)
	{
		UpdateUI(current, max);
	}

	#endregion // Private Functions
}
