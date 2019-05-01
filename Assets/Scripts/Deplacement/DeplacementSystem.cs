using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Burst;

namespace WelcomeToTheJungle.ECS
{
    public class DeplacementSystem : JobComponentSystem
    {
        // Use the [BurstCompile] attribute to compile a job with Burst. You may see significant speed ups, so try it!
        [BurstCompile]
        [RequireComponentTag(typeof(PlayerTagData))]
        struct RotationSpeedJob : IJobForEach<DeplacementData, Translation>
        {
            public float DeltaTime;
            public bool rightDeplacement;
            public bool leftDeplacement;
            public bool forwardDeplacement;
            public bool backDeplacement;

            public void Execute(ref DeplacementData speedData, ref Translation translation)
            {
                var deltaTime = DeltaTime;
                float v = (deltaTime * speedData.speed);
                Vector3 pos = translation.Value;
                if (rightDeplacement)
                {
                    pos += (new Vector3(1f, 0f, 0f) * v);
                }
                if (leftDeplacement)
                {
                    pos += (new Vector3(-1f, 0f, 0f) * v);
                }
                if (backDeplacement)
                {
                    pos += (new Vector3(0f, 0f, -1f) * v);
                }
                if (forwardDeplacement)
                {
                    pos += (new Vector3(0f, 0f, 1f) * v);
                }
                translation.Value = pos;
            }
            

            private void Deplacement(DeplacementData speedData, Translation translation, Vector3 vectDirection)
            {
                float v = (DeltaTime * speedData.speed);
                Vector3 pos = translation.Value;
                pos += (vectDirection* v);
                translation.Value = pos;
            }
        }

        // OnUpdate runs on the main thread.
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new RotationSpeedJob()
            {
                DeltaTime = Time.deltaTime,
                rightDeplacement = Input.GetKey(KeyCode.RightArrow),
                leftDeplacement = Input.GetKey(KeyCode.LeftArrow),
                forwardDeplacement = Input.GetKey(KeyCode.UpArrow),
                backDeplacement = Input.GetKey(KeyCode.DownArrow),
            };

            return job.Schedule(this, inputDependencies);
        }


    }
    
}
