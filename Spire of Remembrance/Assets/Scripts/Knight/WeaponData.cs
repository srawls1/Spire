using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Custom/Sword", order = 1)]
public class WeaponData : ScriptableObject
{
	#region Editor Fields

	[SerializeField] private int m_damage;
	[SerializeField] private float m_attackForce;
	[SerializeField] private string m_attackAnimation;
	[SerializeField] private float m_attackSweepAngle;
	[SerializeField] private float m_attackDuration;
	[SerializeField] private Sprite m_weaponSprite;
	[SerializeField] private Color m_trailColor;
	[SerializeField] private string m_name;
	[SerializeField] private string m_description;

	#endregion // Editor Fields

	#region Properties

	public int damage
	{
		get
		{
			return m_damage;
		}
	}

	public float attackforce
	{
		get
		{
			return m_attackForce;
		}
	}

	public string attackAnimation
	{
		get
		{
			return m_attackAnimation;
		}
	}

	public float attackSweepAngle
	{
		get
		{
			return m_attackSweepAngle;
		}
	}

	public float attackDuration
	{
		get
		{
			return m_attackDuration;
		}
	}

	public Sprite weaponSprite
	{
		get
		{
			return m_weaponSprite;
		}
	}

	public Color trailColor
	{
		get
		{
			return m_trailColor;
		}
	}

	public string name
	{
		get
		{
			return m_name;
		}
	}

	public string description
	{
		get
		{
			return m_description;
		}
	}

	#endregion // Properties
}
