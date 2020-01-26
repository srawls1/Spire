using System.Collections.Generic;

public interface IGraphNode<T>
{
	T storedData { get; }
}

public interface IGraphEdge<T>
{
	IGraphNode<T> node1 { get; }
	IGraphNode<T> node2 { get; }
	float weight { get; }
}

public interface IGraph<T>
{
	IEnumerable<IGraphNode<T>> Nodes { get; }
	int NodeCount { get; }
	IEnumerable<IGraphEdge<T>> Edges { get; }
	int EdgeCount { get; }

	IGraphNode<T> AddNode(T elem);
	IGraphNode<T> GetNode(T elem);
	IEnumerable<IGraphEdge<T>> GetEdges(IGraphNode<T> node);

	IGraphEdge<T> AddDirectedEdge(IGraphNode<T> node1, IGraphNode<T> node2, float weight = 1);
	IGraphEdge<T> AddUndirectedEdge(IGraphNode<T> node1, IGraphNode<T> node2, float weight = 1);
	IGraphEdge<T> GetEdge(IGraphNode<T> node1, IGraphNode<T> node2);
}
