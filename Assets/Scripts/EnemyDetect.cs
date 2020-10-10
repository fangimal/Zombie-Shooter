using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDetect : MonoBehaviour
{
    public bool enemyIsNear = false; //Враг рядом
    public bool enemyLive = false;   //
    public float attackRadius = 10f; //*4 Минимальное расстояние до противника

    float attackCounter; //*4 Счётчик для стрельбы

    public Text nearest;
    Enemy targetEnemy = null; //*4 Враг - Цель
    

    GameObject[] enemy;
    public GameObject closest; //ближайший враг
    void Start()
    {
        enemy = GameObject.FindGameObjectsWithTag("Enemy"); 
    }

    void Update()
    {
        EnemyLive();
        if (enemyLive)
        {
            float distance = Vector3.Distance(FindClosesEnemy().transform.position, transform.position);
            nearest.text = closest.name + " " + distance.ToString(); //Пишем имя врага
        }
        else nearest.text = "Врагов нет!";
    }

    private List<Enemy> GetEnemiesInRange() //Какие Противники в зоне поражения
    {
        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach (Enemy enemy in EnemyManager.Instance.EnemyList) 
        { 
            if(Vector3.Distance(transform.position, enemy.transform.position) <= attackRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }

        return enemiesInRange;
    }
    private Enemy GetNearestEnemy() //Ближайший враг для атаки
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity; //Минимальное расстояние врага, его и будем атаковать

        foreach (Enemy enemy in GetEnemiesInRange())
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < smallestDistance)
            {
                smallestDistance = Vector3.Distance(transform.position, enemy.transform.position);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }




    GameObject FindClosesEnemy() //Поиск ближайшего врага
    {
        float distance = Mathf.Infinity;  //Mathf.Infinity
        Vector3 position = transform.position;
        foreach (GameObject go in enemy)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    public bool EnemyLive() // есть ли враги
    {
        if (closest == null || GameObject.FindGameObjectWithTag("Enemy"))
        {
            FindClosesEnemy();
            return enemyLive = true;
        }
        return enemyLive = false;
    }
    public void EnemyIsNear() //Проверяем близко ли враг
    {
        if (GameObject.FindGameObjectsWithTag("Enemy") == null) enemyIsNear = false;
        else
        {
            FindClosesEnemy();
            float distance = Vector3.Distance(closest.transform.position, transform.position); //Дистанция до врага
                                                                                               //GameObject.FindGameObjectsWithTag("Enemy").Length > 0
            if (distance < attackRadius)
            {
                enemyIsNear = true;
            }
            else enemyIsNear = false;
        }
    }
}
