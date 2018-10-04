using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessable : Interactable
{
	private CharacterMovement characterMovement;

	private void Start()
	{
		characterMovement = CharacterController.instance.GetComponent<CharacterMovement>();
	}

	public override Interaction[] interactions
	{
		get
		{
			return new Interaction[]
			{
				new Interaction("Possess", possess, true)
			};
		}
	}

	private void possess()
	{
		characterMovement.Possess(gameObject);
	}
}
