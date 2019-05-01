using System;
using Unity.Entities;

namespace WelcomeToTheJungle.ECS
{
    /// <summary>
    /// Component class which contain the rotationSpeed
    /// </summary>
    [Serializable]
    public struct RotationData : IComponentData
    {
        public float RadiansPerSecond;
    }
}