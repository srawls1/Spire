using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds2dReference
{
    private Vector2 m_center;
    public Vector2 center
    {
        get
        {
            return m_center;
        }
        set
        {
            m_center = value;
            x0y0.value = center - extents;
            x0y1.value = new Vector2(center.x - extents.x, center.y + extents.y);
            x1y0.value = new Vector2(center.x + extents.x, center.y - extents.y);
            x1y1.value = center + extents;
        }
    }

    private Vector2 m_extents;
    public Vector2 extents
    {
        get
        {
            return m_extents;
        }
        set
        {
            m_extents = value;
            x0y0.value = center - extents;
            x0y1.value = new Vector2(center.x - extents.x, center.y + extents.y);
            x1y0.value = new Vector2(center.x + extents.x, center.y - extents.y);
            x1y1.value = center + extents;
        }
    }

    public Vector2Reference x0y0 { get; private set; }
    public Vector2Reference x0y1 { get; private set; }
    public Vector2Reference x1y0 { get; private set; }
    public Vector2Reference x1y1 { get; private set; }

    public Bounds2dReference(Vector2 center, Vector2 extents)
    {
        x0y0 = Vector2.zero;
        x0y1 = Vector2.zero;
        x1y0 = Vector2.zero;
        x1y1 = Vector2.zero;
        this.center = center;
        this.extents = extents;
    }
}

public class Vector2Reference
{
    private Vector2 m_value;
    public Vector2 value
    {
        get { return m_value; }
        set
        {
            m_value = value;
            if (OnValueChanged != null)
            {
                OnValueChanged();
            }
        }
    }

    public Vector2Reference(Vector2 val)
    {
        value = val;
    }

    public event Action OnValueChanged;

    public static implicit operator Vector2(Vector2Reference reference) => reference.value;
    public static implicit operator Vector2Reference(Vector2 value) => new Vector2Reference(value);
}
