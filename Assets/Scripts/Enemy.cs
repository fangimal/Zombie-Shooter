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

    bool dead;

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
        EnemyManager.Instance.RegisterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) return;

        navMeshAgent.SetDestination(player.transform.position);
        transform.rotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
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
            //enemyManager.UnregisterEnemy();
            enemyManager.DestroyEnemies();
           // Destroy(gameObject, 2);
        }
    }
}
