namespace TemplateAlternate
{
    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Transforms;
    using UnityEngine;

    public class TemplateObjectAuthoring : MonoBehaviour
    {
        public float Speed;

        void Awake()
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var baseEntity = entityManager.CreateEntity();
            entityManager.SetName(baseEntity, name);

            // Add basic Components
            entityManager.AddComponent<LocalTransform>(baseEntity);

            // Add additional Components
            var data = new TemplateData()
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Scale = transform.localScale,
                Speed = Speed
            };
            entityManager.AddComponentData(baseEntity, data);

            //?????
            //entityManager.AddComponentObject(baseEntity, transform);
            //entityManager.AddComponentObject(baseEntity, this);
        }
    }

    public struct TemplateData : IComponentData
    {
        public float3 Position;
        public quaternion Rotation;
        public float3 Scale;

        public float Speed;
    }
}