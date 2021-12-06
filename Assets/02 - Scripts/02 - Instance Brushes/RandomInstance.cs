using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInstance : InstanceAddBrush
{
    public bool randomAroundGridPoint = true;   // only on grid mode

    [Header("Random")]
    public int number = 5;
    public bool inCircle = false;

    public override void draw(float x, float z) {
        if (removePreviousObjectInArea)
            removeObjects(x, z);
        if (!randomAroundGridPoint)
            snapToGrid(ref x, ref z);

        for (int i = 0; i < number; i++)
        {
            float _x = x;
            float _z = z;
            if (inCircle)
            {
                float r = Random.Range(0f, radius);
                float a = Random.Range(0f, 2 * Mathf.PI);
                _x += r * Mathf.Cos(a);
                _z += r * Mathf.Sin(a);
            }
            else
            {
                _x += Random.Range(-radius, radius);
                _z += Random.Range(-radius, radius);
            }

            if (randomAroundGridPoint)
                snapToGrid(ref _x, ref _z);
            spawnObject(_x, _z);
        }
    }
}
