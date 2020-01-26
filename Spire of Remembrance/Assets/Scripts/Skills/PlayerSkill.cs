using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill : ScriptableObject
{
	public abstract void OnLearned(CharacterSkillSet skillSet);

	[SerializeField] private PlayerSkill[] m_prerequisites;

	public PlayerSkill[] prerequisites
	{
		get { return m_prerequisites; }
	}
}
