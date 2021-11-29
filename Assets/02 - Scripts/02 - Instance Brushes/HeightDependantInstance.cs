using System.Collections;
using System.Collections.Generic;
using System;  
using System.IO;  
using System.Runtime.Serialization;  
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[Serializable]
public class GameObjectToSpwan {
    public int height;
    public GameObject prefab;
}


public class HeightDependantInstance : InstanceBrush {    

    public GameObjectToSpwan[] prefabsToSpawn;

    public override void draw(float x, float z) {
        float height = terrain.get(x, z);
        foreach (GameObjectToSpwan object in prefabsToSpawn)
        {
            if (height <= object.height || object.height < 0)
            {
                spawnObject(x, z);
                return;
            }
        }
    }
}