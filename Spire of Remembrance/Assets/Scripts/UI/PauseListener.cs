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
				Time.timeScale = 1f;
				menuCanvas.SetActive(false);
			}
			else
			{
				Time.timeScale = 0f;
				menuCanvas.SetActive(true);
			}
		}
	}
}
