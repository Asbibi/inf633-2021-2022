using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetInstanceBrush : InstanceBrush
{
    public bool allObjects = false;

    public override void draw(float x, float z)
    {
        if (allObjects)
            terrain.removeAllObjects();

        else
            removeObjects(x, z);
    }
}
