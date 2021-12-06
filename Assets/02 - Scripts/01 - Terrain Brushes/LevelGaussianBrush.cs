using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGaussianBrush : TerrainBrush {

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


        float sigma = radius * 0.6f;
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                float gaussianFallOffFactor = Mathf.Exp(-0.5f * (Mathf.Pow(xi, 2) / sigma)) * Mathf.Exp(-0.5f * (Mathf.Pow(zi, 2) / sigma));
                float currentHeight = terrain.get(x + xi, z + zi);
                float heightDifference = gaussianFallOffFactor * strengh * (currentHeight - heightAverage);
                terrain.set(x + xi, z + zi, currentHeight - heightDifference);
            }
        }
    }
}
