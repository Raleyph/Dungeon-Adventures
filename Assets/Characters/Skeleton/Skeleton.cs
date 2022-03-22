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
        } else {
            Agent.GetComponent<Animator>().SetBool("Move", false);
        }

        if (health <= 0) {
            Death();
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

    private void Death() {
        Destroy(gameObject);
    }
}
