using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetInstanceBrush : InstanceBrush
{
    public bool allObjects = false;

    public override void draw(float x, float z)
    {
        if (allObjects)
        {
            terrain.removeAllObjects();
            return;
        }

        // Only remove the trees in the brush area
        int instanceCount = terrain.getObjectCount();
        List<int> instanceToRemove = new List<int>();
        for (int i = 0; i < instanceCount; i++)
        {
            Vector3 terrainDimension = terrain.terrainSize();
            Vector3 instancePosition = terrain.getObject(i).position;

            instancePosition.x = (instancePosition.x * terrainDimension.x) - x;
            instancePosition.z = (instancePosition.z * terrainDimension.z) - z;
            if (Mathf.Abs(instancePosition.x) < radius && Mathf.Abs(instancePosition.z) < radius)
                instanceToRemove.Add(i);
        }
        terrain.removeObjects(instanceToRemove);
    }
}
