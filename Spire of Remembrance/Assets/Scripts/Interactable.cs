using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	[SerializeField] private string m_interactionText;

	public string interactionText
	{
		get
		{
			return m_interactionText;
		}
	}

	public abstract Coroutine Interact(GameObject interacter);
}
