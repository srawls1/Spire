using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionObstacle : MonoBehaviour
{
    private Bounds2dReference m_bounds;
    new private Collider2D collider;
    private UpdatePolicy updatePolicy;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        Bounds bounds = collider.bounds;
        m_bounds = new Bounds2dReference(bounds.center, bounds.extents);
    }

    private void Update()
    {
        if (updatePolicy.ShouldUpdate())
        {
            Bounds bounds = collider.bounds;
            m_bounds.center = bounds.center;
            m_bounds.extents = bounds.extents;
        }
    }

    public Bounds2dReference Bounds
    {
        get { return m_bounds; }
    }
}
