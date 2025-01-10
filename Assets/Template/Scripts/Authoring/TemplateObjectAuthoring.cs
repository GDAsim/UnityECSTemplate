namespace Template
{
    using Unity.Entities;
    using Unity.Mathematics;
    using UnityEngine;

    public class TemplateObjectAuthoring : MonoBehaviour
    {
        // Add MonoBehaviour Info here to transfer to Entity World
        public float SpawnRate;

        class Baker : Baker<TemplateObjectAuthoring>
        {
            public override void Bake(TemplateObjectAuthoring authoring)
            {
                // Transform the GameObject into Entity with transform data
                var baseEntity = GetEntity(TransformUsageFlags.Dynamic);

                // Add additional Components
                var data = new TemplateData()
                {
                    Position = authoring.transform.position,
                    Rotation = authoring.transform.rotation,
                    Scale = authoring.transform.localScale,
                };
                AddComponent(baseEntity, data);
            }
        }
    }

    public struct TemplateData : IComponentData
    {
        public float3 Position;
        public quaternion Rotation;
        public float3 Scale;
    }
}