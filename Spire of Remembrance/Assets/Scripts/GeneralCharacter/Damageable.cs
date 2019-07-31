using System;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
	public event Action<int, int> OnHealthChanged;

	protected void HealthChanged(int current, int max)
	{
		if (OnHealthChanged != null)
		{
			OnHealthChanged(current, max);
		}
	}

	public abstract void TakeDamage(float damage, Vector3 damagerPosition, float force);

}
