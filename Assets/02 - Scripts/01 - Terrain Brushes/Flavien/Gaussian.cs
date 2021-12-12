using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class Gaussian : TerrainBrush {

    public float height = 5;
    public float std = 10;

    public override void draw(int x, int z) {
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                if (xi * xi + zi * zi <= radius*radius){
                    var h = terrain.get(x+xi, z+zi);
                    float h_0 = height / (std* (1/ (float) Math.Sqrt(2* Math.PI)));
                    float h_new = 10 * h_0 * (float) Math.Exp(-0.5f * xi*xi + zi*zi)/std/std;
                    terrain.set(x + xi, z + zi, h_new);
                }
            }
        }
    }
}
