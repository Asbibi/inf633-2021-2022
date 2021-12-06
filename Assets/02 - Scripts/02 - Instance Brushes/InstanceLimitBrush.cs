using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstanceLimitBrush : InstanceBrush
{
    [Header("Limits")]
    public bool limitHeight = false;
    public float heightMax = 20;
    public bool limitSteepness = false;
    public float steepnessMaxAngle = 70;

    public override void spawnObject(float x, float z)
    {
        if (limitHeight && terrain.get(x, z) > heightMax)
            return;

        if (limitSteepness)
        {
            float maxLocalY = terrain.get(x, z);
            float minLocalY = maxLocalY;
            Vector2 maxCoord = new Vector2(x, z);
            Vector2 minCoord = new Vector2(x, z);
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    float h = terrain.get(x + i, z + j);
                    if (h < minLocalY)
                    {
                        minLocalY = h;
                        minCoord = new Vector2(x + i, z + j);
                    }
                    if (h > maxLocalY)
                    {
                        maxLocalY = h;
                        maxCoord = new Vector2(x + i, z + j);
                    }
                }
            }
            float vert = maxLocalY - minLocalY;
            if (vert != 0)
            {
                float horiz = Mathf.Sqrt(Mathf.Pow(maxCoord.x - minCoord.x, 2) + Mathf.Pow(maxCoord.y - minCoord.y, 2));
                float tanAngle = vert / horiz;  // horiz can't be 0 because I would mean that the local max and min are at the same position => vert = 0
                float a = Mathf.Atan(tanAngle) * 360/ (2*Mathf.PI);
                if (a > steepnessMaxAngle)
                {
                    Debug.Log(a);
                    return;
                }
            }
        }

        base.spawnObject(x, z);
    }
}
