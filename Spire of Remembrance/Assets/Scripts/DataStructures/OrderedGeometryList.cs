using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexData
{
    public Vector2Reference position;
    public int index;
    public float angle;
    public VertexData prevVert;
    public VertexData nextVert;
    public EdgeData nearestEdge;

    public VertexData(Vector2Reference pos, int n)
    {
        position = pos;
        index = n;
    }
}

public class EdgeData
{
    public VertexData v1;
    public VertexData v2;

    public EdgeData(VertexData v1, VertexData v2)
    {
        this.v1 = v1;
        this.v2 = v2;
    }

    public override bool Equals(object other)
    {
        if (!(other is EdgeData))
        {
            return false;
        }

        EdgeData edge = other as EdgeData;
        return (v1.Equals(edge.v1) && v2.Equals(edge.v2)) ||
            (v1.Equals(edge.v2) && v2.Equals(edge.v1));
    }

    public override int GetHashCode()
    {
        return v1.GetHashCode() * v2.GetHashCode();
    }
}

public class OrderedGeometryList
{
    private List<VertexData> vertexData;

    public OrderedGeometryList(int capacity)
    {
        vertexData = new List<VertexData>(capacity * 4);
    }

    public void AddGeometry(Bounds2dReference bounds)
    {
        bounds.x0y0.OnValueChanged += SetDirty;
        bounds.x0y1.OnValueChanged += SetDirty;
        bounds.x1y0.OnValueChanged += SetDirty;
        bounds.x1y1.OnValueChanged += SetDirty;
        VertexData x0y0 = new VertexData(bounds.x0y0, vertexData.Count);
        VertexData x0y1 = new VertexData(bounds.x0y1, vertexData.Count + 1);
        VertexData x1y0 = new VertexData(bounds.x1y0, vertexData.Count + 2);
        VertexData x1y1 = new VertexData(bounds.x1y1, vertexData.Count + 3);
        x0y0.prevVert = x0y1;
        x0y0.nextVert = x1y0;
        x0y1.prevVert = x1y1;
        x0y1.nextVert = x0y0;
        x1y1.prevVert = x1y0;
        x1y1.nextVert = x0y1;
        x1y0.prevVert = x0y0;
        x1y0.nextVert = x1y1;
        vertexData.Add(x0y0);
        vertexData.Add(x0y1);
        vertexData.Add(x1y0);
        vertexData.Add(x1y1);
    }

    public void RemoveGeometry(Bounds2dReference bounds)
    {
        int n1 = vertexData.FindIndex(vert => vert.position == bounds.x0y0);
        int n2 = vertexData.FindIndex(vert => vert.position == bounds.x0y1);
        int n3 = vertexData.FindIndex(vert => vert.position == bounds.x1y0);
        int n4 = vertexData.FindIndex(vert => vert.position == bounds.x1y1);

        if (n1 >= 0 && n2 >= 0 && n3 >= 0 && n4 >= 0)
        {
            vertexData[n1].position.OnValueChanged -= SetDirty;
            vertexData[n2].position.OnValueChanged -= SetDirty;
            vertexData[n3].position.OnValueChanged -= SetDirty;
            vertexData[n4].position.OnValueChanged -= SetDirty;
            vertexData.RemoveAt(n1);
            vertexData.RemoveAt(n2);
            vertexData.RemoveAt(n3);
            vertexData.RemoveAt(n4);
        }
    }

    public int vertexCount
    {
        get { return vertexData.Count; }
    }

    public Vector2 this[int index]
    {
        get { return vertexData[index].position; }
    }

    public bool isDirty
    {
        get; private set;
    }

    public float GetAngle(int index)
    {
        return vertexData[index].angle;
    }

    public Vector2 GetClosestPointBefore(Vector2 center, int index, float angle, float maxDistance)
    {
        EdgeData edge = vertexData[index].nearestEdge;
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        if (edge == null)
        {
            return center + maxDistance * new Vector2(cos, sin);
        }

        Vector2 dif1 = edge.v1.position - center;
        Vector2 dif2 = edge.v1.position.value - edge.v2.position.value;
        float det = dif1.x * sin - dif1.y * cos;
        // Cramer's rule, bitch!
        float s = (dif1.x * dif2.y - dif1.y * dif2.x) / det;
        return center + s * new Vector2(cos, sin);
    }

    public Vector2 GetClosestPointAfter(Vector2 center, int index, float angle, float maxDistance)
    {
        return GetClosestPointBefore(center, (index + 1) % vertexData.Count, angle, maxDistance);
    }

    public void PopulateAdjacentVertices(int vertexIndex, out int vert1, out int vert2)
    {
        vert1 = vertexData[vertexIndex].prevVert.index;
        vert2 = vertexData[vertexIndex].nextVert.index;
    }

    public void AngularSort(Vector2 center)
    {
        // Insertion sort works well here because we are doing this often,
        // so it always starts out mostly in order
        for (int i = 0; i < vertexData.Count; ++i)
        {
            VertexData vertex = vertexData[i];
            Vector2 diff = vertex.position - center;
            vertex.angle = Mathf.Atan2(diff.y, diff.x);

            int j;
            for (j = i; j > 0; --j)
            {
                if (vertexData[j - 1].angle < vertex.angle)
                {
                    break;
                }

                vertexData[j] = vertexData[j - 1];
                vertexData[j].index = j;
            }

            vertexData[j] = vertex;
            vertexData[j].index = j;
        }
        NearestEdgeSweep(center);

        isDirty = false;
    }

    private void NearestEdgeSweep(Vector2 center)
    {
        PriorityQueue<ComparablePair<float, EdgeData>> closestEdges =
            new PriorityQueue<ComparablePair<float, EdgeData>>();
        Dictionary<EdgeData, ComparablePair<float, EdgeData>> queueEntries =
            new Dictionary<EdgeData, ComparablePair<float, EdgeData>>();
        for (int round = 0; round < 2; ++round)
        {
            for (int i = 0; i < vertexData.Count; ++i)
            {
                vertexData[i].nearestEdge = closestEdges.Peek().Second;

                EdgeData edge1 = new EdgeData(vertexData[i], vertexData[i].prevVert);
                ProcessEdge(edge1, closestEdges, queueEntries, center);

                EdgeData edge2 = new EdgeData(vertexData[i], vertexData[i].nextVert);
                ProcessEdge(edge2, closestEdges, queueEntries, center);
            }
        }
    }

    private void ProcessEdge(EdgeData edge, PriorityQueue<ComparablePair<float, EdgeData>> closestEdges,
        Dictionary<EdgeData, ComparablePair<float, EdgeData>> queueEntries, Vector2 center)
    {
        Vector2 edgeCenter = (edge.v1.position.value + edge.v2.position.value) / 2;
        float angle1 = NormalizeAngle(edge.v1, edge.v1);
        float angle2 = NormalizeAngle(edge.v1, edge.v2);
        if (edge.v1.angle < edge.v2.angle)
        {
            float distance = Vector2.Distance(center, edgeCenter);
            ComparablePair<float, EdgeData> pair = new ComparablePair<float, EdgeData>(distance, edge);
            closestEdges.Add(pair);
            queueEntries.Add(edge, pair);
        }
        else
        {
            ComparablePair<float, EdgeData> pair = queueEntries[edge];
            closestEdges.Remove(pair);
            queueEntries.Remove(edge);
        }
    }

    private float NormalizeAngle(VertexData reference, VertexData vertex)
    {
        if (reference.angle > 0f && reference.angle < 90f && vertex.angle > 270f)
        {
            return vertex.angle - 360f;
        }

        return vertex.angle;
    }

    public int GetVertexIndex(float angle)
    {
        // Do a binary search to get the closest vertex *after* the given angle
        int start = 0;
        int end = vertexCount;

        while (start < end)
        {
            int mid = (start + end) / 2;
            if (angle < GetAngle(mid))
            {
                end = mid;
            }
            else
            {
                start = mid + 1;
            }
        }

        return start % vertexCount;
    }

    private void SetDirty()
    {
        isDirty = true;
    }
}
