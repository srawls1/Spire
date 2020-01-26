using System.Collections.Generic;
using UnityEngine;

#region Node Class

public class WeightMatrixNode<T> : IGraphNode<T>
{
	internal int index
	{
		get; private set;
	}
	public T storedData
	{
		get; private set;
	}

	public WeightMatrixNode(int i, T data)
	{
		index = i;
		storedData = data;
	}

	public override bool Equals(object obj)
	{
		if (obj is WeightMatrixNode<T>)
		{
			WeightMatrixNode<T> node = obj as WeightMatrixNode<T>;
			return index == node.index;
		}

		return false;
	}

	public override int GetHashCode()
	{
		return index;
	}

	public override string ToString()
	{
		return storedData.ToString();
	}
}

#endregion // Node Class

#region Edge Class

public class WeightMatrixEdge<T> : IGraphEdge<T>
{
	private const int prime = 97;

	private WeightMatrixNode<T> m_node1;
	private WeightMatrixNode<T> m_node2;

	internal bool undirected { get; private set; }
	public IGraphNode<T> node1 { get { return m_node1; } }
	public IGraphNode<T> node2 { get { return m_node2; } }
	public float weight { get; private set; }

	public WeightMatrixEdge(WeightMatrixNode<T> n1, WeightMatrixNode<T> n2, float w, bool bidirectional)
	{
		m_node1 = n1;
		m_node2 = n2;
		weight = w;
		undirected = bidirectional;
	}

	public override bool Equals(object obj)
	{
		if (obj is WeightMatrixEdge<T>)
		{
			WeightMatrixEdge<T> edge = obj as WeightMatrixEdge<T>;
			if (node1.Equals(edge.node1) && node2.Equals(edge.node2))
			{
				return true;
			}
			if (undirected && node1.Equals(edge.node2) && node2.Equals(edge.node1))
			{
				return true;
			}
		}

		return false;
	}

	public override int GetHashCode()
	{
		if (undirected)
		{
			return m_node1.index * m_node2.index;
		}
		return m_node1.index * prime + m_node2.index;
	}

	public override string ToString()
	{
		return "<" + node1.ToString() + ',' + node2.ToString() + ">";
	}
}

#endregion // Edge Class

public class WeightMatrixGraph<T> : IGraph<T>
{
	#region Private Variables

	private T[] nodeValues;
	private IDictionary<T, int> nodeLocator;
	private float[,] weights;

	#endregion // Private Variables

	#region Public Properties

	public int EdgeCount
	{
		get; private set;
	}

	public IEnumerable<IGraphEdge<T>> Edges
	{
		get
		{
			for (int i = 0; i < weights.GetLength(0); ++i)
			{
				WeightMatrixNode<T> n1 = new WeightMatrixNode<T>(i, nodeValues[i]);
				for (int j = 0; j < weights.GetLength(1); ++j)
				{
					WeightMatrixNode<T> n2 = new WeightMatrixNode<T>(j, nodeValues[j]);
					yield return new WeightMatrixEdge<T>(n1, n2, weights[i, j], weights[i, j] == weights[j, i]);
				}
			}
		}
	}

	public int NodeCount
	{
		get; private set;
	}

	public IEnumerable<IGraphNode<T>> Nodes
	{
		get
		{
			for (int i = 0; i < NodeCount; ++i)
			{
				yield return new WeightMatrixNode<T>(i, nodeValues[i]);
			}
		}
	}

	#endregion // Public Properties

	#region Constructor

	public WeightMatrixGraph(int maxNumNodes)
	{
		nodeValues = new T[maxNumNodes];
		nodeLocator = new Dictionary<T, int>();
		weights = new float[maxNumNodes, maxNumNodes];
		for (int i = 0; i < maxNumNodes; ++i)
		{
			for (int j = 0; j < maxNumNodes; ++j)
			{
				weights[i, j] = Mathf.Infinity;
			}
		}
	}

	#endregion // Constructor

	#region Public Functions

	public IGraphNode<T> AddNode(T elem)
	{
		Debug.Assert(NodeCount < nodeValues.Length);

		nodeValues[NodeCount] = elem;
		nodeLocator[elem] = NodeCount;
		return new WeightMatrixNode<T>(NodeCount++, elem);
	}

	public IGraphNode<T> GetNode(T elem)
	{
		Debug.Assert(nodeLocator.ContainsKey(elem));

		int i = nodeLocator[elem];
		return new WeightMatrixNode<T>(i, elem);
	}

	public IEnumerable<IGraphEdge<T>> GetEdges(IGraphNode<T> node)
	{
		Debug.Assert(node is WeightMatrixNode<T>);
		WeightMatrixNode<T> n1 = node as WeightMatrixNode<T>;
		int i = n1.index;

		for (int j = 0; j < weights.GetLength(1); ++j)
		{
			if (weights[i, j] < Mathf.Infinity)
			{
				WeightMatrixNode<T> n2 = new WeightMatrixNode<T>(j, nodeValues[j]);
				yield return new WeightMatrixEdge<T>(n1, n2, weights[i, j], weights[i, j] == weights[j, i]);
			}
		}
	}

	public IGraphEdge<T> AddDirectedEdge(IGraphNode<T> node1, IGraphNode<T> node2, float weight = 1)
	{
		Debug.Assert(node1 is WeightMatrixNode<T>);
		Debug.Assert(node2 is WeightMatrixNode<T>);

		WeightMatrixNode<T> n1 = node1 as WeightMatrixNode<T>;
		WeightMatrixNode<T> n2 = node2 as WeightMatrixNode<T>;

		++EdgeCount;
		weights[n1.index, n2.index] = weight;
		return new WeightMatrixEdge<T>(n1, n2, weight, false);
	}

	public IGraphEdge<T> AddUndirectedEdge(IGraphNode<T> node1, IGraphNode<T> node2, float weight = 1)
	{
		Debug.Assert(node1 is WeightMatrixNode<T>);
		Debug.Assert(node2 is WeightMatrixNode<T>);

		WeightMatrixNode<T> n1 = node1 as WeightMatrixNode<T>;
		WeightMatrixNode<T> n2 = node2 as WeightMatrixNode<T>;

		++EdgeCount;
		weights[n1.index, n2.index] = weight;
		weights[n2.index, n1.index] = weight;
		return new WeightMatrixEdge<T>(n1, n2, weight, true);
	}

	public IGraphEdge<T> GetEdge(IGraphNode<T> node1, IGraphNode<T> node2)
	{
		Debug.Assert(node1 is WeightMatrixNode<T>);
		Debug.Assert(node2 is WeightMatrixNode<T>);

		WeightMatrixNode<T> n1 = node1 as WeightMatrixNode<T>;
		WeightMatrixNode<T> n2 = node2 as WeightMatrixNode<T>;

		float weight = weights[n1.index, n2.index];
		if (weight == Mathf.Infinity)
		{
			return null;
		}

		return new WeightMatrixEdge<T>(n1, n2, weight, weight == weights[n2.index, n1.index]);
	}

	#endregion // Public Functions
}
