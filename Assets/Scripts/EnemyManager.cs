using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instanse = null; //*4 EnemySpawner появляется не сразу
    //public float Period;
    //public GameObject Enemy;

    public GameObject spawnPoint; //*4 Где должны появляться противники
    public GameObject[] enemies; //*4
    public int maxEnemiesOfScreen; //*4 Максимально на сцене в один момент могут быть врагов
    public int totalEnemies; //*4 Всего за уровень будут врагов
    public int enemiesPerSpawn; //*4 Сколько одновременно могут появляться враги

    public List<Enemy> EnemyList = new List<Enemy>(); //Собирает всех врагов в Список

    //int enemiesOnScreen = 0; //*4 Изначальное количество врагов
    const float spawnDelay = 0.5f; //*4

    float TimeUntilNextSpawn;

    void Awake() //*4
    {
        if (instanse == null) instanse = this; //*4 Создать этот элемент
        else if (instanse != this) Destroy(gameObject); //Если менеджер не должен создоваться то созданный новый менеджер должен удалится 

        DontDestroyOnLoad(gameObject); //При загрузке менеджер должен оставаться на сцене
    }
    void Start()
    {
        //TimeUntilNextSpawn = Random.Range(0, Period);
        Spawn();
    }

    
    void Update()
    {
        //TimeUntilNextSpawn -= Time.deltaTime;
        //if (TimeUntilNextSpawn <= 0.0f)
        //{
        //    TimeUntilNextSpawn = Period;
        //    Instantiate(Enemy, transform.position, transform.rotation);
        //}
    }

    IEnumerator Spawn() //*4
    {
        if (enemiesPerSpawn> 0 && EnemyList.Count < totalEnemies)
        {
            for (int i =0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < maxEnemiesOfScreen)
                {
                    GameObject newEnemy = Instantiate(enemies[1]);
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
            Destroy(enemy.gameObject);
        }

        EnemyList.Clear(); //Очищаем список для создания нового
    }
}
