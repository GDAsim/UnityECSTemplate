namespace Template
{
    using Unity.Burst;
    using Unity.Burst.Intrinsics;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Transforms;

    /// <summary>
    /// Entity Job with Required Components defined Explictly
    /// Note: This gets translated to IJobChunk after compile
    /// </summary>
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

    /// <summary>
    /// Entity Job with Arbitrary Data Lookup
    /// </summary>
    [BurstCompile]
    public partial struct TemplateJobDataLookup : IJobEntity
    {
        public ComponentLookup<LocalTransform> localTransformLU;
        [ReadOnly] public ComponentLookup<TemplateData> dataLU;

        public float time;

        void Execute(Entity e,
                    [EntityIndexInChunk] int EntityIndexInChunk,
                    [ChunkIndexInQuery] int ChunkIndexInQuery,
                    [EntityIndexInQuery] int EntityIndexInQuery)
        {
            if (!localTransformLU.HasComponent(e) || !dataLU.HasComponent(e)) return;

            var localTransform = localTransformLU.GetRefRW(e);
            var data = dataLU.GetRefRO(e).ValueRO;

            var pos = new float3(math.sin(data.Speed.x * time), math.sin(data.Speed.y * time), localTransform.ValueRO.Position.z);

            localTransform.ValueRW.Position = pos;
        }
    }

    /// <summary>
    /// Chunk Job
    /// </summary>
    [BurstCompile]
    public partial struct TemplateChunkJob : IJobChunk
    {
        public ComponentTypeHandle<LocalTransform> LocalTransformHandle;
        [ReadOnly] public ComponentTypeHandle<TemplateData> TemplateDataHandle;

        public float time;

        public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
        {
            //if (chunk.Has<OptionalComp>(OptionalCompType))

            var chunkLocalTransform = chunk.GetNativeArray(ref LocalTransformHandle);
            var chunkTemplateData = chunk.GetNativeArray(ref TemplateDataHandle);

            for (var i = 0; i < chunk.Count; i++)
            {
                var localTransform = chunkLocalTransform[i];
                var data = chunkTemplateData[i];

                localTransform.Position = new float3(math.sin(data.Speed.x * time), math.sin(data.Speed.y * time), localTransform.Position.z);
                
                chunkLocalTransform[i] = localTransform;
            }
        }
    }
}