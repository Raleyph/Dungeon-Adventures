using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private Vector3 moveSpeed;
    private Menu Menu;
    private Inventory Inventory;
    private CharacterController controllerComponent;
    private Animator Anim;

    public float health = 100;
    public float armor = 100;
    public int coins = 0;

    public bool look;

    private float maxMoveSpeed = 8;
    private float dashingTimeLeft;

    private int atackArmor;

    public GameObject[] Weapons;
    public PlayerAtack PlayerAtack;

    private void Start() {
        Menu = GameObject.FindWithTag("GameController").GetComponent<Menu>();
        Inventory = GameObject.FindWithTag("GameController").GetComponent<Inventory>();
        
        controllerComponent = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();

        if (PlayerPrefs.GetFloat("Health") <= 0) {
            health = 100;
        } else {
            health = PlayerPrefs.GetFloat("Health");
        }
        
        if (PlayerPrefs.GetFloat("Armor") <= 0) {
            armor = 100;
        } else {
            armor = PlayerPrefs.GetFloat("Armor");
        }
        SavePlayerData();
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

            if (Inventory.Sam == null) {
                Inventory.Sam = this;
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

    public void AutoJump() {
        moveSpeed.y = 0.5f;
    }

    private void Dash(bool holding) {
        if (dashingTimeLeft < (holding ? -.4f : -.2f)) {
            dashingTimeLeft = .1f;
            Menu.PlaySound("Dash");
        }
    }

    public void Damage(float damage) {
        if (armor != 0) {
            armor -= damage / 2;
            health -= damage / 6;
        } else {
            health -= damage;
        }

        SavePlayerData();
        Menu.PlaySound("Damage");
    }

    public void Death() {
        Menu.Loose();
        Inventory.ClearInventory();
        ClearPlayerData();
        this.enabled = false;
    }

    public void SetWeapon(string name) {
        int weaponNumber = 0;
        
        switch (name) {
            case "Default Sword":
                weaponNumber = 0;
                PlayerAtack.damage = 20;
                break;
            case "Axe":
                weaponNumber = 1;
                PlayerAtack.damage = 40;
                break;
        }

        for (int i = 0; i < Weapons.Length; i++) {
            if (i != weaponNumber) {
                Weapons[i].SetActive(false);
            } else {
                Weapons[weaponNumber].SetActive(true);
            }
        }
    }

    public void LookAtEnemy(GameObject Enemy) {
        var position = Enemy.transform.position;
        transform.LookAt(new Vector3(position.x, transform.position.y, position.z));
    }

    public void Buy(string item, int price) {
        coins -= price;
        SavePlayerData();

        if (!Inventory.full) {
            Inventory.AddToInventory(item);
        }
    }

    public void Fix(string item, int price) {
        coins -= price;
        SavePlayerData();

        switch (item) {
            case "Armor":
                armor = 100;
                break;
        }
    }

    public void HealthPotion() {
        if (PlayerPrefs.GetFloat("Health") <= 75) health += 25;
        else health = 100;
        SavePlayerData();
        Menu.PlaySound("PotionUse");
    }

    public void SavePlayerData() {
        PlayerPrefs.SetFloat("Health", health);
        PlayerPrefs.SetFloat("Armor", armor);
        PlayerPrefs.SetInt("Coins", coins);
    }

    public void ClearPlayerData() {
        PlayerPrefs.DeleteKey("Health");
        PlayerPrefs.DeleteKey("Armor");
        PlayerPrefs.DeleteKey("Coins");
    }
}
