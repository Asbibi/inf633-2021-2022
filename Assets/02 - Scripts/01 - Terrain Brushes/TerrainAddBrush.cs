using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainAddBrush : TerrainBrush {

    public bool addOnTop = false;

    protected void setTerrainHeight(int x, int z, float height)
    {
        if (addOnTop)
            terrain.set(x, z, height + terrain.get(x, z));
        else
            terrain.set(x, z, height);
    }

    protected void setTerrainHeight(float x, float z, float height)
    {
        setTerrainHeight((int)x, (int)z, height);
    }
}
