using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerController2 : MonoBehaviour
{
    public SelectControl selectControl; //Выбираем вид управления
    public enum SelectControl  { Joystic, Keyboard }

    public LayerMask GroundLayer = 1;
    public Joystick joystick;
    public Transform gunBarrel;

    public float moveSpeed = 1f;
    public float jumpForce = 1f;
    public byte bulletCount;
    
    Rigidbody rb;
    new CapsuleCollider collider;
    Vector3 moveDirection; //MovementVector() вспомогательная переменная
    Animator anim;
    Shot shot;
    EnemyDetect enemyDetect;

    private float rotationSpeed = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        shot = FindObjectOfType<Shot>();
        enemyDetect = GetComponent<EnemyDetect>();

        anim = GetComponentInChildren<Animator>();
        
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        //  Защита от дурака
        if (GroundLayer == gameObject.layer)
            Debug.LogError("Слой сортировки игроков должен отличаться от слоя сортировки Земли!");
    }
    void Update()
    {
        AnimLogic();
        TurnLogic();
    }

    void FixedUpdate()
    {
        MoveLogic();
        ShotLogic();
        //JumpLogic();  
    }

    void AnimLogic() //Правила переключения анимации
    {
        if (rb.velocity.magnitude > 1.5f && bulletCount > 0)
            anim.SetBool("walkattack", true);
        else if (rb.velocity.magnitude < 1f)
            anim.SetBool("walkattack", false);
        else if (rb.velocity.magnitude > 1f && bulletCount == 0)
            anim.SetBool("walk", true);
    }

    void ShotLogic() //Стрельба
    {
        if (bulletCount > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                bulletCount--;
                Vector3 from = gunBarrel.position;
                Vector3 target = enemyDetect.targetEnemy.transform.position;

                Vector3 to = new Vector3(target.x, from.y, target.z);

                Vector3 direction1 = (to - from).normalized;

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
    }
    void TurnLogic() //Поворот 
    {
        //Если есть враг поворачивает к нему
        if (enemyDetect.targetEnemy != null)
        {
            Vector3 direction = enemyDetect.targetEnemy.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        else  //Иначе в сторону движения
        {
            transform.LookAt(moveDirection + transform.position);
        }
    }

    bool IsGrounded
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

    Vector3 MovementVector
    {
        get
        {
            float horizontal, vertical;
            if (selectControl == 0)
            {
                horizontal = joystick.Horizontal;
                vertical = joystick.Vertical;
                moveDirection = new Vector3(horizontal, 0.0f, vertical);
                return moveDirection;
            }
            else 
            {
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
                moveDirection = new Vector3(horizontal, 0.0f, vertical);
                return new Vector3(horizontal, 0.0f, vertical);
            }
        }
    }
 
    void MoveLogic()
    {
        rb.AddForce(MovementVector * moveSpeed, ForceMode.Impulse);
    }

    void JumpLogic()
    {
        if (IsGrounded && (Input.GetAxis("Jump") > 0))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}

