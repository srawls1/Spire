using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DynamicGridLayout : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private int preferredItemsPerRow;
	[SerializeField] private float horizontalItemSpacing;
	[SerializeField] private float verticalItemSpacing;
	[SerializeField] private int leftPadding;
	[SerializeField] private int rightPadding;
	[SerializeField] private int topPadding;
	[SerializeField] private int bottomPadding;
	[SerializeField] private float minItemWidth;

	#endregion // Editor Fields

	#region Non-Editor Fields

	GridLayoutGroup layout;

	#endregion // Non-Editor Fields

	private void Update()
	{
		// Fuck Unity UI
		layout = GetComponent<GridLayoutGroup>();
		RectTransform rect = transform as RectTransform;
		int itemsPerRow = preferredItemsPerRow;
		float effectiveRowWidth = rect.rect.width -
			(itemsPerRow - 1) * horizontalItemSpacing -
			leftPadding - rightPadding;
		float itemWidth = effectiveRowWidth / itemsPerRow;

		while (itemWidth < minItemWidth && itemsPerRow > 1)
		{
			--itemsPerRow;
			effectiveRowWidth = rect.rect.width -
				(itemsPerRow - 1) * horizontalItemSpacing -
				leftPadding - rightPadding;
			itemWidth = effectiveRowWidth / itemsPerRow;
		}

		layout.cellSize = new Vector2(itemWidth, itemWidth);
		layout.spacing = new Vector2(horizontalItemSpacing, verticalItemSpacing);
		layout.padding = new RectOffset(leftPadding, rightPadding, topPadding, bottomPadding);
	}
}
