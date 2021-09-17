using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    public float speed;
    const float turnSpeed = 30.0f;
    const float jumpPower = 10.0f;
    float xAxis;
    float yAxis;
    bool isShiftOn;
    bool isSpacebarOn;
    bool isJumpNow = false;

    Vector3 vMove;
    Rigidbody rigid;
    Animator animPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animPlayer = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
    }

    /*----------------------------------------------------------------------------------*/
    void GetInput()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        isShiftOn = Input.GetButton("Sprint");
        isSpacebarOn = Input.GetButton("Jump");
    }

    void Move()
    {
        vMove = new Vector3(xAxis, 0, yAxis).normalized;
        transform.position += vMove * speed * (isShiftOn ? 1.7f : 1f) * Time.deltaTime;

        animPlayer.SetBool("isMoving", vMove != Vector3.zero);
        animPlayer.SetBool("isSprint", isShiftOn);
    }
    
    void Turn()
    {
        transform.LookAt(transform.position + vMove);
    }

    void Jump()
    {
        if(isSpacebarOn && !isJumpNow)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJumpNow = true;
        }
    }

    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            isJumpNow = false;
        }
    }
    
}
