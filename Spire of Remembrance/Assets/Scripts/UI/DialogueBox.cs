using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
	private static DialogueBox m_instance;
	public static DialogueBox instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<DialogueBox>();
			}

			return m_instance;
		}
	}

	[SerializeField, Range(0f, 1f)] private float textSpeed;

	private Text text;

	private void Awake()
	{
		text = GetComponentInChildren<Text>();
		m_instance = this;
		gameObject.SetActive(false);
	}

	public Coroutine GoThroughDialogue(string[] lines)
	{
		gameObject.SetActive(true);
		return StartCoroutine(DialogueRoutine(lines));
	}

	private IEnumerator DialogueRoutine(string[] lines)
	{
		for (int i = 0; i < lines.Length; ++i)
		{
			yield return StartCoroutine(WriteOutLine(lines[i]));
			yield return StartCoroutine(WaitForInput());
		}
		gameObject.SetActive(false);
	}

	private IEnumerator WriteOutLine(string line)
	{
		for (int i = 1; i <= line.Length; ++i)
		{
			text.text = line.Substring(0, i);
			float timeToWait = (1f - textSpeed) * .1f;
			for (float timePassed = 0f; timePassed < timeToWait; timePassed += Time.unscaledDeltaTime)
			{
				yield return null;
				if (Input.GetButtonDown("Submit"))
				{
					i = line.Length - 1;
				}
			}
		}
	}

	private IEnumerator WaitForInput()
	{
		while (true)
		{
			yield return null;
			if (Input.GetButtonDown("Submit"))
			{
				break;
			}
		}
	}
}
