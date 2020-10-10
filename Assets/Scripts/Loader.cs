using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject manager;

    void Awake()
    {
        if (EnemyManager.instanse == null)
        {
            Instantiate(manager);
        }
    }
}
