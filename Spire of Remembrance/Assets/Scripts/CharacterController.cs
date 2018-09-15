using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : Controller
{
	#region Private Fields

	private CharacterMovement spiritMovement;
	private LinkedList<GameObject> interactables;

	#endregion // Private Fields

	#region Events

	public event Action<GameObject, bool> OnInteractableChanged;

	#endregion // Events

	#region Unity Functions

	new void Awake()
	{
		interactables = new LinkedList<GameObject>();
		spiritMovement = GetComponent<CharacterMovement>();
		base.Awake();
	}

	void Update()
	{
		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (controlledMovement != null)
		{
			controlledMovement.Walk(input);
		}

		if (Input.GetButtonDown("Attack"))
		{
			controlledMovement.Attack();
		}

		if (Input.GetButtonDown("RotateInteractable") && interactables.Count > 1)
		{
			GameObject obj = interactables.First.Value;
			interactables.RemoveFirst();
			interactables.AddLast(obj);
			
			if (OnInteractableChanged != null)
			{
				OnInteractableChanged(interactables.First.Value, true);
			}
		}

		if (Input.GetButtonDown("Interact") && interactables.Count > 0)
		{
			controlledMovement.Interact(interactables.First.Value);
		}

		if (Input.GetButtonDown("Possess"))
		{
			if (controlledMovement == spiritMovement && interactables.Count > 0)
			{
				spiritMovement.Possess(interactables.First.Value);
			}
			else
			{
				Deposess();
			}
		}
	}

	#endregion // Unity Functions

	#region Public Functions

	public override void Possess(Movement move)
	{
		if (controlledMovement != null)
		{
			controlledMovement.OnEnteredInteractable -= AddInteractable;
			controlledMovement.OnExitedInteractable -= RemoveInteractable;
		}

		interactables.Clear();
		
		if (OnInteractableChanged != null)
		{
			OnInteractableChanged(null, false);
		}

		base.Possess(move);

		if (move != null)
		{
			controlledMovement.OnEnteredInteractable += AddInteractable;
			controlledMovement.OnExitedInteractable += RemoveInteractable;
		}
	}

	public void Deposess()
	{
		controlledMovement.CurrentController = controlledMovement.GetComponent<Controller>();
		Possess(GetComponent<Movement>());
		spiritMovement.Depossess();
	}

	#endregion // Public Functions

	#region Private Functions

	private void AddInteractable(GameObject obj)
	{
		interactables.AddFirst(obj);

		if (OnInteractableChanged != null)
		{
			OnInteractableChanged(obj, interactables.Count > 1);
		}
	}

	private void RemoveInteractable(GameObject obj)
	{
		interactables.Remove(obj);
		if (interactables.Count > 0)
		{
			if (OnInteractableChanged != null)
			{
				OnInteractableChanged(interactables.First.Value, interactables.Count > 1);
			}
		}
		else
		{
			if (OnInteractableChanged != null)
			{
				OnInteractableChanged(null, interactables.Count > 0);
			}
		}
	}

	#endregion // Private Functions
}
