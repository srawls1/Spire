using System.Diagnostics;
using System.Collections.Generic;

#region Node Class

public class AdjListGraphNode<T> : IGraphNode<T>
{
	public T storedData { get; private set; }
	internal List<AdjListGraphEdge<T>> edges;

	public AdjListGraphNode(T data)
	{
		storedData = data;
		edges = new List<AdjListGraphEdge<T>>();
	}

	public override bool Equals(object obj)
	{
		if (obj is AdjListGraphNode<T>)
		{
			AdjListGraphNode<T> node = obj as AdjListGraphNode<T>;
			return storedData.Equals(node.storedData);
		}
		else
		{
			return false;
		}
	}

	public override int GetHashCode()
	{
		return storedData.GetHashCode();
	}

	public override string ToString()
	{
		return storedData.ToString();
	}
}

#endregion // Node Class

#region Edge Class

public class AdjListGraphEdge<T> : IGraphEdge<T>
{
    private const int prime = 97;

	private AdjListGraphNode<T> m_node1;
	private AdjListGraphNode<T> m_node2;

	public bool undirected { get; private set; }
    public IGraphNode<T> node1 { get { return m_node1; } }
    public IGraphNode<T> node2 { get { return m_node2; } }
    public float weight { get; private set; }

    public AdjListGraphEdge(AdjListGraphNode<T> n1, AdjListGraphNode<T> n2, float w, bool directionless)
    {
		undirected = directionless;
        m_node1 = n1;
        m_node2 = n2;
        weight = w;
    }

    public override bool Equals(object obj)
    {
        if (obj is AdjListGraphEdge<T>)
        {
            AdjListGraphEdge<T> edge = obj as AdjListGraphEdge<T>;
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
			return node1.GetHashCode() * node2.GetHashCode();
		}

        return node1.GetHashCode() * prime + node2.GetHashCode();
    }

	public override string ToString()
	{
		return "<" + node1.ToString() + ',' + node2.ToString() + ">";
	}
}

#endregion // Edge Class

public class AdjacencyListGraph<T> : IGraph<T>
{
	#region Private Variables

	IDictionary<AdjListGraphNode<T>, AdjListGraphNode<T>> nodes;
	ICollection<AdjListGraphEdge<T>> edges;

	#endregion Private Variables

	#region Public Properties

	public IEnumerable<IGraphNode<T>> Nodes
	{
		get
		{
			foreach (AdjListGraphNode<T> node in nodes.Keys)
			{
				yield return node;
			}
		}
	}

	public int NodeCount
	{
		get { return nodes.Count; }
	}

	public IEnumerable<IGraphEdge<T>> Edges
	{
		get
		{
			foreach (AdjListGraphEdge<T> edge in edges)
			{
				yield return edge;
			}
		}
	}

	public int EdgeCount
	{
		get { return edges.Count; }
	}

	#endregion // public Properties

	#region Constructor

	public AdjacencyListGraph()
    {
        nodes = new Dictionary<AdjListGraphNode<T>, AdjListGraphNode<T>>();
		edges = new List<AdjListGraphEdge<T>>();
    }

	#endregion // Constructor

	#region Public Functions

	public IGraphNode<T> AddNode(T elem)
	{
		AdjListGraphNode<T> node = new AdjListGraphNode<T>(elem);
		nodes.Add(node, node);
		return node;
	}

	public IGraphNode<T> GetNode(T elem)
	{
		AdjListGraphNode<T> node = new AdjListGraphNode<T>(elem);
		if (nodes.TryGetValue(node, out node))
		{
			return node;
		}
		else
		{
			return null;
		}
	}

	public IEnumerable<IGraphEdge<T>> GetEdges(IGraphNode<T> node)
	{
		Debug.Assert(node is AdjListGraphNode<T>);

		AdjListGraphNode<T> n = node as AdjListGraphNode<T>;
		foreach (AdjListGraphEdge<T> edge in n.edges)
		{
			yield return edge;
		}
	}


	public IGraphEdge<T> AddDirectedEdge(IGraphNode<T> n1, IGraphNode<T> n2, float weight = 1)
	{
		Debug.Assert(n1 is AdjListGraphNode<T>);
		Debug.Assert(n2 is AdjListGraphNode<T>);
		AdjListGraphNode<T> node1 = n1 as AdjListGraphNode<T>;
		AdjListGraphNode<T> node2 = n2 as AdjListGraphNode<T>;

		Debug.Assert(nodes.ContainsKey(node1));
		Debug.Assert(nodes.ContainsKey(node2));

		AdjListGraphEdge<T> edge = new AdjListGraphEdge<T>(node1, node2, weight, false);
		edges.Add(edge);
		node1.edges.Add(edge);
		return edge;
	}

	public IGraphEdge<T> AddUndirectedEdge(IGraphNode<T> n1, IGraphNode<T> n2, float weight = 1)
	{
		Debug.Assert(n1 is AdjListGraphNode<T>);
		Debug.Assert(n2 is AdjListGraphNode<T>);
		AdjListGraphNode<T> node1 = n1 as AdjListGraphNode<T>;
		AdjListGraphNode<T> node2 = n2 as AdjListGraphNode<T>;

		Debug.Assert(nodes.ContainsKey(node1));
		Debug.Assert(nodes.ContainsKey(node2));

		AdjListGraphEdge<T> edge = new AdjListGraphEdge<T>(node1, node2, weight, true);

		node1.edges.Add(edge);
		node2.edges.Add(new AdjListGraphEdge<T>(node2, node1, weight, true));
		edges.Add(edge);
		return edge;
	}

	public IGraphEdge<T> GetEdge(IGraphNode<T> n1, IGraphNode<T> n2)
	{
		Debug.Assert(n1 is AdjListGraphNode<T>);
		Debug.Assert(n2 is AdjListGraphNode<T>);
		AdjListGraphNode<T> node1 = n1 as AdjListGraphNode<T>;
		AdjListGraphNode<T> node2 = n2 as AdjListGraphNode<T>;

		Debug.Assert(nodes.ContainsKey(node1));
		Debug.Assert(nodes.ContainsKey(node2));

		return node1.edges.Find((edge) => edge.node2.Equals(node2));
	}

	#endregion // Public Functions
}
