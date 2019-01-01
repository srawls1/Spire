using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TabRow : MonoBehaviour
{
	#region Non-Editor Fields

	private Tab[] tabs;
	private int m_activeTabIndex;

	#endregion // Non-Editor Fields

	#region Properties

	public int activeTabIndex
	{
		get
		{
			return m_activeTabIndex;
		}
		set
		{
			if (m_activeTabIndex >= 0 && m_activeTabIndex < tabs.Length)
			{
				tabs[m_activeTabIndex].SetInactive();
			}
			m_activeTabIndex = value;
			if (m_activeTabIndex >= 0 && m_activeTabIndex < tabs.Length)
			{
				tabs[m_activeTabIndex].SetActive();
			}
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Awake()
	{
		tabs = GetComponentsInChildren<Tab>();
	}

	private void Start()
	{
		for (int i = 0; i < tabs.Length; ++i)
		{
			tabs[i].SetInactive();
		}
		activeTabIndex = 0;
	}

	private void Update()
	{
		RectTransform trans = transform as RectTransform;
		float tabWidth = trans.rect.width / tabs.Length;
		float tabHeight = trans.rect.height;
		for (int i = 0; i < tabs.Length; ++i)
		{
			RectTransform rect = tabs[i].transform as RectTransform;
			rect.anchorMin = new Vector2(0f, 0f);
			rect.anchorMax = new Vector2(0f, 1f);
			rect.sizeDelta = new Vector2(tabWidth, 0f);
			rect.anchoredPosition = new Vector2(tabWidth * (i + 0.5f), 0f);
		}
	}

	private void OnDisable()
	{
		activeTabIndex = 0;
	}

	#endregion // Unity Functions
}
