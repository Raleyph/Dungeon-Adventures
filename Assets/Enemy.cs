using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    [SerializeField]
    private NavMeshAgent Agent;
    
    public enum enemyTypes {
        Skeleton,
        Holem,
        Ork
    };
    
    public enemyTypes EnemyType = enemyTypes.Skeleton;
    
    private int health;
    private int damageType;
    private float atackDistance;
    
    public bool active;

    private GameObject Player;
    private PlayerController Sam;

    public Animator Animator;
    public GameObject Canvas;
    public Slider Healthbar;

    private void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
        Sam = Player.GetComponent<PlayerController>();

        if (EnemyType == enemyTypes.Skeleton) {
            health = 100;
            damageType = 1;
            atackDistance = 2f;
        } else if (EnemyType == enemyTypes.Holem) {
            health = 200;
            damageType = 2;
            atackDistance = 3f;
        } else if (EnemyType == enemyTypes.Ork) {
            health = 150;
            damageType = 3;
        }
    }

    private void Update() {
        if (active) {
            if (Vector3.Distance(transform.position, Player.transform.position) >= atackDistance) {
                Agent.enabled = true;
                Agent.SetDestination(Player.transform.position);
                Animator.SetBool("Move", true);
                Sam.look = false;
                
                transform.LookAt(Player.transform, Vector3.up);
            } else {
                Agent.enabled = false;
                Animator.SetBool("Move", false);
                Animator.SetTrigger("Atack");
                
                Sam.LookAtEnemy(gameObject);
                Sam.look = true;
                Sam.Enemies = this;
            }
            if (Canvas) {
                Healthbar.value = health;
            }
        } else {
            Sam.look = false;
            Agent.enabled = false;
            Animator.SetBool("Move", false);
        }
        if (health <= 0) {
            Death();
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Sam.StartCoroutine("GetDamage", damageType);
            Sam.LookAtEnemy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            Sam.StopCoroutine("GetDamage");
        }
    }
    
    public void Active() {
        active = true;
        Canvas.SetActive(true);
    }

    public void Deactive() {
        active = false;
    }
    
    public void Damage(int type) {
        switch (type) {
            case 1:
                health -= 25;
                break;
            case 2:
                health -= 50;
                break;
        }
    }

    private void Death() {
        Sam.StopCoroutine("GetDamage");
        Destroy(gameObject);
    }
}
