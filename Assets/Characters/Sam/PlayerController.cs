using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private Vector3 moveSpeed;

    public int health = 0;
    public int armor;
    public int coins;

    private bool isArmored = false;
    
    public float MaxMoveSpeed = 8;
    private float dashingTimeLeft;

    private GameObject Menu;
    private CharacterController controllerComponent;
    private Animator Anim;

    private void Start() {
        Menu = GameObject.FindWithTag("GameController");
        controllerComponent = GetComponent<CharacterController>();
        
        rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();

        if (PlayerPrefs.GetInt("Health") <= 0) {
            health = 100;
            PlayerPrefs.SetInt("Health", health);
        } else {
            health = PlayerPrefs.GetInt("Health");
        }
        
    }

    private void Update() {
        UpdateWalk();
        
        if (Input.GetKeyDown(KeyCode.LeftShift)) Dash(false);
        else if (Input.GetKey(KeyCode.LeftShift)) Dash(true);

        if (moveSpeed.x != 0 || moveSpeed.z != 0) {
            Anim.SetBool("Move", true);
        } else {
            Anim.SetBool("Move", false);
        }

        if (health <= 0 && Time.timeScale == 1) {
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
        if (isArmored == false || armor == 0) {
            switch (type) {
                case 1:
                    health -= 5;
                    break;
                case 2:
                    health -= 10;
                    break;
                case 3:
                    health -= 15;
                    break;
                case 4:
                    health -= 20;
                    break;
                case 5:
                    health -= 50;
                    break;
            }
            
            PlayerPrefs.SetInt("Health", health);
        }
        
        Menu.GetComponent<Menu>().PlaySound("Damage");
    }

    public void Death() {
        Menu.GetComponent<Menu>().Loose();
        this.enabled = false;
    }

    public void HealthPosion() {
        if (health <= 75) {
            health += 25;
        } else {
            Debug.Log("Здоровый, блять, как пельмень!");
        }
    }

    public void ArmorSet() {
        
    }
}
