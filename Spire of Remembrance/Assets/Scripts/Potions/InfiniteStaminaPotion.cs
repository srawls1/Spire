using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteStaminaPotion : Potion
{
	private Sprite m_sprite;
	private float duration;

	public override string description
	{
		get
		{
			return "Gives infinite stamina for a limited time.";
		}
	}

	public override string name
	{
		get
		{
			return "Infinite Stamina Potion";
		}
	}

	public override Sprite sprite
	{
		get
		{
			return m_sprite;
		}
	}

	public InfiniteStaminaPotion(Sprite spr, float dur)
	{
		m_sprite = spr;
		duration = dur;
	}

	public override void Use(Controller controller, Bottle container)
	{
		Stamina stamina = CharacterController.instance.GetComponent<Stamina>();
		if (stamina == null)
		{
			return;
		}

		StaminaFilter filter = new InfiniteStaminaFilter();
		stamina.AddFilter(filter);
		stamina.RemoveFilter(filter, duration);

		container.containedPotion = null;
	}
}
