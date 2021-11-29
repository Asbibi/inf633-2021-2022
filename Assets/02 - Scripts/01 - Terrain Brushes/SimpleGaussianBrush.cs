using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGaussianBrush : TerrainAddBrush {

    public float height = 5;

    public override void draw(int x, int z) {
        float sigma = radius * 0.6f;
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                float currentHeight = height * Mathf.Exp(-0.5f * (Mathf.Pow(xi, 2)/sigma)) * Mathf.Exp(-0.5f * (Mathf.Pow(zi, 2)/sigma));
                setTerrainHeight(x + xi, z + zi, currentHeight);
            }
        }
    }
}
