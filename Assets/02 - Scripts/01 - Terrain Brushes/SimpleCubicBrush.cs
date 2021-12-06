using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCubicBrush : TerrainAddBrush
{
    public float height = 5;

    public override void draw(int x, int z)
    {
        if (radius == 0)
            return;

        float a = 2f / Mathf.Pow(radius, 3);
        float b = -3f / Mathf.Pow(radius, 2);
        // c = 0
        // d = 1

        for (int zi = -radius; zi <= radius; zi++)
        {
            for (int xi = -radius; xi <= radius; xi++)
            {
                float r = Mathf.Min(Mathf.Sqrt(xi * xi + zi * zi), radius);
                float wantedHeight = height * (a * r * r * r + b * r * r + 1);
                setTerrainHeight(x + xi, z + zi, wantedHeight);
            }
        }
    }
}