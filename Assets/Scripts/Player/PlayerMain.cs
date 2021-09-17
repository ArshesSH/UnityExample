using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    public float speed;
    const float fTurnSpeed = 30.0f;
    const float fJumpPower = 10.0f;
    float fXAxis;
    float fYAxis;
    bool bIsShiftOn;
    bool bIsSpacebarOn;
    public bool bIsJumpNow = false;

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
        fXAxis = Input.GetAxisRaw("Horizontal");
        fYAxis = Input.GetAxisRaw("Vertical");
        bIsShiftOn = Input.GetButton("Sprint");
        bIsSpacebarOn = Input.GetButton("Jump");
    }

    void Move()
    {
        vMove = new Vector3(fXAxis, 0, fYAxis).normalized;
        transform.position += vMove * speed * (bIsShiftOn ? 1.7f : 1f) * Time.deltaTime;

        animPlayer.SetBool("bIsMoving", vMove != Vector3.zero);
        animPlayer.SetBool("bIsSprint", bIsShiftOn);
    }
    
    void Turn()
    {
        transform.LookAt(transform.position + vMove);
    }

    void Jump()
    {
        if(bIsSpacebarOn && !bIsJumpNow)
        {
            rigid.AddForce(Vector3.up * fJumpPower, ForceMode.Impulse);
            bIsJumpNow = true;
        }
    }

    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            bIsJumpNow = false;
        }
    }
    
}
