using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Interactable
{
	[SerializeField] private Sprite closedSprite;
	[SerializeField] private Sprite openSprite;
	[SerializeField] private GameObject contentsPrefab;

	new private SpriteRenderer renderer;

	public override string interactionText
	{
		get
		{
			return "Open";
		}
	}

	private void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = closedSprite;
	}

	public override Coroutine Interact(GameObject interacter)
	{
		renderer.sprite = openSprite;
		Instantiate(contentsPrefab, transform.position, Quaternion.identity);
		return StartCoroutine(dummyCoroutine());
	}

	private IEnumerator dummyCoroutine()
	{
		Destroy(this);
		yield break;
	}
}
