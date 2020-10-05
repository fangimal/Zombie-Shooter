using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public Joystick joystick;

    private Rigidbody rb;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(moveInput * moveSpeed, rb.velocity.y, moveInput * moveSpeed);

        //if (facinRight == false && moveSpeed > 0)
        //{
        //    Flip();
        //}
        //else if (facinRight == true && moveSpeed < 0)
        //{
        //    Flip();
        //}
        //if (moveSpeed == 0) //Если игрок не двигается
        //{
        //    anim.SetBool("IsRunning", false);
        //}
        //else
        //{
        //    anim.SetBool("IsRunning", true);
        //}
    }
}
