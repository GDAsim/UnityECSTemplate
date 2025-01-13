namespace TemplateSpawner
{
    using Unity.Burst;
    using Unity.Entities;
    using Unity.Transforms;

    [BurstCompile]
    [DisableAutoCreation]
    public partial struct TemplateSpawnerSystem : ISystem
    {
        public void OnCreate(ref SystemState state) { }

        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            EntityCommandBuffer ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            //vs
            //EntityCommandBuffer ecb2 = new EntityCommandBuffer(Allocator.TempJob);
            //EntityCommandBuffer.ParallelWriter parallelEcb = ecb2.AsParallelWriter();

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
        void Execute([ChunkIndexInQuery] int chunkIndex, ref SpawnerData spawner)
        {
            if (spawner.NextSpawnTime < ElapsedTime)
            {
                // Spawns a new entity and positions it at the spawner.
                Entity newBulletEntity = Ecb.Instantiate(chunkIndex, spawner.EntitySpawnPrefab);

                var pos = spawner.SpawnPosition;
                Ecb.SetComponent(chunkIndex, newBulletEntity, LocalTransform.FromPosition(pos));

                // Resets the next spawn time.
                spawner.NextSpawnTime = (float)ElapsedTime + spawner.SpawnRate;
            }
        }
    }
}
