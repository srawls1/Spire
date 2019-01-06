using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointActivator : Activator
{
	[SerializeField] private Activator[] activators;
	[SerializeField] private int numRequired;

	private int numActive;

	private void Awake()
	{
		for (int i = 0; i < activators.Length; ++i)
		{
			activators[i].OnActivated += Activated;
			activators[i].OnDeactivated += Deactivated;
		}
	}

	private void Deactivated()
	{
		if (numActive-- == numRequired)
		{
			Deactivate();
		}
	}

	private void Activated()
	{
		if (++numActive == numRequired)
		{
			Activate();
		}
	}
}
