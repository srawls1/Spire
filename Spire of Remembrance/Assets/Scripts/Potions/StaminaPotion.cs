using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPotion : GradualFillPotion
{
	public float restoreScale
	{
		get; private set;
	}

	public StaminaPotion(Sprite partial, Sprite full, float scale)
		: base(partial, full)
	{
		restoreScale = scale;
	}

	public override void Use(Controller controller, Bottle container)
	{
		portionFull = RestoreCharacterStamina(portionFull, restoreScale);
		ClearIfEmpty(container);
	}

	public static float RestoreCharacterStamina(float portion, float scale)
	{
		Stamina stamina = CharacterController.instance.GetComponent<Stamina>();
		if (stamina == null)
		{
			return portion;
		}

		float roomToRestore = stamina.totalStamina - stamina.currentStamina;
		float ableToRestore = scale * portion;
		float amountRestored = Mathf.Min(roomToRestore, ableToRestore);

		stamina.Restore(amountRestored);
		portion -= amountRestored / scale;
		return portion;
	}
}
