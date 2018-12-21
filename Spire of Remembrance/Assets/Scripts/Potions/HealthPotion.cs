using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthPotion : GradualFillPotion
{
	public float healScale
	{
		get; private set;
	}

	public HealthPotion(Sprite partial, Sprite full, float scale)
		: base(partial, full)
	{
		healScale = scale;
	}

	public override void Use(Controller controller, Bottle container)
	{
		EnemyHealth health = GetHealthTarget(controller);
		portionFull = HealTarget(portionFull, healScale, health);
		ClearIfEmpty(container);
	}

	public static float HealTarget(float portion, float scale, EnemyHealth target)
	{
		if (target == null)
		{
			return portion;
		}

		int roomToHeal = target.maxHealth - target.currentHealth;
		int ableToHeal = Mathf.RoundToInt(scale * portion);
		int amountHealed = Mathf.Min(roomToHeal, ableToHeal);

		target.Heal(amountHealed);
		portion -= amountHealed / scale;
		return portion;
	}

	protected abstract EnemyHealth GetHealthTarget(Controller controller);
}
