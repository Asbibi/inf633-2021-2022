using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstanceAddBrush : InstanceGridBrush
{
    public bool removePreviousObjectInArea = false;

    protected void smartSpwanObject(float x, float z)
    {
        if (removePreviousObjectInArea)
            removeObjects(x, z);

        spawnObject(x, z);
    }
}
