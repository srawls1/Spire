using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BruteForceRectAdjoiner : MonoBehaviour
{
	public void Start()
	{
		IEnumerable<NavRectangle> allRects = NavMesh.instance.rectangles;
		List<NavRectangle> rectList = new List<NavRectangle>(allRects);

		for (int i = 0; i < rectList.Count; ++i)
		{
			rectList[i].ClearAdjoinments();
		}

		for (int i = 0; i < rectList.Count; ++i)
		{
			for (int j = i + 1; j < rectList.Count; ++j)
			{
				if (rectList[i].GetAdjoiningEdge(rectList[j]) != null)
				{
					rectList[i].Adjoin(rectList[j]);
				}
			}
		}

		for (int i = 0; i < rectList.Count; ++i)
		{
			rectList[i].ID = i;
		}

		for (int i = 0; i < rectList.Count; ++i)
		{
			rectList[i].SerializeAdjacents(rectList);
		}
	}
}
