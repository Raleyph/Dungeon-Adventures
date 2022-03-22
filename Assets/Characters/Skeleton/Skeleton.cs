using System;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour {
    [SerializeField]
    private NavMeshAgent Agent;
    
    private GameObject Player;
    public bool isActive;

    public int health;

    private void Start() {
        Player = GameObject.FindWithTag("Player");
        health = 100;
    }

    void Update() {
        if (isActive) {
            Agent.SetDestination(Player.transform.position);
            Agent.GetComponent<Animator>().SetBool("Move", true);
            
            transform.LookAt(Player.transform);
        } else {
            Agent.GetComponent<Animator>().SetBool("Move", false);
        }

        if (health <= 0) {
            Death();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (Vector3.Distance(transform.position, other.transform.position) < 10f) {
                other.GetComponent<PlayerController>().Damage(2);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Room") {
            if (other.GetComponent<RoomBehaviour>().isActiveRoom == true) {
                isActive = true;
            } else {
                isActive = false;
            }
        }
    }

    public void Damage(int type) {
        switch (type) {
            case 1:
                health -= 5;
                break;
        }
    }

    private void Death() {
        Destroy(gameObject);
    }
}
