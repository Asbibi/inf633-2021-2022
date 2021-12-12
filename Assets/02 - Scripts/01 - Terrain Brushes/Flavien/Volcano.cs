using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Volcano : TerrainBrush {
    public float height = 5;
    public float height_hole = 1; 

    public override void draw(int x, int z) {
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                var r = (float) Math.Sqrt(xi*xi + zi*zi);
                var h = 0.0f;
                 if (xi*xi + zi*zi <= radius*radius){ 
                    var h_0 = terrain.get(x+xi, z+zi);
                    if (r >= radius){
                        h = (float) (height*(radius - r)*(radius - r))/(radius*radius);
                    }
                    else{
                        h = (float) (height*(radius - radius)*(radius-radius))/(radius*radius) - height_hole + (float) height_hole*((r*r)/(radius*radius));
                    }
                    terrain.set(x + xi, z + zi, Math.Max(h_0,h));
                 }
            }
        }
    }
}

