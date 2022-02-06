using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private Vector3 moveSpeed;

    public int health;
    public float MaxMoveSpeed = 8;
    public float JumpForce;
    private float dashingTimeLeft;

    private GameObject HealthBar;
    private GameObject LooseMenu;
    private CharacterController controllerComponent;
    private Animator Anim;

    private void Start() {
        controllerComponent = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        health = 100;
    }

    public void SetBar() {
        HealthBar = GameObject.FindWithTag("HealthBar");
    }

    private void FixedUpdate() {
        if (Input.GetKey(KeyCode.Space)) rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }

    private void Update() {
        UpdateWalk();
        
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.X)) Dash(false);
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.X)) Dash(true);

        if (moveSpeed.x != 0 || moveSpeed.z != 0) {
            Anim.SetBool("Move", true);
        } else {
            Anim.SetBool("Move", false);
        }

        if (HealthBar) {
            HealthBar.GetComponent<Slider>().value = health;
        }

        if (health <= 0) {
            Death();
        }
    }

    private void UpdateWalk() {
        float ySpeed = moveSpeed.y;
        moveSpeed.y = 0;
        
        if (dashingTimeLeft <= 0) {
            Vector3 target = MaxMoveSpeed * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            moveSpeed = Vector3.MoveTowards(moveSpeed, target, Time.deltaTime * 300);
            if (moveSpeed.magnitude > 0.1f) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveSpeed), Time.deltaTime * 720);
            }
        } else {
            moveSpeed = MaxMoveSpeed * 5 * moveSpeed.normalized;
        }
        
        dashingTimeLeft -= Time.deltaTime;

        moveSpeed.y = ySpeed + Physics.gravity.y * Time.deltaTime;
        controllerComponent.Move(moveSpeed * Time.deltaTime);
    }
    
    private void Dash(bool holding) {
        if (dashingTimeLeft < (holding ? -.4f : -.2f)) {
            dashingTimeLeft = .1f;
        }
    }

    public void Damage(int type) {
        if (type == 1) {
            health -= 5;
        } else if (type == 2) {
            health -= 10;
        } else if (type == 3) {
            health -= 15;
        } else if (type == 4) {
            health -= 20;
        }
    }

    public void Death() {
        Time.timeScale = 0;
        LooseMenu = GameObject.FindWithTag("Loose Menu");
        LooseMenu.SetActive(true);
    }
}
