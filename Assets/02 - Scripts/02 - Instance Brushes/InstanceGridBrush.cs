using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstanceGridBrush : InstanceBrush
{
    [Header("Grid")]
    public bool snapOnGrid = false;
    public int xSubdivision = 100;
    public int zSubdivision = 100;

    private float snapXToGrid(float x)
    {
        float cellWidth = terrain.terrainSize().x / xSubdivision;
        int nbPastCell = 0;
        while (cellWidth * nbPastCell < x)
            nbPastCell++;

        if (cellWidth * (nbPastCell - 0.5) < x)
            return nbPastCell * cellWidth;
        else
            return (nbPastCell - 1) * cellWidth;
    }

    private float snapZToGrid(float z)
    {
        float cellHeight = terrain.terrainSize().z / zSubdivision;
        int nbPastCell = 0;
        while (cellHeight * nbPastCell < z)
            nbPastCell++;

        if (cellHeight * (nbPastCell - 0.5) < z)
            return nbPastCell * cellHeight;
        else
            return (nbPastCell - 1) * cellHeight;
    }

    protected void snapToGrid(ref float x, ref float z)
    {
        if (!snapOnGrid)
            return;

        x = snapXToGrid(x);
        z = snapZToGrid(z);
    }
}
