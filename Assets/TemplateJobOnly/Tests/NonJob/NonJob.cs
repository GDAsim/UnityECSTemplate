using UnityEngine;
using Unity.Mathematics;

public class Move_Normal : MonoBehaviour
{
    public int SpawnSize = 10000;

    [SerializeField] GameObject prefab;

    GameObject[] gos;

    void Awake()
    {
        gos = new GameObject[SpawnSize];
        for (int i = 0; i < SpawnSize; i++)
        {
            GameObject go = Instantiate(prefab);
            go.SetActive(true);
            gos[i] = go;
        }
    }
    void Update()
    {
        var time = Time.time;

        for (int index = 0; index < gos.Length; index++)
        {
            var pos = transform.position;

            var y = math.sin(time + index) * math.sqrt(index);
            var x = math.cos(time + index) * math.sqrt(index);

            float2 fx = new float2(x, 0);
            float2 fy = new float2(0, y);

            var dist = math.distance(fx, fy);
            var z = math.abs(dist + math.log(index + 1));

            //Move
            gos[index].transform.position = new Vector3(x, y, z);
        }
    }
}