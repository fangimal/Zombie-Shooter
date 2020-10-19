using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    PlayerController2 player;
    CapsuleCollider capsuleCollider;
    Animator animator;
    MovementAnimator movementAnimator;
    EnemyManager enemyManager;

    [SerializeField]
    int helth;

    bool dead = false;

    Transform enemy;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController2>();
        navMeshAgent.updateRotation = false;

        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();
        movementAnimator = GetComponent<MovementAnimator>();
        enemyManager = FindObjectOfType<EnemyManager>();

        enemy = GetComponent<Transform>();
        EnemyManager.Instance.RegisterEnemy(this); //Регистрируем противников в листе
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == false) 
        {
            navMeshAgent.SetDestination(player.transform.position);
            transform.rotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
        }
        
    }
    public void Kill()
    {
        if (!dead)
        {
            dead = true;
            Destroy(capsuleCollider);
            Destroy(movementAnimator);
            Destroy(navMeshAgent);
            GetComponentInChildren<ParticleSystem>().Play();
            animator.SetTrigger("died");
            enemyManager.DestroyEnemies();
            enemyManager.UnregisterEnemy(this);
            //Destroy(gameObject, 2);
        }
    }

    public void EnemyHit(int hitPoints)
    {
        if(helth - hitPoints > 0)
        {
            helth -= hitPoints;
            Kill();
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        dead = true;
    }
}
