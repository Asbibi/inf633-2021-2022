using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBrush : TerrainBrush {
    public override void draw(int x, int z)
    {
        Vector3 terrainSize = terrain.gridSize();
        for (int zi = 0; zi <= terrainSize.z; zi++)
        {
            for (int xi = 0; xi <= terrainSize.x; xi++)
            {
                terrain.set(xi, zi, 0);
            }
        }
    }
}
