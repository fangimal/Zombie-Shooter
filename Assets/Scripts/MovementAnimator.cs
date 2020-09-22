using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementAnimator : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Animator animator;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    
    void Update()
    {
        animator.SetFloat("speed", navMeshAgent.velocity.magnitude);
    }
}
