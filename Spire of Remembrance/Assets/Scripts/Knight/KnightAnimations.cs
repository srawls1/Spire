using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimations : EntityAnimations
{
	#region Non-Editor Fields

	private Weapon weapon;
	private SwordItem defaultSword;

	#endregion // Non-Editor Fields

	#region Properties

	public WeaponData weaponData
	{
		get
		{
			return weapon.data;
		}
		set
		{
			weapon.data = value;
		}
	}

	#endregion // Properties

	#region Unity Functions

	private new void Awake()
	{
		base.Awake();
		weapon = GetComponentInChildren<Weapon>();
	}

	private void Start()
	{
		weapon.gameObject.SetActive(false);
	}

	#endregion // Unity Functions

	#region Public Functions

	public override List<InventoryItem> UpdateInventoryWeapons(InventoryManager playerInventory, InventoryManager bodyInventory)
	{
		List<SwordItem> playerSwords = playerInventory.GetAllSwords();
		List<InventoryItem> ret = new List<InventoryItem>();
		defaultSword = bodyInventory.defaultSword;
		defaultSword.manager = playerInventory;
		ret.Add(defaultSword);
		for (int i = 0; i < playerSwords.Count; ++i)
		{
			ret.Add(playerSwords[i]);
		}

		WeaponSelectionUI.instance.SetAvailableWeapons(ret);
		UpdateWeaponSelectedUI(playerInventory.equippedSword);

		playerInventory.OnNewSwordEquipped += UpdateWeaponSelectedUI;

		return ret;
	}

	public override void CleanUpInventoryEvents(InventoryManager playerInventory, InventoryManager bodyInventory)
	{
		defaultSword.manager = bodyInventory;
		playerInventory.OnNewSwordEquipped -= UpdateWeaponSelectedUI;
	}

	#endregion // Public Functions

	#region Private Functions

	private string directionStringForState(Animations state)
	{
		switch (state)
		{
			case Animations.AttackRight:
				return "Right";
			case Animations.AttackUp:
				return "Up";
			case Animations.AttackLeft:
				return "Left";
			case Animations.AttackDown:
				return "Down";
			default:
				return "Right";
		}
	}

	protected override IEnumerator AttackRoutine(Animations state)
	{
		InventoryManager inventory = movement.CurrentController.GetComponent<InventoryManager>();
		weaponData = inventory.equippedSword.data;

		animator.Play(weapon.attackAnimation + directionStringForState(state));
		weapon.gameObject.SetActive(true);
		movement.enabled = false;

		float centralAngle = getAngle(state);
		float minAngle = centralAngle - weapon.attackSweepAngle / 2;
		float maxAngle = centralAngle + weapon.attackSweepAngle / 2;

		float timePassed = 0f;
		while (timePassed < 1f)
		{
			timePassed += Time.deltaTime / weapon.attackDuration;
			float currentAngle = Mathf.Lerp(minAngle, maxAngle, timePassed);
			weapon.transform.parent.localRotation = Quaternion.Euler(0, 0, currentAngle);
			yield return null;
		}

		weapon.gameObject.SetActive(false);
		movement.enabled = true;
		currentState = returnToState;
		if (queuedAction.HasValue)
		{
			PlayAnimation(queuedAction.Value);
			queuedAction = null;
		}
	}

	private void UpdateWeaponSelectedUI(SwordItem sword)
	{
		WeaponSelectionUI.instance.ShowSelectedWeapon(sword);
	}

	#endregion // Private Functions
}
