using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Interaction
{
	public Interaction(string text, Action action, bool active)
	{
		interactionText = text;
		interact = action;
		enabled = active;
	}
		
	public string interactionText { get; private set; }
	public Action interact { get; private set; }
	public bool enabled { get; private set; }
}

public abstract class Interactable : MonoBehaviour
{
	public abstract Interaction[] interactions { get; }
}
