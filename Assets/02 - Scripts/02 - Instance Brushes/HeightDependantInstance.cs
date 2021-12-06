using System.Collections;
using System.Collections.Generic;
using System;  
using System.IO;  
using System.Runtime.Serialization;  
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[Serializable]
public class GameObjectToSpawn {
    public int height;
    public GameObject prefab;
}


public class HeightDependantInstance : InstanceAddBrush
{
    public GameObjectToSpawn[] prefabsToSpawn;

    public override void draw(float x, float z)
    {
        snapToGrid(ref x, ref z);

        float height = terrain.get(x, z);
        foreach (GameObjectToSpawn objectToSpawn in prefabsToSpawn)
        {
            if (height <= objectToSpawn.height || objectToSpawn.height < 0)
            {
                // set the object to spawn to objectToSpawn.prefab here
                setInstancePrefab(objectToSpawn.prefab);
                smartSpwanObject(x, z);
                return;
            }
        }
    }
}