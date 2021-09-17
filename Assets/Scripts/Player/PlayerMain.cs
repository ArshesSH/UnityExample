using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    public float speed;
    const float turnSpeed = 30.0f;
    const float jumpPower = 12.0f;
    float xAxis;
    float yAxis;
    bool isShiftOn;
    bool isSpacebarOn;
    bool isJumpNow;
    bool isDodgeOn;

    Vector3 vMove;
    Vector3 dodgeVec;
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
        Dodge();
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
        if(isDodgeOn)
        {
            vMove = dodgeVec;
        }
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
        if(isSpacebarOn && vMove == Vector3.zero && !isJumpNow && !isDodgeOn)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            animPlayer.SetBool("isJump", true);
            animPlayer.SetTrigger("doJump");
            isJumpNow = true;
        }
    }

    void Dodge()
    {
        if (isSpacebarOn && vMove != Vector3.zero && !isJumpNow && !isDodgeOn)
        {
            dodgeVec = vMove;
            speed *= 2.0f;
            animPlayer.SetTrigger("doDodge");
            isDodgeOn = true;

            Invoke("DodgeOut", 0.4f);
        }
    }

    void DodgeOut()
    {
        isDodgeOn = false;
        speed *= 0.5f;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            animPlayer.SetBool("isJump", false);
            isJumpNow = false;
        }
    }
    
}
