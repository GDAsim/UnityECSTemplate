namespace Template
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Transforms;

    /// <summary>
    /// Template Managed System
    /// Note: Use this for Operating on Unmanaged + Managed Components
    /// Note: Can use Entities.ForEach
    /// </summary>
    [RequireMatchingQueriesForUpdate] // Run OnUpdate Only when there's valid Queries
    [DisableAutoCreation]
    public partial class TemplateManagedSystem : SystemBase
    {
        EntityQuery query;

        public ComponentTypeHandle<LocalTransform> localTransformHandle;
        [ReadOnly] public ComponentTypeHandle<TemplateData> templateDataHandle;

        /// <summary>
        /// Called when System is created
        /// </summary>
        protected override void OnCreate()
        {
            // Build Query Method 1 - GetEntityQuery()
            query = GetEntityQuery(ComponentType.ReadOnly<TemplateData>());

            // Build Query Method 2 - Using EntityQueryDesc 
            EntityQueryDesc qdesc = new EntityQueryDesc
            {
                All = new ComponentType[] // All of the inside must be in the archetype & enabled
                {
                    typeof(LocalTransform),
                    typeof(TemplateData)
                },
                Disabled = new ComponentType[] // All of the inside must be in the archetype & disabled
                {
                },
                None = new ComponentType[] // None of the inside must be in the archetype ignore disabled
                {
                },
                Absent = new ComponentType[] // None of the inside must be in the archetype including disabled
                {
                },
            };
            query = GetEntityQuery(qdesc);

            localTransformHandle = GetComponentTypeHandle<LocalTransform>(false);
            templateDataHandle = GetComponentTypeHandle<TemplateData>(true);
        }

        /// <summary>
        /// Called when System is Destoryed
        /// </summary>
        protected override void OnDestroy() { }

        /// <summary>
        /// Called Just before the first OnUpdate() - pre-update
        /// </summary>
        protected override void OnStartRunning()
        {
            base.OnStartRunning();
        }

        /// <summary>
        /// Called when system stop updating and before OnDestroy
        /// </summary>
        protected override void OnStopRunning()
        {
            base.OnStartRunning();
        }

        protected override void OnUpdate()
        {
            // Create ECB Method 1 - Create From System - https://docs.unity3d.com/Packages/com.unity.entities@1.3/manual/systems-entity-command-buffer-automatic-playback.html
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged);

            // Create ECB Method 2 - Create Manual Adhoc - https://docs.unity3d.com/Packages/com.unity.entities@1.3/manual/systems-entity-command-buffer-use.html
            // Manually do ecs.Playback() and ecb.Dispose()
            //var ecb = new EntityCommandBuffer(Allocator.TempJob);

            // Choose!! Uncomment one 
            //DoEntitiesForeach();
            //DoEntitiesForeachJob();
            //DoEntitiesForeachParallelJob();
            //DoECSJobs();
            //DoECSJobsWithQuery(ref query);
            //DoECSJobsDataLookup();
            //DoECSChunkJob(ref query,ref localTransformHandle,ref templateDataHandle);

            // Required For ECB Method 2
            //Dependency.Complete();
            //ecb.Playback(state.EntityManager);
            //ecb.Dispose();
        }


        /// <summary>
        /// https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/iterating-data-entities-foreach.html
        /// 
        /// === Entities.Foreach ===
        /// Get Components using Entities.Foreach in a Linq code style
        /// 
        /// Extra Options
        /// .WithAll<Comp>      - returns Entities that have all the specified components
        /// .WithAny<Comp>      - returns Entities that have at least one of the specified components
        /// .WithNone<Comp>     - returns Entities that do not have any of the specified components
        /// .WithChangeFilter() - returns Entities entities that *might* have changed
        /// and more....
        /// 
        /// Note: use "ref" for ReadWrite, "in" for ReadOnly
        /// Note: Up to 8 Parameters can be passed in the Foreach
        /// Note: This Linq style code are translated into proper ECS code hence some code are not allowed (refer to docs)
        /// Note: for jobs, you need to Managed Job Dependency
        /// 
        /// Note: Able to capture/record certain variables e.g native containers (refer to docs)
        /// </summary>
        void DoEntitiesForeach()
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var time = (float)SystemAPI.Time.ElapsedTime;

            Entities
                .WithName("Move") // Shown in error messages and profiler
                .WithAll<LocalToWorld>() // Entities that have this Component
                .ForEach((
                    Entity e, // Optional
                    int entityInQueryIndex, // Optional - The Index of the entity in the list of the query
                    int nativeThreadIndex, // Optional - The current ThreadIndex for used when running in a job
                    ref LocalTransform transform, in TemplateData data) =>
            {
                transform.Position = new float3(math.sin(data.Speed.x * time), transform.Position.y, transform.Position.z);
            })
                //.WithoutBurst() // Use when using code not supported by Burst (refer to docs)
                .WithBurst(FloatMode.Fast, FloatPrecision.Low, synchronousCompilation: true) // Use when code is supported by Burst (refer to docs)
                .Run();
        }
        void DoEntitiesForeachJob()
        {
            var time = (float)SystemAPI.Time.ElapsedTime;
            var deltaTime = SystemAPI.Time.DeltaTime;

            Entities
                .WithName("MoveJob") // Shown in error messages and profiler
                .WithAll<LocalToWorld>() // Entities that have this Component
                .ForEach((
                    Entity e, // Optional
                    int entityInQueryIndex, // Optional - The Index of the entity in the list of the query
                    int nativeThreadIndex, // Optional - The current ThreadIndex for used when running in a job
                    ref LocalTransform transform, in TemplateData data) =>
                {
                    transform.Position = new float3(math.sin(data.Speed.x * time), transform.Position.y, transform.Position.z);
                })
                //.WithoutBurst() // Use when using code not supported by Burst (refer to docs)
                .WithBurst(FloatMode.Fast, FloatPrecision.Low, synchronousCompilation: true) // Use when code is supported by Burst (refer to docs)
                .Schedule();
        }
        void DoEntitiesForeachParallelJob()
        {
            var time = (float)SystemAPI.Time.ElapsedTime;
            var deltaTime = SystemAPI.Time.DeltaTime;

            Entities
                .WithName("MoveParallelJob") // Shown in error messages and profiler
                .WithAll<LocalToWorld>() // Entities that have this Component
                .ForEach((
                    Entity e, // Optional
                    int entityInQueryIndex, // Optional - The Index of the entity in the list of the query
                    int nativeThreadIndex, // Optional - The current ThreadIndex for used when running in a job
                    ref LocalTransform transform, in TemplateData data) =>
                {
                    transform.Position = new float3(math.sin(data.Speed.x * time), transform.Position.y, transform.Position.z);
                })
                //.WithoutBurst() // Use when using code not supported by Burst (refer to docs)
                .WithBurst(FloatMode.Fast, FloatPrecision.Low, synchronousCompilation: true) // Use when code is supported by Burst (refer to docs)
                .ScheduleParallel();
        }

        /// <summary>
        /// https://docs.unity3d.com/Packages/com.unity.entities@1.3/manual/iterating-data-ijobentity.html
        /// 
        /// === ECS Jobs aka IJobEntity or Jobs for Entity===
        /// Note: IJobEntity is reusable
        /// Note: IJobEntity takes less time to compile than Entities.ForEach
        /// </summary>
        void DoECSJobs()
        {
            new TemplateJob
            {
                time = (float)SystemAPI.Time.ElapsedTime,
            }.ScheduleParallel();
        }
        /// <summary>
        /// https://docs.unity3d.com/Packages/com.unity.entities@1.3/manual/iterating-data-ijobentity.html
        /// 
        /// === ECS Jobs aka IJobEntity or Jobs for Entity===
        /// Note: IJobEntity is reusable
        /// Note: IJobEntity takes less time to compile than Entities.ForEach
        /// </summary>
        void DoECSJobsWithQuery(ref EntityQuery query)
        {
            new TemplateJob
            {
                time = (float)SystemAPI.Time.ElapsedTime,
            }.ScheduleParallel(query);
        }

        /// <summary>
        /// https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/systems-looking-up-data.html
        /// 
        /// === ECS Jobs with Arbitrary Data Lookup ===
        /// Note: This cannot be Schedule ScheduleParallel if there are write operations
        /// </summary>
        void DoECSJobsDataLookup()
        {
            new TemplateJobDataLookup
            {
                localTransformLU = GetComponentLookup<LocalTransform>(false),
                dataLU = GetComponentLookup<TemplateData>(true),
                time = (float)SystemAPI.Time.ElapsedTime,
            }.Schedule();
        }

        /// <summary>
        /// https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/iterating-data-ijobchunk.html?q=IJobChunk
        /// 
        /// === ECS Chunk Jobs ===
        /// Note: Have to do a lot of extra work, that other methods automatically does for us
        /// </summary>
        void DoECSChunkJob(ref EntityQuery query,
            ref ComponentTypeHandle<LocalTransform> localTransformHandle,
            ref ComponentTypeHandle<TemplateData> templateDataHandle)
        {
            localTransformHandle.Update(this);
            templateDataHandle.Update(this);
            var job = new TemplateChunkJob
            {
                LocalTransformHandle = localTransformHandle,
                TemplateDataHandle = templateDataHandle,
                time = (float)SystemAPI.Time.ElapsedTime,
            };

            Dependency = job.ScheduleParallel(query, Dependency);
        }
    }
}