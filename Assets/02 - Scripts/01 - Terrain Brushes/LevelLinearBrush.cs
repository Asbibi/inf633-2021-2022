using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLinearBrush : TerrainBrush {

    public float strengh = 0.1f;

    public override void draw(int x, int z) {
        float heightAverage = 0;
        int count = 0;
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                heightAverage += terrain.get(x + xi, z + zi);
                count++;
            }
        }
        heightAverage /= count;

        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                float linearFallOffFactor = Mathf.Max(1 - ((float)(xi*xi + zi*zi) / (radius * radius)), 0f);
                float currentHeight = terrain.get(x + xi, z + zi);
                float heightDifference = linearFallOffFactor * strengh * (currentHeight - heightAverage);
                terrain.set(x + xi, z + zi, currentHeight - heightDifference);
            }
        }
    }
}
