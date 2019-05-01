using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace WelcomeToTheJungle.ECS
{

    // Use to convert your Monobehaviour data in a component data 
    // add the component in a entity form from the Gameobject parent
    [RequireComponent(typeof(ConvertToEntity))]
    public class RotationProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float DegreesPerSecond = 360;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var data = new RotationData { RadiansPerSecond = math.radians(DegreesPerSecond) };
            dstManager.AddComponentData(entity, data);
        }
    }
}