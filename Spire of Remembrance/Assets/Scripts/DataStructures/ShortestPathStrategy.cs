using System.Collections.Generic;

public interface IShortestPathStrategy<T>
{
	IList<IGraphEdge<T>> ShortestPath(IGraph<T> graph, T start, T end);
}
