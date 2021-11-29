using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBrush : TerrainBrush {

    public float strengh = 5;

    public override void draw(int x, int z) {
        /*float valueAverage = 0;
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                valueAverage += terrain.get(x + xi, z + zi);
                    //setTerrainHeight(x + xi, z + zi, height);
            }
        }
        valueAverage /= (4*radius*radius);
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                float heightDifference = strengh * (...sa heihgt actuelle - la height moyenne);
                setTerrainHeight(x + xi, z + zi, heightDifference + sa height actuelle);
            }
        }*/
    }
}
