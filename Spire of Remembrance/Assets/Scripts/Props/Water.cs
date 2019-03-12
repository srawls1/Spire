using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Pit
{
	#region Editor Fields

	[SerializeField] private Sprite waterSprite;
	[SerializeField] private Sprite iceSprite;
	[SerializeField] private bool m_frozen;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private new SpriteRenderer renderer;
	private List<Collider2D> currentlyInside;
	private NavObstacle obstacle;

	#endregion // Non-Editor Fields

	#region Properties

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
			obstacle.terrainType = m_frozen ? NavTerrainTypes.Floor : NavTerrainTypes.Pit;
			if (m_frozen)
			{
				for (int i = currentlyInside.Count - 1; i >= 0; --i)
				{
					base.OnTriggerEnter2D(currentlyInside[i]);
					currentlyInside.RemoveAt(i);
				}
			}
		}
	}

	#endregion // Properties

	#region Unity Message

	public new void Awake()
	{
		base.Awake();
		renderer = GetComponent<SpriteRenderer>();
		obstacle = GetComponent<NavObstacle>();
		currentlyInside = new List<Collider2D>();
		frozen = frozen;
	}

	protected new void OnTriggerEnter2D(Collider2D other)
	{
		if (frozen)
		{
			currentlyInside.Add(other);
		}
		else
		{
			base.OnTriggerEnter2D(other);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		currentlyInside.Remove(other);
	}

	public void OnFireDamage(FireDamageArgs args)
	{
		frozen = false;
	}

	public void OnIceDamage(IceDamageArgs args)
	{
		frozen = true;
	}

	#endregion // Unity Messages
}
