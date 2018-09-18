using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
	#region Editor Fields

	[SerializeField] bool m_startOpen;
	[SerializeField] Sprite closedSprite;
	[SerializeField] Sprite openSprite;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private bool m_open;
	private bool m_locked;

	private new SpriteRenderer renderer;
	private new Collider2D collider;

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
			collider.enabled = !m_open;
			renderer.sprite = m_open ? openSprite : closedSprite;
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

	public override string interactionText
	{
		get
		{
			return open ? "Close Door" : "Open Door";
		}
	}

	public override Coroutine Interact(GameObject interacter)
	{
		return StartCoroutine(ToggleOpen());
	}

	#endregion // Overrides

	#region Unity Functions

	private void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<Collider2D>();
		open = m_startOpen;
	}

	#endregion // Unity Functions

	#region Private Functions

	private IEnumerator ToggleOpen()
	{
		open = !open;
		yield break;
	}

	#endregion // Private Functions
}
