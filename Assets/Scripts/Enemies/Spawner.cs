using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public float spawnRadius = 5f;
    public float angleMin = 0f;
    public float angleMax = 360f;

    public bool autospawn = true;
    public float spawnrate = .5f;

    [SerializeField] GameObject prefab;
    [SerializeField] Transform spawnContainer;

    private float lastSpawn = 0f;

	public void Spawn(int number = 1)
    {
        if (lastSpawn + spawnrate < Time.time)
        {
            for(var i = 0; i < number; i++)
            {
                Instantiate(prefab, RandomCircle(spawnContainer.position, spawnRadius), spawnContainer.transform.rotation);
            }
            lastSpawn = Time.time;
        }
    }

    public void BeginSpawning()
    {
        autospawn = true;
    }

    public void StopSpawning()
    {
        autospawn = false;
    }

    private void Update()
    {
        if (autospawn)
        {
            Spawn();
        }
    }

    //
    // Thanks @robertbu  http://answers.unity.com/answers/714853/view.html
    Vector3 RandomCircle(Vector3 point, float radius)
    {
        float angle = Random.Range(angleMin, angleMax);
        Vector3 output;
        output.x = point.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        output.z = point.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        output.y = point.y;
        return output;
    }
}
