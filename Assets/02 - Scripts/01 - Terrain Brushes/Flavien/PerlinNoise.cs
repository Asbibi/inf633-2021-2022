using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class PerlinNoise : TerrainBrush {
    public float height = 5;
    public float scale = 10; 

    public override void draw(int x, int z) {
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                var h = terrain.get(x+xi, z+zi);
                float X = xi/scale; 
                float Z = zi/scale; 
                float perlin = height * Mathf.PerlinNoise(X, Z); 
                terrain.set(x + xi, z + zi, perlin);
            }
        }
    }
}
