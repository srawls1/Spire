using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
	private static InGameUIManager m_instance;
	public static InGameUIManager instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<InGameUIManager>();
			}
			return m_instance;
		}
	}

	private void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
		else if (m_instance != this)
		{
			Destroy(this);
		}
	}

	[SerializeField] private CounterUI keyCounter;
	[SerializeField] private CounterUI spiritOrbCounter;

	public void SetNumKeys(int numKeys)
	{
		keyCounter.SetCount(numKeys);
	}

	public void SetNumSpiritOrbs(int numOrbs)
	{
		spiritOrbCounter.SetCount(numOrbs);
	}
}
