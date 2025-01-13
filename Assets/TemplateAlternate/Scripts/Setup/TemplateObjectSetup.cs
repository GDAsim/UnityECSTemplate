namespace TemplateAlternate
{
    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Transforms;
    using UnityEngine;

    /// <summary>
    /// Alternative Way To Setup ECS, though monobehaviour instead of Authoring (not recommanded workflow)
    /// </summary>
    public class TemplateObjectSetup : MonoBehaviour
    {
        // Add MonoBehaviour Info here to transfer to Entity World
        [SerializeField] Vector3 Speed;

        EntityManager entityManager;
        Entity thisEntity;

        void Awake()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            // Create Entity using entityManager
            thisEntity = entityManager.CreateEntity();
            entityManager.SetName(thisEntity, name);

            // Add basic Components
            entityManager.AddComponent<LocalTransform>(thisEntity);

            // Add additional Components
            var data = new TemplateData()
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Scale = transform.localScale,
                Speed = Speed
            };
            entityManager.AddComponentData(thisEntity, data);
        }

        void Update()
        {
            // Extract Component from ECS world (Live World) using entity Manager
            var t = entityManager.GetComponentData<LocalTransform>(thisEntity);
            transform.position = t.Position;
        }
    }

    public struct TemplateData : IComponentData
    {
        public float3 Position;
        public quaternion Rotation;
        public float3 Scale;
        public float3 Speed;
    }
}