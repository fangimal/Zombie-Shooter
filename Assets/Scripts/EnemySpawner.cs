using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float Period;
    public GameObject Enemy;
    float TimeUntilNextSpawn;
    void Start()
    {
        TimeUntilNextSpawn = Random.Range(0, Period);
    }

    
    void Update()
    {
        TimeUntilNextSpawn -= Time.deltaTime;
        if (TimeUntilNextSpawn <= 0.0f)
        {
            TimeUntilNextSpawn = Period;
            Instantiate(Enemy, transform.position, transform.rotation);
        }
    }
}
