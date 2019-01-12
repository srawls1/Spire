using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region D-Pad Input Struct

struct DPadAxisState
{
	private bool rightPressedPrev;
	private bool leftPressedPrev;
	private bool upPressedPrev;
	private bool downPressedPrev;

	public bool rightPressed;
	public bool leftPressed;
	public bool upPressed;
	public bool downPressed;

	public bool right { get { return rightPressed && !rightPressedPrev; } }
	public bool left { get { return leftPressed && !leftPressedPrev; } }
	public bool up { get { return upPressed && !upPressedPrev; } }
	public bool down { get { return downPressed && !downPressedPrev; } }

	public bool this[int n]
	{
		get
		{
			switch (n)
			{
				case 0: return right;
				case 1: return up;
				case 2: return left;
				case 3: return down;
				default: return false;
			}
		}
	}

	public void Update(float horiz, float vertic)
	{
		rightPressedPrev = rightPressed;
		leftPressedPrev = leftPressed;
		upPressedPrev = upPressed;
		downPressedPrev = downPressed;

		rightPressed = horiz > 0.5f;
		leftPressed = horiz < -0.5f;
		upPressed = vertic > 0.5f;
		downPressed = vertic < -0.5f;
	}
}

#endregion // D-Pad Input Struct

public class CharacterController : Controller
{
	#region Private Fields

	private CharacterMovement spiritMovement;
	private Interactable interactable;
	private Interaction[] interactions;
	private List<InventoryItem> availableWeapons;
	private DPadAxisState dpad = new DPadAxisState();
	private int interactableIndex;
	private int m_numKeys;
	private int m_numSpiritOrbs;

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
		private set
		{
			m_numKeys = value;
			InGameUIManager.instance.SetNumKeys(m_numKeys);
		}
	}

	public int numSpiritOrbs
	{
		get
		{
			return m_numSpiritOrbs;
		}
		private set
		{
			m_numSpiritOrbs = value;
			InGameUIManager.instance.SetNumSpiritOrbs(m_numSpiritOrbs);
		}
	}

	#endregion // Properties

	#region Events

	public event Action<Interaction, bool> OnInteractableChanged;

	#endregion // Events

	#region Unity Functions

	void Awake()
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
	}

	new void Start()
	{
		base.Start();
		numSpiritOrbs = numSpiritOrbs;
		numKeys = numKeys;
	}

	void Update()
	{
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if (controlledMovement != null)
		{
			controlledMovement.Walk(input);
		}

		if (Input.GetButtonDown("Attack"))
		{
			controlledMovement.Attack();
		}

		if (Input.GetButtonDown("RotateInteractable") && interactions.Length > 1)
		{
			++interactableIndex;
			interactableIndex %= interactions.Length;
			Interaction inter = interactions[interactableIndex];

			if (OnInteractableChanged != null)
			{
				OnInteractableChanged(inter, true);
			}
		}

		if (Input.GetButtonDown("Interact") && interactions.Length > 0 && interactions[interactableIndex].enabled)
		{
			interactions[interactableIndex].interact();
			RefreshInteractable();
		}

		if (Input.GetButtonDown("Possess"))
		{
			if (controlledMovement != spiritMovement)
			{
				Deposess();
			}
		}

		dpad.Update(Input.GetAxisRaw("ItemH"), Input.GetAxisRaw("ItemV"));
		if (availableWeapons != null)
		{
			for (int i = 0; i < availableWeapons.Count; ++i)
			{
				if (Input.GetButtonDown("Weapon" + i) || dpad[i])
				{
					EquipWeapon(availableWeapons[i]);
				}
			}
		}
	}

	#endregion // Unity Functions

	#region Public Functions

	public void gainKey()
	{
		++numKeys;
	}

	public void useKey()
	{
		--numKeys;
	}

	public void gainSpiritOrb()
	{
		++numSpiritOrbs;
	}

	public void useSpiritOrbs(int count)
	{
		numSpiritOrbs -= count;
	}

	public override void Possess(Movement move)
	{
		if (controlledMovement != null)
		{
			controlledMovement.OnNewInteractable -= SetInteractions;
		}

		RefreshInteractable();

		if (OnInteractableChanged != null)
		{
			OnInteractableChanged(null, false);
		}

		base.Possess(move);

		if (move != null)
		{
			controlledMovement.OnNewInteractable += SetInteractions;
		}

		AITarget target = controlledMovement.GetComponent<AITarget>();
		if (target != null)
		{
			target.alignment = Alignment.Player;
		}
		ReconcileDefaultWeapons(InventoryManager.playerInventory, InventoryManager.bodyInventory);
	}

	public Coroutine Deposess()
	{
		AITarget target = controlledMovement.GetComponent<AITarget>();
		if (target != null)
		{
			target.ResetAlignmentFrom(Alignment.Player);
		}
		ResetDefaultWeapons(InventoryManager.playerInventory, InventoryManager.bodyInventory);
		controlledMovement.CurrentController = disabledController;
		Possess(GetComponent<Movement>());
		return spiritMovement.Depossess();
	}

	public Coroutine TurnPhysical()
	{
		return StartCoroutine(TurnPhysicalRoutine());
	}

	#endregion // Public Functions

	#region Private Functions

	private IEnumerator TurnPhysicalRoutine()
	{
		yield return Deposess();
		spiritMovement.TurnPhysical();
	}

	private void RefreshInteractable()
	{
		SetInteractions(null);
	}

	private void SetInteractions(Interactable inter)
	{
		//if (inter == interactable)
		//{
		//	return;
		//}
		interactable = inter;
		interactions = inter != null ? inter.interactions : empty;
		interactableIndex = 0;
		Interaction interaction = interactions.Length > 0 ? interactions[0] : null;

		if (OnInteractableChanged != null)
		{
			OnInteractableChanged(interaction, interactions.Length > 1);
		}
	}

	private void ReconcileDefaultWeapons(InventoryManager playerInventory, InventoryManager bodyInventory)
	{
		availableWeapons = controlledMovement.UpdateInventoryWeapons(playerInventory, bodyInventory);

		if (bodyInventory == null)
		{
			return;
		}

		if (playerInventory.equippedBow == playerInventory.defaultBow &&
			bodyInventory.defaultBow != null)
		{
			playerInventory.equippedBow = bodyInventory.defaultBow;
		}

		if (playerInventory.equippedStaff == playerInventory.defaultStaff &&
			bodyInventory.defaultStaff != null)
		{
			playerInventory.equippedStaff = bodyInventory.defaultStaff;
		}

		if (playerInventory.equippedSword == playerInventory.defaultSword &&
			bodyInventory.equippedSword != null)
		{
			playerInventory.equippedSword = bodyInventory.defaultSword;
		}
	}

	private void ResetDefaultWeapons(InventoryManager playerInventory, InventoryManager bodyInventory)
	{
		controlledMovement.CleanUpInventoryEvents(playerInventory, bodyInventory);

		if (bodyInventory == null)
		{
			return;
		}

		if (playerInventory.equippedSword == bodyInventory.defaultSword)
		{
			playerInventory.equippedSword = playerInventory.defaultSword;
		}

		if (playerInventory.equippedStaff == bodyInventory.defaultStaff)
		{
			playerInventory.equippedStaff = playerInventory.defaultStaff;
		}

		if (playerInventory.equippedBow == bodyInventory.defaultBow)
		{
			playerInventory.equippedBow = playerInventory.defaultBow;
		}
	}

	private void EquipWeapon(InventoryItem inventoryItem)
	{
		List<ItemAction> actions = inventoryItem.actions;
		ItemAction equipAction = actions.Find((action) => action.actionString.Equals("Equip"));
		if (equipAction != null)
		{
			equipAction.Perform();
		}
	}

	#endregion // Private Functions
}
