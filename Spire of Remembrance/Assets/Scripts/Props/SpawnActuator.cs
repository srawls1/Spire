using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActuator : Actuator
{
	[SerializeField] private bool startSpawned;

	new protected void Start()
	{
		base.Start();
		gameObject.SetActive(startSpawned);
	}

	protected override void Actuate()
	{
		gameObject.SetActive(true);
	}

	protected override void Deactuate()
	{
		gameObject.SetActive(false);
	}
}
