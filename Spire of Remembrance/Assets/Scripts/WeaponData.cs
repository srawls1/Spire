using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Custom/Weapon", order = 1)]
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

	#endregion // Properties
}
