using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Pit
{
	[SerializeField] private Sprite waterSprite;
	[SerializeField] private Sprite iceSprite;
	[SerializeField] private bool m_frozen;

	private SpriteRenderer renderer;
	private Collider2D collider;

	private bool frozen
	{
		get
		{
			return m_frozen;
		}
		set
		{
			m_frozen = value;
			renderer.sprite = m_frozen ? iceSprite : waterSprite;
			//collider.enabled = !m_frozen;
		}
	}

	public void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<Collider2D>();
		frozen = frozen;
	}

	protected new void OnTriggerEnter2D(Collider2D other)
	{
		if (frozen)
		{
			return;
		}
		else
		{
			base.OnTriggerEnter2D(other);
		}
	}

	public void OnFireDamage()
	{
		frozen = false;
	}

	public void OnIceDamage()
	{
		frozen = true;
	}
}
