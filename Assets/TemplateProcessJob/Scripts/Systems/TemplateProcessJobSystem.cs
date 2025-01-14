namespace TemplateProcessJob
{
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;

    [BurstCompile]
    [DisableAutoCreation]
    public partial class TemplateProcessJobSystem : SystemBase
    {
        protected override void OnCreate() { }

        protected override void OnDestroy() { }

        protected override void OnUpdate()
        {
            Random randomGen = new Random(1);
            NativeArray<float> randomNumbers = new NativeArray<float>(1000, Allocator.TempJob);

            Job.WithCode(() =>
            {
                for (int i = 0; i < randomNumbers.Length; i++)
                {
                    randomNumbers[i] = randomGen.NextFloat();
                }
            }).Schedule();

            CompleteDependency();

            Entities
                .WithName("Copy") // Shown in error messages and profiler
                .ForEach((
                    ref DynamicBuffer<RandomDataBuffer> buffer) =>
                {
                    var bufferCount = buffer.Length;

                    for (int i = 0; i < bufferCount && i < randomNumbers.Length; i++)
                    {
                        buffer[i] = new RandomDataBuffer() { RandomValue = randomNumbers[i] };
                    }
                })
                .WithBurst(FloatMode.Fast, FloatPrecision.Low, synchronousCompilation: true) // Use when code is supported by Burst (refer to docs)
                .Run();

            randomNumbers.Dispose();
        }
    }
}
