using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelDoor : Interactable
{
	[SerializeField] private string nextSceneName;
	[SerializeField] private string actionName = "Ascend";

	public override Interaction[] interactions
	{
		get
		{
			return new Interaction[]
			{
				new Interaction(actionName, LoadNextLevel, true)
			};
		}
	}

	private void LoadNextLevel()
	{
		// TODO - save the game
		SceneManager.LoadScene(nextSceneName);
	}
}
