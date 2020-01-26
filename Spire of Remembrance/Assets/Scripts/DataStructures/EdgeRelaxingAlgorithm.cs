using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EdgeRelaxingAlgorithm<T>
{
	protected class QueueType : ComparablePair<float, IGraphNode<T>>, IComparable<QueueType>
	{
		public QueueType(float first, IGraphNode<T> second)
			: base(first, second) { }

		public int CompareTo(QueueType other) { return base.CompareTo(other); }
	}

	protected IDictionary<IGraphNode<T>, IGraphEdge<T>> RelaxEdges(IGraph<T> graph, IGraphNode<T> node, float initialCost = 0)
	{
		PriorityQueue<QueueType> openNodes = new PriorityQueue<QueueType>(graph.NodeCount);
		IDictionary<IGraphNode<T>, QueueType> priorityQueueLocator = new Dictionary<IGraphNode<T>, QueueType>();
		IDictionary<IGraphNode<T>, IGraphEdge<T>> parents = new Dictionary<IGraphNode<T>, IGraphEdge<T>>();
		HashSet<IGraphNode<T>> closedNodes = new HashSet<IGraphNode<T>>();

		QueueType nodePair = new QueueType(initialCost, node);
		openNodes.Add(nodePair);
		priorityQueueLocator[node] = nodePair;

		while (openNodes.Count > 0)
		{
			nodePair = openNodes.Pop();
			float baseCost = nodePair.First;
			node = nodePair.Second;

			if (ShouldTerminate(node))
			{
				break;
			}

			foreach (IGraphEdge<T> edge in graph.GetEdges(node))
			{
				if (closedNodes.Contains(edge.node2))
				{
					continue;
				}

				float cost = GetCost(baseCost, edge);
				IGraphEdge<T> currentEdge;
				if (!parents.TryGetValue(edge.node2, out currentEdge) ||
					cost < currentEdge.weight)
				{
					parents[edge.node2] = edge;
					nodePair = new QueueType(cost, edge.node2);
					QueueType oldPair;
					if (priorityQueueLocator.TryGetValue(edge.node2, out oldPair)
						&& openNodes.Contains(oldPair))
					{
						openNodes.UpdatePosition(oldPair, nodePair);
					}
					else
					{
						openNodes.Add(nodePair);
					}
					priorityQueueLocator[edge.node2] = nodePair;
				}
			}

			closedNodes.Add(node);
		}

		return parents;
	}

	protected abstract float GetCost(float prevNodeCost, IGraphEdge<T> edge);

	protected virtual bool ShouldTerminate(IGraphNode<T> node)
	{
		return false;
	}
}
