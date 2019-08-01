using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightResistPotion : Potion
{
	private Sprite m_sprite;
	private float duration;

	public override string description
	{
		get
		{
			return "Temporarily stops damage from light when not possessing a body.";
		}
	}

	public override string name
	{
		get
		{
			return "Light Resist Potion";
		}
	}

	public override Sprite sprite
	{
		get
		{
			return m_sprite;
		}
	}

	public LightResistPotion(Sprite spr, float dur)
	{
		m_sprite = spr;
		duration = dur;
	}

	public override void Use(Controller controller, Bottle container)
	{
		SpiritHealth health = CharacterController.instance.GetComponent<SpiritHealth>();
		health.ResistLight(duration);
		container.containedPotion = null;
	}
}
