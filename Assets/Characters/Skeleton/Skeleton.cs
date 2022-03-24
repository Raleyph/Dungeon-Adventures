using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour {
    [SerializeField]
    private NavMeshAgent Agent;
    
    private GameObject Player;
    public bool isActive;

    public int health;

    public Animator Anim;
    public GameObject Canvas;
    public Slider HealthBar;

    private void Start() {
        Player = GameObject.FindWithTag("Player");
        
        health = 100;
    }

    void Update() {
        if (isActive) {
            if (Vector3.Distance(transform.position, Player.transform.position) >= 2) {
                Agent.enabled = true;
                Agent.SetDestination(Player.transform.position);
                Anim.SetBool("Move", true);
                
                transform.LookAt(Player.transform);
            } else {
                Agent.enabled = false;
                Anim.SetBool("Move", false);
            }

            if (Canvas) {
                HealthBar.value = health;
            }
        } else {
            Agent.enabled = false;
            Anim.SetBool("Move", false);
        }

        if (health <= 0) {
            Death();
        }
    }

    public void Active() {
        isActive = true;
        Canvas.SetActive(true);
    }

    public void Deactive() {
        isActive = false;
    }

    public void Damage(int type) {
        switch (type) {
            case 1:
                health -= 25;
                break;
        }
    }

    private void Death() {
        Destroy(gameObject);
    }
}
