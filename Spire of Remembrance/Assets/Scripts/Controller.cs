using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
	protected Controller disabledController;
	protected Movement controlledMovement;

	protected void Awake()
	{
		Possess(GetComponent<Movement>());
	}

	public virtual void Possess(Movement move)
	{
		disabledController = move.CurrentController;
		move.CurrentController = this;
		controlledMovement = move;
	}
}
