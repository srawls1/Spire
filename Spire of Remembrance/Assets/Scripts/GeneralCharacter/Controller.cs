using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
	protected Controller disabledController;

	public Movement controlledMovement
	{
		get; protected set;
	}

	protected void Start()
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
