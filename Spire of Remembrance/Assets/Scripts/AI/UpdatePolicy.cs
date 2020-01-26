using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UpdatePolicy
{
    void Tick(float deltaTime);

    bool ShouldUpdate();
}
