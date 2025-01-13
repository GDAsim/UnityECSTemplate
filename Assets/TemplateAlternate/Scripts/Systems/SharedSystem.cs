namespace TemplateAlternate
{
    using Unity.Burst;
    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Transforms;

    [BurstCompile]
    public partial struct TemplateJob : IJobEntity
    {
        public float time;

        // IJobEntity generates a component data query based on the parameters of its `Execute` method.
        // This example queries for all Spawner components and uses `ref` to specify that the operation
        // requires read and write access. Unity processes `Execute` for each entity that matches the
        // component data query.
        void Execute(Entity e, // Optional
            ref LocalTransform transform, in TemplateData data,
            [EntityIndexInQuery] int EntityIndexInQuery,  // Optional
            [EntityIndexInChunk] int EntityIndexInChunk,  // Optional
            [ChunkIndexInQuery] int ChunkIndexInQuery)  // Optional
        {
            transform.Position = new float3(math.sin(data.Speed.x * time), math.sin(data.Speed.y * time), transform.Position.z);
        }
    }
}