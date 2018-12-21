using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBar : Gauge
{
	[SerializeField] private Stamina target;

	void Start()
	{
		if (target != null)
		{
			target.OnStaminaChanged += OnStaminaChanged;
		}
	}

	private void OnDestroy()
	{
		if (target != null)
		{
			target.OnStaminaChanged -= OnStaminaChanged;
		}
	}

	private void OnStaminaChanged(float current, float max)
	{
		UpdateUI(current, max);
	}
}
