using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegateActivator : Activator
{
	[SerializeField] private Activator activator;

	private void Awake()
	{
		activator.OnActivated += Activated;
		activator.OnDeactivated += Deactivated;
	}

	private void Deactivated()
	{
		Activate();
	}

	private void Activated()
	{
		Deactivate();
	}
}
