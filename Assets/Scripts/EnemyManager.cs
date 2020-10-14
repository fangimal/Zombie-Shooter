using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Loader<EnemyManager>
{
    //public float Period;
    //public GameObject Enemy;
    [SerializeField]
    GameObject spawnPoint; //*4 Где должны появляться противники
    [SerializeField] 
    GameObject[] enemies; //*4
    [SerializeField] 
    int maxEnemiesOfScreen; //*4 Максимально на сцене в один момент могут быть врагов
    [SerializeField] 
    int totalEnemies; //*4 Всего за уровень будут врагов
    [SerializeField] 
    int enemiesPerSpawn; //*4 Сколько одновременно могут появляться враги

    public List<Enemy> EnemyList = new List<Enemy>(); //Собирает всех врагов в Список

    const float spawnDelay = 0.5f; //*4

    float TimeUntilNextSpawn;


    void Start()
    {
        //TimeUntilNextSpawn = Random.Range(0, Period);
        spawnPoint = GameObject.FindWithTag("Respawn"); //
        StartCoroutine(Spawn());
    }

    void Update()
    {

    }

    IEnumerator Spawn() //*4
    {
        if (enemiesPerSpawn> 0 && EnemyList.Count < totalEnemies)
        {
            for (int i =0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < maxEnemiesOfScreen)
                {
                    GameObject newEnemy = Instantiate(enemies[0]);
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy) //Добавляем врагов в Лист
    {
        EnemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy) //Удаляем врагов из Листа
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyEnemies() //Уловие Уничтожения противников
    {
        foreach (Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject, 1);
        }

        EnemyList.Clear(); //Очищаем список для создания нового
    }
}
