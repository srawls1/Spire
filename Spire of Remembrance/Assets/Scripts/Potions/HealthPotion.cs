using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthPotion : GradualFillPotion
{
	[SerializeField] public float healScale;

	public HealthPotion(Sprite partial, Sprite full)
		: base(partial, full)
	{ }

	public override void Use(Controller controller, Bottle container)
	{
		EnemyHealth health = GetHealthTarget(controller);
		if (health == null)
		{
			return;
		}

		int roomToHeal = health.maxHealth - health.currentHealth;
		int ableToHeal = Mathf.RoundToInt(healScale * portionFull);
		int amountHealed = Mathf.Min(roomToHeal, ableToHeal);

		health.Heal(amountHealed);
		portionFull -= amountHealed / healScale;
		ClearIfEmpty(container);
	}

	protected abstract EnemyHealth GetHealthTarget(Controller controller);
}
