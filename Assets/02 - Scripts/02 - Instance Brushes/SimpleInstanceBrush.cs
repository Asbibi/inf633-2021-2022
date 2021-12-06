using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInstanceBrush : InstanceGridBrush
{

    public override void draw(float x, float z)
    {
        snapToGrid(ref x, ref z);

        spawnObject(x, z);
        spawnObject(x - radius, z - radius);
        spawnObject(x - radius, z + radius);
        spawnObject(x + radius, z - radius);
        spawnObject(x + radius, z + radius);
    }
}
