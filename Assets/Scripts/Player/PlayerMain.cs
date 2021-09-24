using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    Vector3 vMove;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator animPlayer;

    GameObject nearObject;
    GameObject equipWeapon;

    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;

    public int ammoCurrent;
    public int coinCurrent;
    public int healthCurrent;
    public int granadesCurrent;

    public int ammoMax;
    public int coinMax;
    public int healthMax;
    public int granadesMax;


    const float turnSpeed = 30.0f;
    const float jumpPower = 12.0f;
    float xAxis;
    float yAxis;
    int currentWeaponIndex = -1;

    // Key State
    bool ButtonSprint;
    bool ButtonJump;
    bool ButtonInteractive;
    bool ButtonWeapon1;
    bool ButtonWeapon2;
    bool ButtonWeapon3;

    // Player State
    bool isJumpNow;
    bool isDodgeOn;
    bool isChangeWeaponNow;

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
        ChangeWeapon();
        Interaction();
    }

    /*----------------------------------------------------------------------------------*/
    void GetInput()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        ButtonSprint = Input.GetButton("Sprint");
        ButtonJump = Input.GetButtonDown("Jump");
        ButtonInteractive = Input.GetButtonDown("Interaction");
        ButtonWeapon1 = Input.GetButtonDown("Weapon1");
        ButtonWeapon2 = Input.GetButtonDown("Weapon2");
        ButtonWeapon3 = Input.GetButtonDown("Weapon3");
    }

    void Move()
    {
        vMove = new Vector3(xAxis, 0, yAxis).normalized;
        if(isDodgeOn)
        {
            vMove = dodgeVec;
        }

        if(isChangeWeaponNow)
        {
            vMove = Vector3.zero;
        }

        transform.position += vMove * speed * (ButtonSprint ? 1.7f : 1f) * Time.deltaTime;

        animPlayer.SetBool("isMoving", vMove != Vector3.zero);
        animPlayer.SetBool("isSprint", ButtonSprint);
    }
    
    void Turn()
    {
        transform.LookAt(transform.position + vMove);
    }

    void Jump()
    {
        if(ButtonJump && vMove == Vector3.zero && !isJumpNow && !isDodgeOn && !isChangeWeaponNow)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            animPlayer.SetBool("isJump", true);
            animPlayer.SetTrigger("doJump");
            isJumpNow = true;
        }
    }

    void Dodge()
    {
        if (ButtonJump && vMove != Vector3.zero && !isJumpNow && !isDodgeOn && !isChangeWeaponNow)
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

    void ChangeWeapon()
    {
        int weaponIndex = -1;
        if (ButtonWeapon1 && hasWeapons[0]) weaponIndex = 0;
        if (ButtonWeapon2 && hasWeapons[1]) weaponIndex = 1;
        if (ButtonWeapon3 && hasWeapons[2]) weaponIndex = 2;


        if ( (ButtonWeapon1 || ButtonWeapon2 || ButtonWeapon3) && !isJumpNow && !isDodgeOn && weaponIndex != -1 && currentWeaponIndex != weaponIndex )
        {
            if(equipWeapon != null)
            {
                equipWeapon.SetActive(false);
            }
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);

            currentWeaponIndex = weaponIndex;

            animPlayer.SetTrigger("doChangeWeapon");
            isChangeWeaponNow = true;
            Invoke("ChangeWeaponEnd", 0.4f);
        }
    }

    void ChangeWeaponEnd()
    {
        isChangeWeaponNow = false;
    }

    void Interaction()
    {
        if(ButtonInteractive && nearObject != null && !isJumpNow && !isDodgeOn && !isChangeWeaponNow)
        {
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            animPlayer.SetBool("isJump", false);
            isJumpNow = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }
        Debug.Log(nearObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
    }
}
