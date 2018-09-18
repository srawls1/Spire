using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	public abstract string interactionText
	{
		get;
	}

	public abstract Coroutine Interact(GameObject interacter);
}
