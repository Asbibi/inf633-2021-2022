using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSquareInstance : InstanceBrush {

    public int number = 5;

    public override void draw(float x, float z) {
        for (int i = 0; i < number; i++)
            spawnObject(x + Random.Range(-radius, radius), z + Random.Range(-radius, radius));
    }
}
