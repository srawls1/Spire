using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : Controller
{
	#region Private Fields

	private CharacterMovement spiritMovement;
	private Interaction[] interactables;
	private int interactableIndex;
	private int m_numKeys;

	private static readonly Interaction[] empty = { };
	private static CharacterController m_instance;

	#endregion // Private Fields

	#region Properties

	public static CharacterController instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<CharacterController>();
			}
			return m_instance;
		}
	}

	public int numKeys
	{
		get
		{
			return m_numKeys;
		}
			
	}

	#endregion // Properties

	#region Events

	public event Action<Interaction, bool> OnInteractableChanged;

	#endregion // Events

	#region Unity Functions

	new void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
		else if (m_instance != this)
		{
			Debug.LogWarning("Multiple character controllers in scene! Destroying one");
			Destroy(gameObject);
		}
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

		if (Input.GetButtonDown("RotateInteractable") && interactables.Length > 1)
		{
			++interactableIndex;
			interactableIndex %= interactables.Length;
			Interaction inter = interactables[interactableIndex];
			
			if (OnInteractableChanged != null)
			{
				OnInteractableChanged(inter, true);
			}
		}

		if (Input.GetButtonDown("Interact") && interactables.Length > 0 && interactables[interactableIndex].enabled)
		{
			interactables[interactableIndex].interact();
			RefreshInteractable();
		}

		if (Input.GetButtonDown("Possess"))
		{
			if (controlledMovement != spiritMovement)
			{
				Deposess();
			}
		}
	}

	#endregion // Unity Functions

	#region Public Functions

	public void gainKey()
	{
		++m_numKeys;
	}

	public void useKey()
	{
		--m_numKeys;
	}

	public override void Possess(Movement move)
	{
		if (controlledMovement != null)
		{
			controlledMovement.OnNewInteractables -= SetInteractions;
		}

		interactables = empty;
		
		if (OnInteractableChanged != null)
		{
			OnInteractableChanged(null, false);
		}

		base.Possess(move);

		if (move != null)
		{
			controlledMovement.OnNewInteractables += SetInteractions;
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

	private void RefreshInteractable()
	{
		controlledMovement.RefreshInteracable();
	}

	private void SetInteractions(Interaction[] inters)
	{
		interactables = inters != null ? inters : empty;
		interactableIndex = 0;
		Interaction interaction = interactables.Length > 0 ? interactables[0] : null;

		if (OnInteractableChanged != null)
		{
			OnInteractableChanged(interaction, interactables.Length > 1);
		}
	}

	#endregion // Private Functions
}
