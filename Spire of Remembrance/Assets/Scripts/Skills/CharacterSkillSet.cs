using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInput
{
	public string button { get; private set; }
	public bool hold { get; private set; }
	public bool directionRequired { get; private set; }

	public SkillInput(string b, bool h, bool d)
	{
		button = b;
		hold = h;
		directionRequired = d;
	}

	public SkillInput(string b, bool h, Vector2 d)
	: this(b, h, d.magnitude > 0.3f)
	{}

	public override int GetHashCode()
	{
		return button.GetHashCode();
	}

	public override bool Equals(object other)
	{
		SkillInput input = other as SkillInput;
		if (input == null)
		{
			return false;
		}

		return button.Equals(input.button) &&
			hold == input.hold &&
			directionRequired == directionRequired;
	}
}

public class SkillAction
{
	private Func<bool> canUseNow;
	private Action execute;

	public SkillAction(Func<bool> usable, Action use)
	{
		canUseNow = usable;
		execute = use;
	}

	public bool CanUseNow()
	{
		return canUseNow();
	}

	public void Execute()
	{
		execute();
	}
}

public class CharacterSkillSet : MonoBehaviour
{
	[SerializeField] private PlayerSkill[] m_allSkills;

	private HashSet<PlayerSkill> learnedSkills;
	private Dictionary<SkillInput, SkillAction> inputsToActions;

	public PlayerSkill[] allSkills
	{
		get { return m_allSkills; }
	}

	public void LearnSkill(PlayerSkill skill)
	{
		skill.OnLearned(this);
		learnedSkills.Add(skill);
	}

	public bool HasSkill(PlayerSkill skill)
	{
		return learnedSkills.Contains(skill);
	}

	public void AddInput(SkillInput input, SkillAction action)
	{
		inputsToActions.Add(input, action);
	}

	public bool HasInput(string button, bool holding, Vector2 dir)
	{
		SkillInput input = new SkillInput(button, holding, dir);
		SkillAction action;
		return inputsToActions.TryGetValue(input, out action) &&
			action.CanUseNow();
	}

	public void ExecuteInput(string button, bool holding, Vector2 dir)
	{
		SkillInput input = new SkillInput(button, holding, dir);
		SkillAction action;
		if (inputsToActions.TryGetValue(input, out action) &&
			action.CanUseNow())
		{
			action.Execute();
		}
	}
}
