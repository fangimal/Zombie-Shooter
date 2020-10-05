using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotationSpeed = 5f;
    public float JumpForce = 1f;
    public string nearest; //Имя ближайшего врага

    private Rigidbody rb;
    private new CapsuleCollider collider; 

    GameObject[] enemy;
    GameObject closest; //ближайший враг
    Animator animator;

    public LayerMask GroundLayer = 1;
    public Joystick joystick;
    //public Transform gunBarrel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();

        enemy = GameObject.FindGameObjectsWithTag("Enemy");

        //т.к. нам не нужно что бы персонаж мог падать сам по-себе без нашего на то указания.
        //то нужно заблочить поворот по осях X и Z
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        //  Защита от дурака
        if (GroundLayer == gameObject.layer)
            Debug.LogError("Player SortingLayer must be different from Ground SourtingLayer!");
    }
    private void Update()
    {
        Vector3 direction = FindClosesEnemy().transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        //transform.rotation = Quaternion.LookRotation(rb.velocity.normalized);

        //transform.rotation =Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        nearest = FindClosesEnemy().name;

        animator.SetFloat("speed", rb.velocity.magnitude);
        
    }
    void FixedUpdate()
    {
        JumpLogic();
        MoveLogic();
    }

    GameObject FindClosesEnemy()
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

