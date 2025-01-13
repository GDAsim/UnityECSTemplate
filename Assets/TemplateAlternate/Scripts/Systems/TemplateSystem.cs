namespace TemplateAlternate
{
    using Unity.Burst;
    using Unity.Entities;

    [BurstCompile]
    [DisableAutoCreation]
    public partial struct TemplateSystem : ISystem
    {
        public void OnCreate(ref SystemState state) { }

        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();

            EntityCommandBuffer ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            DoJobs(ref state, ref ecb);
        }

        void Do(ref SystemState state)
        {

        }

        void DoJobs(ref SystemState state, ref EntityCommandBuffer ecb)
        {
            new TemplateJob
            {
                ElapsedTime = SystemAPI.Time.ElapsedTime,
                Ecb = ecb.AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct TemplateJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter Ecb;
        public double ElapsedTime;

        // IJobEntity generates a component data query based on the parameters of its `Execute` method.
        // This example queries for all Spawner components and uses `ref` to specify that the operation
        // requires read and write access. Unity processes `Execute` for each entity that matches the
        // component data query.
        void Execute([ChunkIndexInQuery] int chunkIndex, ref TemplateData spawner)
        {
            //Ecb.SetComponent(chunkIndex, newBulletEntity, LocalTransform.FromPosition(pos));
        }
    }
}
