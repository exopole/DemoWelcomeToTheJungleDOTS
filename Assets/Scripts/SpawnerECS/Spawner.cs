using Unity.Entities;
using System;
using System.Collections.Generic;
using Unity.Collections;

public struct Spawner : IComponentData
{
    public Entity prefab { get; set; }
    //public int Erows { get; set; }
    //public int Ecols { get; set; }
    public int number { get; set; }
}


public struct SpawnerTerrain : IComponentData
{
    public Entity prefab { get; set; }
    public int Erows { get; set; }
    public int Ecols { get; set; }
}