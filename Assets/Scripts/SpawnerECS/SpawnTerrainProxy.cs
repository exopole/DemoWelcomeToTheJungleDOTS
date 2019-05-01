using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class SpawnTerrainProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public GameObject cube;
    public int rows;
    public int cols;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnerData = new SpawnerTerrain
        {
            prefab = conversionSystem.GetPrimaryEntity(cube),
            Erows = rows,
            Ecols = cols,
        };
        dstManager.AddComponentData(entity, spawnerData);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(cube);
    }
}
