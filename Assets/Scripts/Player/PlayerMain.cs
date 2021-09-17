using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    public float speed;
    float fTurnSpeed = 30f;
    float fXAxis;
    float fYAxis;
    bool bIsShiftOn;

    Vector3 vMove;
    Animator animPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        animPlayer = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        fXAxis = Input.GetAxisRaw("Horizontal");
        fYAxis = Input.GetAxisRaw("Vertical");
        bIsShiftOn = Input.GetButton("Sprint");

        vMove = new Vector3(fXAxis, 0, fYAxis).normalized;
        transform.position += vMove * speed * (bIsShiftOn ? 1.7f : 1f) * Time.deltaTime;
        transform.LookAt(transform.position + vMove);

        animPlayer.SetBool("bIsMoving", vMove != Vector3.zero);
        animPlayer.SetBool("bIsSprint", bIsShiftOn);
    }
}
