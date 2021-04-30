using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private Vector3 moveSpeed;
    public float MaxMoveSpeed = 8;
    public float JumpForce;
    private float dashingTimeLeft;

    private CharacterController controllerComponent;

    public string[] Animations;

    private void Start()
    {
        controllerComponent = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space)) rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }

    private void Update()
    {
        UpdateWalk();
        
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.X)) Dash(false);
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.X)) Dash(true);

        if (Input.GetMouseButton(0))
        {
            gameObject.GetComponent<Animation>().Play(Animations[UnityEngine.Random.Range(0, Animations.Length)]);
        }
    }

    private void UpdateWalk()
    {
        float ySpeed = moveSpeed.y;
        moveSpeed.y = 0;
        if (dashingTimeLeft <= 0)
        {
            Vector3 target = MaxMoveSpeed * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            moveSpeed = Vector3.MoveTowards(moveSpeed, target, Time.deltaTime * 300);

            if (moveSpeed.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveSpeed), Time.deltaTime * 720);
            }
        }
        else
        {
            moveSpeed = MaxMoveSpeed * 5 * moveSpeed.normalized;
        }

        dashingTimeLeft -= Time.deltaTime;

        moveSpeed.y = ySpeed + Physics.gravity.y * Time.deltaTime;
        controllerComponent.Move(moveSpeed * Time.deltaTime);
    }
    
    private void Dash(bool holding)
    {
        if (dashingTimeLeft < (holding ? -.4f : -.2f))
        {
            dashingTimeLeft = .1f;
        }
    }
}
