using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GaussianIncremental : TerrainBrush
{
    public float height = 50;
    public float std = 8;

    public static float GetTerrainMax(CustomTerrain terrain, int x, int z, int radius)
    {
        float max_h = 4;
        for (int zi = -radius; zi <= radius; zi++)
        {
            for (int xi = -radius; xi <= radius; xi++)
            {
                float hauteur = terrain.get(x + xi, z + zi);
                if (hauteur > max_h)
                {
                    max_h = hauteur;
                }
            }
        }
        return max_h;
    }

    public override void draw(int x, int z)
    {
        const float inv_sqrt_2pi = 0.3989422804014327f;
        float max_height = GetTerrainMax(terrain, x, z, radius);
        for (int zi = -radius; zi <= radius; zi++)
        {
            for (int xi = -radius; xi <= radius; xi++)
            {
                if (xi * xi + zi * zi <= radius * radius)
                {
                    var h = terrain.get(x + xi, z + zi);
                    float temporal_height = (float)height / std * inv_sqrt_2pi;
                    float new_height = 1.2f * max_height * (float) Math.Exp(-0.5f * (Math.Abs(xi) * Math.Abs(xi) + Math.Abs(zi) * Math.Abs(zi)) / std / std) ;
                    print(new_height);
                    terrain.set(x + xi, z + zi, new_height);
                }
            }
        }
    }
}