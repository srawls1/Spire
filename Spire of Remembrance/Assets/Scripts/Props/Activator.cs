using System;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
	public event Action OnActivated;
	public event Action OnDeactivated;

	public void Activate()
	{
		if (OnActivated != null)
		{
			OnActivated();
		}
	}

	public void Deactivate()
	{
		if (OnDeactivated != null)
		{
			OnDeactivated();
		}
	}
}
