using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    CharacterController CC;
    Animator anim;
    float speed = 6;
    float jumpSpeed = 8;
    float gravity = 20;

    void Awake()
    {
        CC = gameObject.GetComponent<CharacterController>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float hor = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        anim.SetBool("isMoving", hor != 0 || vert != 0);

        Vector3 direction = Vector3.zero;
        if (CC.isGrounded)
        {
            direction = new Vector3(hor, 0, vert);
            direction = transform.TransformDirection(direction);
            direction *= speed;
            if (Input.GetButton("Jump"))
            {
                direction.y = jumpSpeed;
            }
        }
        direction.y -= gravity * Time.deltaTime;
        CC.Move(direction * Time.deltaTime);
    }


}
