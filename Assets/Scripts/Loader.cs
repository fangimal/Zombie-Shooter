using UnityEngine;

public class Loader <T> : MonoBehaviour where T: MonoBehaviour //*6
{
    private static T instance; //*4 EnemySpawner появляется не сразу

    public static T Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<T>(); //*4 Создать этот элемент
            else if (instance != FindObjectOfType<T>()) Destroy(FindObjectOfType<T>()); //Если менеджер не должен создоваться то созданный новый менеджер должен удалится 

            DontDestroyOnLoad(FindObjectOfType<T>()); //При загрузке менеджер должен оставаться на сцене

            return instance;
        }
    }

    //public GameObject manager;

    //void Awake()
    //{
    //    if (EnemyManager.instanse == null)
    //    {
    //        Instantiate(manager);
    //    }
    //}
    
}
