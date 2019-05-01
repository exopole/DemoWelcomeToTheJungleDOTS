using UnityEngine;
using Unity.Entities;

namespace WelcomeToTheJungle.ECS
{
    [RequireComponent(typeof(ConvertToEntity))]
    public class DeplacementProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float speed = 10;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            DeplacementData data = new DeplacementData { speed = this.speed};
            dstManager.AddComponentData(entity, data);
        }
    }
}
