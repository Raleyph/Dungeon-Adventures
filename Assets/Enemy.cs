using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {
    [SerializeField]
    private NavMeshAgent Agent;
    
    public enum enemyTypes {
        Skeleton,
        Holem,
        Ork
    };
    
    public enemyTypes EnemyType = enemyTypes.Skeleton;
    
    private float health;
    private int damage;
    
    public float atackDistance;
    public bool active;

    private GameObject Player;
    private PlayerController Sam;
    private Menu Menu;

    public Animator Animator;
    public GameObject Canvas;
    public Slider Healthbar;

    private void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
        Sam = Player.GetComponent<PlayerController>();
        
        Menu = GameObject.FindWithTag("GameController").GetComponent<Menu>();
        Animator.SetFloat("Random", Random.value);

        if (EnemyType == enemyTypes.Skeleton) {
            health = 100;
            damage = 10;
            atackDistance = 2f;
        } else if (EnemyType == enemyTypes.Holem) {
            health = 200;
            damage = 20;
            atackDistance = 2f;
        } else if (EnemyType == enemyTypes.Ork) {
            health = 150;
            damage = 20;
        }
    }

    private void Update() {
        if (active && !Menu.isLoosed) {
            if (Vector3.Distance(transform.position, Player.transform.position) >= atackDistance) {
                var position = Player.transform.position;
                
                Agent.enabled = true;
                Agent.SetDestination(position);
                
                Animator.SetBool("Move", true);
                Sam.look = false;
                transform.LookAt(new Vector3(position.x, transform.position.y, position.z));
            } else {
                Animator.SetTrigger("Atack");
            }
        } else {
            Sam.look = false;
            Agent.enabled = false;
            Animator.SetBool("Move", false);
        }
        
        if (Canvas) {
            Healthbar.value = health;
        }
        
        if (health <= 0) {
            Destroy(gameObject);
            Sam.coins += 5;
            PlayerPrefs.SetInt("Coins", Sam.coins);
            Menu.PlaySound("DeathEnemy");
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            Sam.LookAtEnemy(gameObject);
            Sam.look = true;
        }
    }

    public void Active() {
        active = true;
        Canvas.SetActive(true);
    }

    public void Deactive() {
        active = false;
    }

    public void Damage(float damage) {
        health -= damage;
        Menu.PlaySound("DamageEnemy");
    }

    public void Atack() {
        Sam.Damage(damage);
    }
}
