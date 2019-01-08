using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schematics : Interactable
{
	[SerializeField] private Sprite mapSprite;
	[SerializeField] private MapScreen mapScreen;

	public override Interaction[] interactions
	{
		get
		{
			return new Interaction[]
			{
				new Interaction("Copy Map", FillInMap, true)
			};
		}
	}

	private void FillInMap()
	{
		mapScreen.mapSprite = mapSprite;
		Destroy(this);
	}
}
