using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : Movement
{
	#region Editor Fields

	[SerializeField] private WeaponData m_weaponData;

	#endregion // Editor Fields

	#region Non-Editor Fields

	private Weapon m_weapon;

	#endregion // Non-Editor Fields

	#region Properties

	public Weapon weapon
	{
		get
		{
			return m_weapon;
		}
	}

	public WeaponData weaponData
	{
		get
		{
			return m_weaponData;
		}
		set
		{
			weapon.data = value;
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Start()
	{
		m_weapon = GetComponentInChildren<Weapon>();
		m_weapon.gameObject.SetActive(false);
		weaponData = weaponData;
	}

	#endregion // Unity Functions

	#region Override Functions

	protected override int getInteractionLayermask()
	{
		return ~LayerMask.GetMask(new string[]
		{
			"Spirit"
		});
	}

	#endregion // Override Functions
}
