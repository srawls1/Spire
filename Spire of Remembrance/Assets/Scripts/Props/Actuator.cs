using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actuator : MonoBehaviour
{
	[SerializeField] private Activator activator;

	protected void Start()
	{
		activator.OnActivated += Actuate;
		activator.OnDeactivated += Deactuate;
	}

	protected abstract void Actuate();
	protected abstract void Deactuate();
}
