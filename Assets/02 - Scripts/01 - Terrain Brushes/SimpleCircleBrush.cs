using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCircleBrush : TerrainAddBrush {

    public float height = 5;

    public override void draw(int x, int z) {
        int radiusSquare = radius * radius;
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                if (xi*xi + zi*zi <= radiusSquare)
                    setTerrainHeight(x + xi, z + zi, height);
            }
        }
    }
}