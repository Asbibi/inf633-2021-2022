using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstanceBrush : Brush {

    private int prefab_idx;

    public override void callDraw(float x, float z) {
        if (terrain.object_prefab)
            prefab_idx = terrain.registerPrefab(terrain.object_prefab);
        else {
            prefab_idx = -1;
            terrain.debug.text = "No prefab to instantiate";
            return;
        }
        Vector3 grid = terrain.world2grid(x, z);
        draw(grid.x, grid.z);
    }

    public override void draw(int x, int z) {
        draw((float)x, (float)z);
    }

    public virtual void spawnObject(float x, float z) {
        if (prefab_idx == -1) {
            return;
        }
        float scale_diff = Mathf.Abs(terrain.max_scale - terrain.min_scale);
        float scale_min = Mathf.Min(terrain.max_scale, terrain.min_scale);
        float scale = (float)CustomTerrain.rnd.NextDouble() * scale_diff + scale_min;
        terrain.spawnObject(terrain.getInterp3(x, z), scale, prefab_idx);
    }

    protected void removeObjects(float x, float z)
    {
        int instanceCount = terrain.getObjectCount();
        List<int> instanceToRemove = new List<int>();
        for (int i = 0; i < instanceCount; i++)
        {
            Vector3 terrainDimension = terrain.terrainSize();
            Vector3 instancePosition = terrain.getObject(i).position;

            instancePosition.x = (instancePosition.x * terrainDimension.x) - x;
            instancePosition.z = (instancePosition.z * terrainDimension.z) - z;
            if (Mathf.Abs(instancePosition.x) < (radius + 1) && Mathf.Abs(instancePosition.z) < (radius + 1))
                instanceToRemove.Add(i);
        }
        terrain.removeObjects(instanceToRemove);
    }

    protected void setInstancePrefab(GameObject prefabToInstanciate)
    {        
        terrain.object_prefab = prefabToInstanciate;
        prefab_idx = terrain.registerPrefab(terrain.object_prefab);
    }
}
