using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstrasAlgorithm<T> : EdgeRelaxingAlgorithm<T>, IShortestPathStrategy<T>
{
	private IGraph<T> m_graph;
	private T m_start;
	private IDictionary<IGraphNode<T>, IGraphEdge<T>> parentsInPath;

	public DijkstrasAlgorithm(IGraph<T> graph, T start)
	{
		m_graph = graph;
		m_start = start;
		IGraphNode<T> node = graph.GetNode(start);
		if (node == null)
		{
			return;
		}

		parentsInPath = RelaxEdges(graph, node);
	}

	public IList<IGraphEdge<T>> ShortestPath(IGraph<T> graph, T start, T end)
	{
		Debug.Assert(m_graph == graph);
		Debug.Assert(m_start.Equals(start));

		IGraphNode<T> node = graph.GetNode(end);

		IList<IGraphEdge<T>> ret = new List<IGraphEdge<T>>();
		if (!parentsInPath.ContainsKey(node))
		{
			return ret;
		}

		while (!node.storedData.Equals(start))
		{
			IGraphEdge<T> edge = parentsInPath[node];
			ret.Add(edge);
			node = node == edge.node2 ? edge.node1 : edge.node2;
		}

		for (int i = 0; i < ret.Count / 2; ++i)
		{
			IGraphEdge<T> temp = ret[i];
			ret[i] = ret[ret.Count - i - 1];
			ret[ret.Count - i - 1] = temp;
		}

		return ret;
	}

	protected override float GetCost(float prevNodeCost, IGraphEdge<T> edge)
	{
		return prevNodeCost + edge.weight;
	}
}
