using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public Transform gunBarrel;

    NavMeshAgent navMeshAgent;
    Cursor cursor;
    Shot shot;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        cursor = FindObjectOfType<Cursor>();
        navMeshAgent.updateRotation = false;

        shot = FindObjectOfType<Shot>();
    }


    void Update()
    {
        //Что бы игрок поворачивался в сторону курсора
        Vector3 forward = cursor.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(forward.x, 0, forward.z));

        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
            dir.x = 5.0f;
        if (Input.GetKey(KeyCode.RightArrow))
            dir.x = -5.0f;
        if (Input.GetKey(KeyCode.UpArrow))
            dir.z = -5.0f;
        if (Input.GetKey(KeyCode.DownArrow))
            dir.z = 2.0f;
        navMeshAgent.velocity = dir.normalized * moveSpeed;

        if (Input.GetMouseButtonDown(0))
        {
            var from = gunBarrel.position;
            var target = cursor.transform.position;
            var to = new Vector3(target.x, from.y, target.z);
            var direction = (to - from).normalized;

            RaycastHit hit;
            if (Physics.Raycast(from, to - from, out hit, 100))
                to = new Vector3(hit.point.x, from.y, hit.point.z);
            else
                to = from + direction * 100;

            if (hit.transform != null)
            {
                var zombie = hit.transform.GetComponent<Enemie>();
                if (zombie != null)
                    zombie.Kill();
            }

            shot.Show(from, to);
        }
    }
}
