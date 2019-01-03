using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseListener : MonoBehaviour
{
	[SerializeField] private GameObject menuCanvas;

	void Update()
	{
		if (Input.GetButtonDown("Pause"))
		{
			if (menuCanvas.activeSelf)
			{
				Unpause();
			}
			else
			{
				Pause();
			}
		}
	}

	public void Pause()
	{
		Time.timeScale = 0f;
		menuCanvas.SetActive(true);
	}

	public void Unpause()
	{
		Time.timeScale = 1f;
		menuCanvas.SetActive(false);
	}
}
