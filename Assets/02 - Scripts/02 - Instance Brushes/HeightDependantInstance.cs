using System.Collections;
using System.Collections.Generic;
using System;  
using System.IO;  
using System.Runtime.Serialization;  
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[Serializable]
public class GameObjectToSpawn {
    public int height = -1;
    public GameObject prefab;
}


public class HeightDependantInstance : InstanceAddBrush
{
    public GameObjectToSpawn[] prefabsToSpawn;

    public override void draw(float x, float z)
    {
        snapToGrid(ref x, ref z);

        float height = terrain.get(x, z);
        GameObjectToSpawn preferredObject = null;
        foreach (GameObjectToSpawn objectToSpawn in prefabsToSpawn)
        {
            if (height >= objectToSpawn.height && (preferredObject ==null || objectToSpawn.height > preferredObject.height))
            {
                preferredObject = objectToSpawn;
            }
        }

        if (preferredObject == null)
            return;
        setInstancePrefab(preferredObject.prefab);
        smartSpwanObject(x, z);
    }
}