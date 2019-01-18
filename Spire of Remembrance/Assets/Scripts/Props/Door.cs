using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
	#region Editor Fields

	[SerializeField] private bool m_open;
	[SerializeField] private bool m_locked;
	[SerializeField] private Sprite closedSprite;
	[SerializeField] private Sprite openSprite;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private new SpriteRenderer renderer;
	private new Collider2D collider;
	private int startingLayer;

	#endregion // Non-Editor Fields

	#region Properties

	public bool open
	{
		get
		{
			return m_open;
		}
		set
		{
			if (locked)
			{
				return;
			}

			m_open = value;
			collider.isTrigger = m_open;
			renderer.sprite = m_open ? openSprite : closedSprite;
			//gameObject.layer = m_open ?  LayerMask.NameToLayer("Interactable") : startingLayer;
		}
	}

	public bool locked
	{
		get
		{
			return m_locked;
		}
		set
		{
			m_locked = value;
		}
	}

	#endregion // Properties

	#region Overrides

	public override Interaction[] interactions
	{
		get
		{
			if (open)
			{
				return new Interaction[]
				{
					//new Interaction("Close Door", ToggleOpen, true)
				};
			}
			else if (locked)
			{
				return new Interaction[]
				{
					new Interaction("Unlock", ToggleLocked, CharacterController.instance.numKeys > 0)
				};
				//return new Interaction[]
				//{
				//	new Interaction("Open Door", ToggleOpen, !locked),
				//	new Interaction(locked ? "Unlock" : "Lock", ToggleLocked, CharacterController.instance.numKeys > 0)
				//};
			}
			else
			{
				return new Interaction[]
				{
					new Interaction("Open Door", ToggleOpen, true)
				};
			}
		}
	}

	#endregion // Overrides

	#region Unity Functions

	private void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<Collider2D>();
		startingLayer = gameObject.layer;
		open = open;
		locked = locked;
	}

	#endregion // Unity Functions

	#region Private Functions

	private void ToggleOpen()
	{
		open = !open;
	}

	private void ToggleLocked()
	{
		if (CharacterController.instance.numKeys > 0)
		{
			CharacterController.instance.useKey();
			locked = !locked;
		}
	}

	#endregion // Private Functions
}
