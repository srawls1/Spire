using System.Collections.Generic;

public class AStarAlgorithm<T> : EdgeRelaxingAlgorithm<T>, IShortestPathStrategy<T>
{
	public delegate float Heuristic(T step, T dest);
	private Heuristic m_h;
	private IGraphNode<T> destination;

	public AStarAlgorithm(Heuristic h)
	{
		m_h = h;
	}

	public IList<IGraphEdge<T>> ShortestPath(IGraph<T> graph, T start, T end)
	{
		IGraphNode<T> startNode = graph.GetNode(start);
		IGraphNode<T> endNode = graph.GetNode(end);
		destination = endNode;

		IDictionary<IGraphNode<T>, IGraphEdge<T>> parents = RelaxEdges(graph, startNode, m_h(start, end));
		IList<IGraphEdge<T>> path = new List<IGraphEdge<T>>();
		IGraphNode<T> node = endNode;
		while (parents.ContainsKey(node))
		{
			IGraphEdge<T> edge = parents[node];
			path.Add(edge);
			node = edge.node1;
		}

		for (int i = 0; i < path.Count / 2; ++i)
		{
			IGraphEdge<T> temp = path[i];
			path[i] = path[path.Count - i - 1];
			path[path.Count - i - 1] = temp;
		}

		return path;
	}

	protected override float GetCost(float prevNodeCost, IGraphEdge<T> edge)
	{
		return prevNodeCost + edge.weight + m_h(edge.node2.storedData, destination.storedData);
	}

	protected override bool ShouldTerminate(IGraphNode<T> node)
	{
		return node == destination;
	}
}
