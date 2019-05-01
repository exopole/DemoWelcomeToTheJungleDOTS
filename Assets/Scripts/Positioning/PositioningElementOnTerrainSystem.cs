using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class PositioningElementOnTerrainSystem : JobComponentSystem
{

    EndSimulationEntityCommandBufferSystem _EntityCommandBufferSystem;
    private EntityQuery m_GroupPositioning;



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
        m_GroupPositioning = GetEntityQuery(query);
    }




    // This declares a new kind of job, which is a unit of work to do.
    // The job is declared as an IJobForEach<Translation, Rotation>,
    // meaning it will process all entities in the world that have both
    // Translation and Rotation components. Change it to process the component
    // types you want.
    //
    // The job is also tagged with the BurstCompile attribute, which means
    // that the Burst compiler will optimize it for the best performance.
    [BurstCompile]
    struct PositioningElementOnTerrainSystemJob : IJob
    {
        // Add fields here that your job needs to do its work.
        // For example,
        //    public float deltaTime;
        
        public void Execute()
        {
           // throw new System.NotImplementedException();
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new PositioningElementOnTerrainSystemJob();
        
        // Assign values to the fields on your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     job.deltaTime = UnityEngine.Time.deltaTime;
        
        
        
        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(inputDependencies);
    }
}