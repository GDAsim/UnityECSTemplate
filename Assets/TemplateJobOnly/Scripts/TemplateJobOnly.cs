namespace TemplateJobOnly
{
    using Unity.Burst;
    using Unity.Jobs;
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.Jobs;

    public class TemplateJobOnly : MonoBehaviour
    {
        public int SpawnSize = 10000;

        [SerializeField] GameObject prefab;

        TransformAccessArray transformArray;

        void Awake()
        {
            transformArray = new TransformAccessArray(SpawnSize);

            for (int i = 0; i < SpawnSize; i++)
            {
                GameObject go = Instantiate(prefab);
                go.SetActive(true);

                transformArray.Add(go.transform.GetInstanceID());
            }
        }
        void OnDestroy()
        {
            transformArray.Dispose();
        }

        void Update()
        {
            //Run 3 Jobs in Sequence with dependency
            var sinMoveJob = new SinMoveJob()
            {
                time = Time.time,
            };

            var sinMoveJobHandle = sinMoveJob.Schedule(transformArray);

            var cosMoveJob = new CosMoveJob()
            {
                time = Time.time,
            };

            var cosMoveJobHandle = cosMoveJob.Schedule(transformArray, sinMoveJobHandle);

            var zMoveJob = new ZMoveJob()
            {
                time = Time.time,
            };

            zMoveJob.Schedule(transformArray, cosMoveJobHandle);
        }
    }

    /// <summary>
    /// Job to modify y position
    /// </summary>
    [BurstCompile]
    public struct SinMoveJob : IJobParallelForTransform
    {
        public float time;

        public void Execute(int index, TransformAccess transform)
        {
            var pos = transform.position;

            pos.y = math.sin(time + index) * math.sqrt(index);

            transform.position = pos;
        }
    }

    /// <summary>
    /// Job to modify x position
    /// </summary>
    [BurstCompile]
    public struct CosMoveJob : IJobParallelForTransform
    {
        public float time;

        public void Execute(int index, TransformAccess transform)
        {
            var pos = transform.position;

            pos.x = math.cos(time + index) * math.sqrt(index);

            transform.position = pos;
        }
    }

    /// <summary>
    /// Job to modify z position
    /// </summary>
    [BurstCompile]
    public struct ZMoveJob : IJobParallelForTransform
    {
        public float time;

        public void Execute(int index, TransformAccess transform)
        {
            var pos = transform.position;

            float2 fx = new float2(pos.x, 0);
            float2 fy = new float2(0, pos.y);

            var dist = math.distance(fx, fy);
            pos.z = math.abs(dist + math.log(index + 1));

            transform.position = pos;
        }
    }
}

