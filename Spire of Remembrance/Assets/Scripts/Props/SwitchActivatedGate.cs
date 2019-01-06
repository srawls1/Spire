using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActivatedGate : Actuator
{
	[SerializeField] private bool m_open;
	[SerializeField] private Sprite closedSprite;
	[SerializeField] private Sprite openSprite;

	new private Collider2D collider;
	new private SpriteRenderer renderer;

	protected bool open
	{
		get
		{
			return m_open;
		}
		set
		{
			m_open = value;

			renderer.sprite = m_open ? openSprite : closedSprite;
			collider.enabled = !m_open;
		}
	}

	private void Awake()
	{
		collider = GetComponent<Collider2D>();
		renderer = GetComponent<SpriteRenderer>();
	}

	new protected void Start()
	{
		base.Start();
		open = open;
	}

	protected override void Actuate()
	{
		open = true;
	}

	protected override void Deactuate()
	{
		open = false;
	}
}
