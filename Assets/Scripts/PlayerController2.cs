using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotationSpeed = 5f;
    public float JumpForce = 1f;
    //public string nearest; //Имя ближайшего врага

    private Rigidbody rb;
    private new CapsuleCollider collider; 

    GameObject[] enemy;
    GameObject closest; //ближайший враг
    Animator animator;
    Shot shot;

    public LayerMask GroundLayer = 1;
    public Joystick joystick;
    public Transform gunBarrel; 


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        shot = FindObjectOfType<Shot>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        //  Защита от дурака
        if (GroundLayer == gameObject.layer)
            Debug.LogError("Слой сортировки игроков должен отличаться от слоя сортировки Земли!");
    }
    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0) 
        {
            TurnToTheEnemy();
        }


        //transform.rotation = Quaternion.LookRotation(rb.velocity.normalized);

        //transform.rotation =Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        //nearest = FindClosesEnemy().name; //Пишем имя врага

        animator.SetFloat("speed", rb.velocity.magnitude);

        //Стрельба
        if (Input.GetMouseButtonDown(0))
        {
            var from = gunBarrel.position;
            var target = enemy[0].transform.position;
            var to = new Vector3(target.x, from.y, target.z);

            var direction1 = (to - from).normalized;

            RaycastHit hit;
            if (Physics.Raycast(from, to - from, out hit, 100))
                to = new Vector3(hit.point.x, from.y, hit.point.z);
            else
                to = from + direction1 * 100;

            if (hit.transform != null)
            {
                var zombie = hit.transform.GetComponent<Enemy>();
                if (zombie != null)
                    zombie.Kill();
            }
            shot.Show(from, to);
        }

    }
    void FixedUpdate()
    {
        JumpLogic();
        MoveLogic();
    }

    void TurnToTheEnemy() //Поворот к врагу
    {
        Vector3 direction = FindClosesEnemy().transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
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
    private bool IsGrounded
    {
        get
        {
            var bottomCenterPoint = new Vector3(collider.bounds.center.x, collider.bounds.min.y, collider.bounds.center.z);

            //создаем невидимую физическую капсулу и проверяем не пересекает ли она обьект который относится к полу

            //_collider.bounds.size.x / 2 * 0.9f -- эта странная конструкция берет радиус обьекта.
            // был бы обязательно сферой -- брался бы радиус напрямую, а так пишем по-универсальнее

            return Physics.CheckCapsule(collider.bounds.center, bottomCenterPoint, collider.bounds.size.x / 2 * 0.9f, GroundLayer);
            // если можно будет прыгать в воздухе, то нужно будет изменить коэфициент 0.9 на меньший.
        }
    }

    private Vector3 MovementVector
    {
        get
        {
            float horizontal = joystick.Horizontal;
            float vertical = joystick.Vertical;

            //float horizontal = Input.GetAxis("Horizontal");
            //float vertical = Input.GetAxis("Vertical");

            return new Vector3(horizontal, 0.0f, vertical);
        }
    }


    private void MoveLogic()
    {
        rb.AddForce(MovementVector * moveSpeed, ForceMode.Impulse);
    }
    private void JumpLogic()
    {
        if (IsGrounded && (Input.GetAxis("Jump") > 0))
        {
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }
}

