using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using System;

[UpdateAfter(typeof(SpawnerSystem))]
public class SpawnerTerrainSystem : JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem _EntityCommandBufferSystem;
    private EntityQuery m_Group;
    private EntityQuery m_GroupSpawner;
    private NativeArray<Entity> entities;

    protected override void OnCreateManager()
    {
        _EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        
    }

    protected override void OnCreate()
    {
        EntityQueryDesc query = new EntityQueryDesc
        {
            Any = new ComponentType[] { typeof(PositioningTag) }
        };
        m_Group = GetEntityQuery(query);
        

        EntityQueryDesc querySpawner = new EntityQueryDesc
        {
            Any = new ComponentType[] { typeof(Spawner) }
        };
        m_GroupSpawner = GetEntityQuery(querySpawner);
    }


    struct SpawnTerrainJob : IJobForEachWithEntity<SpawnerTerrain, LocalToWorld>
    {
        public EntityCommandBuffer CommandBuffer;
        public NativeArray<Entity> entities;
        public ComponentDataFromEntity<PositioningTag> positionTagAccess;
        public ComponentDataFromEntity<Rotation> rotationAccess;

        [BurstCompile]
        public void Execute(Entity entity, int index, ref SpawnerTerrain spawner, [ReadOnly] ref LocalToWorld location)
        {
            int i = 0;
            NativeArray<int> indexList;
            System.Random random = new System.Random();

            if (entities.Length != 0)
            {
                indexList = new NativeArray<int>(spawner.Erows * spawner.Ecols, Allocator.Temp);
                for (int j = 0; j < indexList.Length; j++)
                {
                    indexList[j] = j;
                }
                Shuffle(indexList);
            }
            else
            {
                indexList = new NativeArray<int>();
            }

            for (int x = 0; x < spawner.Ecols; x++)
            {
                for (int z = 0; z < spawner.Erows; z++)
                {
                    var instance = CommandBuffer.Instantiate(spawner.prefab);
                    var pos = math.transform(location.Value, new float3(x, noise.cnoise(new float2(x, z) * 0.21f), z));
                    CommandBuffer.SetComponent(instance, new Translation { Value = pos });
                    if (entities.Length != 0 && indexList[i] < entities.Length)
                    {
                        var position = positionTagAccess[entities[indexList[i]]];
                        var rotation = rotationAccess[entities[indexList[i]]];
                        pos.y += position.posX;
                        Quaternion valueRotation = math.mul(math.normalize(rotation.Value),
                                quaternion.AxisAngle(math.up(), (float)random.NextDouble() * 360));

                        CommandBuffer.SetComponent(entities[indexList[i]], new Translation { Value = pos });
                        CommandBuffer.SetComponent(entities[indexList[i]], new Rotation { Value = valueRotation });
                    }
                    i++;
                }
            }
            CommandBuffer.DestroyEntity(entity);
            if(indexList.IsCreated)
                indexList.Dispose();
        }

        public NativeArray<int> Shuffle(NativeArray<int> array)
        {
            System.Random _random = new System.Random();
            var random = _random;
            for (int i = array.Length; i > 1; i--)
            {
                // Pick random element to swap.
                int j = random.Next(i); // 0 <= j <= i-1
                                        // Swap.
                int tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
            return array;
        }


    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        NativeArray<Entity> entitiesSpawner = m_GroupSpawner.ToEntityArray(Allocator.TempJob, out inputDeps);
        inputDeps.Complete();

        if(entitiesSpawner.Length != 0)
        {
            entitiesSpawner.Dispose();
            return inputDeps;
        }

        entities = m_Group.ToEntityArray(Allocator.TempJob, out inputDeps);
        inputDeps.Complete();

        float time = DateTime.Now.Millisecond;
        var jobTerrain = new SpawnTerrainJob
        {
            CommandBuffer = _EntityCommandBufferSystem.CreateCommandBuffer(),
            entities = entities,
            positionTagAccess = _EntityCommandBufferSystem.GetComponentDataFromEntity<PositioningTag>(),
            rotationAccess = _EntityCommandBufferSystem.GetComponentDataFromEntity<Rotation>()
        }.ScheduleSingle(this, inputDeps);

        _EntityCommandBufferSystem.AddJobHandleForProducer(jobTerrain);

        jobTerrain.Complete();
        Debug.Log(" Calcul time after job : " + (DateTime.Now.Millisecond - time));
        entities.Dispose();
        entitiesSpawner.Dispose();
        return jobTerrain;
    }

}
