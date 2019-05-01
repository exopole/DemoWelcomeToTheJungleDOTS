using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;



// JobComponentSystems can run on worker threads.
// However, creating and removing Entities can only be done on the main thread to prevent race conditions.
// The system uses an EntityCommandBuffer to defer tasks that can't be done inside the Job.
public class SpawnerDemoSystem : JobComponentSystem
{
    // EndSimulationEntityCommandBufferSystem is used to create a command buffer which will then be played back
    // when that barrier system executes.
    // Though the instantiation command is recorded in the SpawnJob, it's not actually processed (or "played back")
    // until the corresponding EntityCommandBufferSystem is updated. 
    EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

    protected override void OnCreateManager()
    {
        // Cache the EndSimulationEntityCommandBufferSystem in a field, so we don't have to create it every frame
        _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    // This declares a new kind of job, which is a unit of work to do.
    // The job is declared as an IJobForEachWithEntity<LocalToWorld, SpawnerDemoData>,
    // meaning it will process all entities in the world that have both
    // LocalToWorld and SpawnerDemoData components. Change it to process the component
    // types you want.

    struct SpawnerDemoSystemJob : IJobForEachWithEntity<LocalToWorld, SpawnerDemoData>
    {
        // Add fields here that your job needs to do its work.
        //A thread-safe command buffer that can buffer commands that affect entities and components for later playback.
        public EntityCommandBuffer commandBuffer; 

        public void Execute(Entity entity, int index, ref LocalToWorld location, ref SpawnerDemoData data)
        {
            for (int x = 0; x < data.Ecol; x++)
            {
                for (int z = 0; z < data.Erow; z++)
                {
                    //Create your entity from the prefab
                    Entity instance = commandBuffer.Instantiate(data.prefab);
                    //define the position
                    float3 pos = math.transform(location.Value, new float3(x, noise.cnoise(new float2(x,z) *0.21f), z));
                    //set the position in the world
                    commandBuffer.SetComponent(instance, new Translation() { Value = pos});
                }
            }
            //Destroy the spawner entity
            commandBuffer.DestroyEntity(entity);
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        //Instead of performing structural changes directly, a Job can add a command to an EntityCommandBuffer to perform such changes on the main thread after the Job has finished.
        //Command buffers allow you to perform any, potentially costly, calculations on a worker thread, while queuing up the actual insertions and deletions for later.

        // Schedule the job that will add Instantiate commands to the EntityCommandBuffer.
        var job = new SpawnerDemoSystemJob()
        {
            commandBuffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer() // instantiate the Buffer
        }.ScheduleSingle(this, inputDependencies) ;

        // SpawnJob runs in parallel with no sync point until the barrier system executes.
        // When the barrier system executes we want to complete the SpawnJob and then play back the commands (Creating the entities and placing them).
        // We need to tell the barrier system which job it needs to complete before it can play back the commands.
        _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(job);

        return job;
    }
}