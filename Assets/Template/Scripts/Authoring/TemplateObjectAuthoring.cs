namespace Template
{
    using Unity.Entities;
    using Unity.Mathematics;
    using UnityEngine;

    public class TemplateObjectAuthoring : MonoBehaviour
    {
        // Add MonoBehaviour Info here to transfer to Entity World
        [SerializeField] Vector3 Speed;

        class Baker : Baker<TemplateObjectAuthoring>
        {
            public override void Bake(TemplateObjectAuthoring authoring)
            {
                // Transform the GameObject into Entity with transform data (refer to docs on the conversion)
                // https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.TransformUsageFlags.html

                var baseEntity = GetEntity(TransformUsageFlags.Dynamic);            // We want entity with transform data
                //var baseEntity = GetEntity(TransformUsageFlags.ManualOverride);   // We will perform the transform conversion ourselves
                //var baseEntity = GetEntity(TransformUsageFlags.NonUniformScale);  // We want entity with non uniform scale
                //var baseEntity = GetEntity(TransformUsageFlags.None);             // We want entity only
                //var baseEntity = GetEntity(TransformUsageFlags.Renderable);       // We want to draw the entity only and not move it
                //var baseEntity = GetEntity(TransformUsageFlags.WorldSpace);       // We want entity to be represented in worldspace regardless of parent

                // Add Components to baseEntity
                var data = new TemplateData()
                {
                    Position = authoring.transform.position,
                    Rotation = authoring.transform.rotation,
                    Scale = authoring.transform.localScale,
                    Speed = authoring.Speed
                };
                AddComponent(baseEntity, data);

                // Create any additionalEntity
                var additionalEntity = CreateAdditionalEntity(TransformUsageFlags.Dynamic, bakingOnlyEntity: false, entityName: "AdditionalEntity");

                // Add Components additionalEntity
                var data2 = new TemplateData()
                {
                    Position = authoring.transform.position,
                    Rotation = authoring.transform.rotation,
                    Scale = authoring.transform.localScale,
                    Speed = authoring.Speed
                };
                AddComponent(additionalEntity, data2);
            }
        }
    }

    public struct TemplateData : IComponentData,
        IEnableableComponent // Optional - This Component can be enable/disable
    {
        public float3 Position;
        public quaternion Rotation;
        public float3 Scale;
        public float3 Speed;
    }
}