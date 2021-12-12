using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Erosion : TerrainBrush {

    public float height = 5;
    public int erosion_pixels_numb = 10;

    public override void draw(int x, int z) {
        int[] N_pixels_x = new int[erosion_pixels_numb];
        int[] N_pixels_z = new int[erosion_pixels_numb];
        System.Random randNum = new System.Random();
        System.Random randNum2 = new System.Random();
        for (int i = 0; i < N_pixels_x.Length; i++)
        {
            N_pixels_x[i] = randNum.Next(-radius, radius);
            N_pixels_z[i] = randNum2.Next(-radius, radius);
        }

        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                var h = terrain.get(x + xi, z + zi);
                bool cond1 = N_pixels_x.Contains((int)xi);
                bool cond2 = N_pixels_x.Contains((int)zi);
                if (cond1 && cond2) {
                    terrain.set(x + xi, z + zi, h - 1);
                }
            }
        }
    }
}

