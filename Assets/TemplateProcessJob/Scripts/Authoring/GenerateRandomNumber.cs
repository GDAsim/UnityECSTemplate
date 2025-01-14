namespace TemplateProcessJob
{
    using Unity.Entities;
    using UnityEngine;

    public class GenerateRandomNumber : MonoBehaviour
    {
        // Add MonoBehaviour Info here to transfer to Entity World
        public int Count;

        class Baker : Baker<GenerateRandomNumber>
        {
            public override void Bake(GenerateRandomNumber authoring)
            {
                // Transform the GameObject into Entity with transform data
                var baseEntity = GetEntity(TransformUsageFlags.Dynamic);

                // Add additional Components
                DynamicBuffer<RandomDataBuffer> buffer = AddBuffer<RandomDataBuffer>(baseEntity);
                buffer.Length = authoring.Count;
            }
        }
    }

    [InternalBufferCapacity(32)]
    public struct RandomDataBuffer : IBufferElementData
    {
        public float RandomValue;
    }
}