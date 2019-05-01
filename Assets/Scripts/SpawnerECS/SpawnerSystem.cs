using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using System;

public class SpawnerSystem : JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem _EntityCommandBufferSystem;
    NativeArray<Entity> _allEntity;

    protected override void OnCreateManager()
    {
        _EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    struct SpawnJob : IJobForEachWithEntity<Spawner, LocalToWorld>
    {
        public EntityCommandBuffer CommandBuffer;
        //public Entity[] list;
        public void Execute(Entity entity, int index, ref Spawner spawner, [ReadOnly] ref LocalToWorld location)
        {
            float time = DateTime.Now.Millisecond;
            int i = 0;
            for (i = 0; i < spawner.number; i++)
            {
                var instance = CommandBuffer.Instantiate(spawner.prefab);
                var pos = math.transform(location.Value, new float3(0, 0, 0));
                CommandBuffer.SetComponent(instance, new Translation { Value = pos });
            }
            //for (int x = 0; x < spawner.Ecols; x++)
            //{
            //    for (int z = 0; z < spawner.Erows; z++)
            //    {
            //        var instance = CommandBuffer.Instantiate(spawner.prefab);
            //        var pos = math.transform(location.Value, new float3(x, noise.cnoise(new float2(x, z) * 0.21f), z));
            //        CommandBuffer.SetComponent(instance, new Translation { Value = pos });
            //    }
            //}
            Debug.Log(i + " Calcul time : " + (DateTime.Now.Millisecond - time));
            //Debug.Log("Prefabs count : " + spawner.prefabs.Capacity);
            CommandBuffer.DestroyEntity(entity);
        }


    }
    

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        var job = new SpawnJob
        {
            CommandBuffer = _EntityCommandBufferSystem.CreateCommandBuffer(),
        }.ScheduleSingle(this, inputDeps);

        _EntityCommandBufferSystem.AddJobHandleForProducer(job);
        
        return job;
    }
}
