using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private Vector3 moveSpeed;
    private GameObject DungeonController;
    private Menu Menu;
    private CharacterController controllerComponent;
    private Animator Anim;

    public int health = 100;
    public int armor = 100;
    public int coins = 0;

    public Enemy Enemies;

    public bool look;

    private float maxMoveSpeed = 8;
    private float dashingTimeLeft;

    private void Start() {
        DungeonController = GameObject.FindWithTag("GameController");
        Menu = DungeonController.GetComponent<Menu>();
        controllerComponent = GetComponent<CharacterController>();
        
        rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();

        if (PlayerPrefs.GetInt("Health") <= 0) {
            health = 100;
            PlayerPrefs.SetInt("Health", health);
        } else {
            health = PlayerPrefs.GetInt("Health");
        }
        
        if (PlayerPrefs.GetInt("Armor") <= 0) {
            armor = 100;
            PlayerPrefs.SetInt("Armor", armor);
        } else {
            armor = PlayerPrefs.GetInt("Armor");
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

        if (Time.timeScale == 1) {
            if (health <= 0) {
                Death();
            }
            if (Input.GetMouseButtonDown(0)) {
                StartCoroutine(Atack(1));
            }
        }
    }

    private void UpdateWalk() {
        float ySpeed = moveSpeed.y;
        moveSpeed.y = 0;
        
        if (dashingTimeLeft <= 0) {
            Vector3 target = maxMoveSpeed * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            moveSpeed = Vector3.MoveTowards(moveSpeed, target, Time.deltaTime * 300);
            if (moveSpeed.magnitude > 0.1f) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveSpeed), Time.deltaTime * 720);
            }
        } else {
            moveSpeed = maxMoveSpeed * 5 * moveSpeed.normalized;
        }
        
        dashingTimeLeft -= Time.deltaTime;

        moveSpeed.y = ySpeed + Physics.gravity.y * Time.deltaTime;
        controllerComponent.Move(moveSpeed * Time.deltaTime);
    }
    
    private void Dash(bool holding) {
        if (dashingTimeLeft < (holding ? -.4f : -.2f)) {
            dashingTimeLeft = .1f;
            Menu.PlaySound("Dash");
        }
    }

    public void Damage(int type) {
        if (armor >= 0) {
            switch (type) {
                case 1:
                    health -= 2;
                    break;
                case 2:
                    health -= 5;
                    break;
                case 3:
                    health -= 10;
                    break;
                case 4:
                    health -= 15;
                    break;
                case 5:
                    health -= 30;
                    break;
            }
            armor -= 5;
        }

        PlayerPrefs.SetInt("Health", health);
        PlayerPrefs.SetInt("Armor", armor);
        Menu.PlaySound("Damage");
    }

    public void Death() {
        Menu.Loose();
        this.enabled = false;
    }

    public void LookAtEnemy(GameObject Enemy) {
        transform.LookAt(Enemy.transform, Vector3.up);
    }

    public IEnumerator Atack(int type) {
        Anim.SetTrigger("Atack");
        if (Enemies && look) {
            if (Vector3.Distance(transform.position, Enemies.transform.position) <= Enemies.atackDistance) {
                Enemies.Damage(1);
            }
        }

        yield return new WaitForSeconds(0.5f);
    }
    
    public IEnumerator GetDamage(int type) {
        while (true) {
            Damage(type);
            yield return new WaitForSeconds(1f);
        }
    }

    public void Buy(string item, int price) {
        if (PlayerPrefs.GetInt("Coins") >= price) {
            coins -= price;
            PlayerPrefs.SetInt("Coins", coins);
        } else {
            Debug.Log("Ну как там с деньгами?");
        }

        if (item == "HealthPotion") {
            HealthPoison();
        }
    }

    public void HealthPoison() {
        if (health <= 75) {
            health += 25;
            PlayerPrefs.SetInt("Health", health);
        } else {
            //Inventory.Add("HealthPotion", 1);
        }
    }
}
