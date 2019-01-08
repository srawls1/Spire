using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScreen : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private Image mapImage;
	[SerializeField] private float zoomStep;
	[SerializeField] private float maxZoom;
	[SerializeField] private float minZoom;
	[SerializeField] private Color blankColor;

	#endregion // Editor Fields

	#region Properties

	public Sprite mapSprite
	{
		get
		{
			return mapImage.sprite;
		}
		set
		{
			mapImage.sprite = value;
			if (value == null)
			{
				mapImage.color = blankColor;
			}
			else
			{
				mapImage.preserveAspect = true;
				mapImage.color = Color.white;
			}
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Start()
	{
		mapSprite = mapSprite;
	}

	private void OnEnable()
	{
		mapImage.transform.localScale = Vector3.one;
	}

	#endregion // Unity Function

	#region Public Functions

	public void ZoomIn()
	{
		float zoom = Mathf.Min(mapImage.transform.localScale.x + zoomStep, maxZoom);
		mapImage.transform.localScale = Vector3.one * zoom;
	}

	public void ZoomOut()
	{
		float zoom = Mathf.Max(mapImage.transform.localScale.y - zoomStep, minZoom);
		mapImage.transform.localScale = Vector3.one * zoom;
	}

	#endregion // Public Functions
}
