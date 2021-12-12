using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IncrementalBrush: TerrainBrush {

    public override void draw(int x, int z) {
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                if (xi * xi + zi *zi <= radius*radius){
                    var h = terrain.get(x + xi, z + zi);
                    terrain.set(x + xi, z + zi, h+1);
                }
            }
        }
    }
}