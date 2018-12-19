using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Interactable
{
	[SerializeField] private Sprite closedSprite;
	[SerializeField] private Sprite openSprite;
	[SerializeField] private GameObject contentsPrefab;
	[SerializeField] private Facing facing;

	new private SpriteRenderer renderer;

	public override Interaction[] interactions
	{
		get
		{
			return new Interaction[]
			{
				new Interaction("Open Chest", interact, true)
			};
		}
	}

	private void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = closedSprite;
	}

	private void interact()
	{
		renderer.sprite = openSprite;
		Vector3 itemPosition = transform.position;
		Vector3 displacementDirection = getDirectionForFacing();
		Collider2D collider = GetComponent<Collider2D>();
		while (collider.OverlapPoint(itemPosition))
		{
			itemPosition += displacementDirection * 0.1f;
		}
		Instantiate(contentsPrefab, itemPosition, Quaternion.identity);
		Destroy(this);
	}

	private Vector3 getDirectionForFacing()
	{
		switch (facing)
		{
			case Facing.down:
				return Vector3.down;
			case Facing.up:
				return Vector3.up;
			case Facing.right:
				return Vector3.right;
			case Facing.left:
				return Vector3.left;
		}
		return Vector3.zero;
	}
}
