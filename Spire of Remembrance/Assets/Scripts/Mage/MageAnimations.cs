using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAnimations : EntityAnimations
{
	#region Editor Fields

	[SerializeField] private bool isArcher;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private ProjectileShooter weapon;
	private ShooterItem defaultShooter;

	#endregion // Non-Editor Fields

	#region Properties

	public ProjectileShooterData shooterData
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

	new private void Awake()
	{
		base.Awake();
		weapon = GetComponentInChildren<ProjectileShooter>();
	}

	private void Start()
	{
		weapon.gameObject.SetActive(false);
	}

	#endregion // Unity Functions

	#region Overrides

	public override List<InventoryItem> UpdateInventoryWeapons(InventoryManager playerInventory, InventoryManager bodyInventory)
	{
		List<ShooterItem> playerShooters = isArcher ? playerInventory.GetAllBows() : playerInventory.GetAllStaves();
		List<InventoryItem> ret = new List<InventoryItem>();
		defaultShooter = isArcher ? bodyInventory.defaultBow : bodyInventory.defaultStaff;
		defaultShooter.manager = playerInventory;
		ret.Add(defaultShooter);
		for (int i = 0; i < playerShooters.Count; ++i)
		{
			ret.Add(playerShooters[i]);
		}

		WeaponSelectionUI.instance.SetAvailableWeapons(ret);
		UpdateWeaponSelectedUI(isArcher ? playerInventory.equippedBow : playerInventory.equippedStaff);

		if (isArcher)
		{
			playerInventory.OnNewBowEquipped += UpdateWeaponSelectedUI;
		}
		else
		{
			playerInventory.OnNewStaffEquipped += UpdateWeaponSelectedUI;
		}

		return ret;
	}

	public override void CleanUpInventoryEvents(InventoryManager playerInventory, InventoryManager bodyInventory)
	{
		defaultShooter.manager = bodyInventory;
		if (isArcher)
		{
			playerInventory.OnNewBowEquipped -= UpdateWeaponSelectedUI;
		}
		else
		{
			playerInventory.OnNewStaffEquipped -= UpdateWeaponSelectedUI;
		}
	}

	protected override IEnumerator AttackRoutine(Animations state)
	{
		InventoryManager inventory = movement.CurrentController.GetComponent<InventoryManager>();
		shooterData = isArcher ? inventory.equippedBow.weaponData : inventory.equippedStaff.weaponData;

		animator.Play(state.ToString());
		weapon.transform.parent.localRotation = Quaternion.Euler(0f, 0f, getAngle(state));
		weapon.gameObject.SetActive(true);
		movement.enabled = false;

		yield return new WaitForSeconds(weapon.releaseTime);

		weapon.SpawnProjectile().instigator = gameObject;

		yield return new WaitForSeconds(weapon.attackDuration - weapon.releaseTime);

		weapon.gameObject.SetActive(false);
		movement.enabled = true;
		currentState = returnToState;
		if (queuedAction.HasValue)
		{
			PlayAnimation(queuedAction.Value);
			queuedAction = null;
		}
	}

	private void UpdateWeaponSelectedUI(ShooterItem item)
	{
		WeaponSelectionUI.instance.ShowSelectedWeapon(item);
	}

	#endregion // Overrides
}
