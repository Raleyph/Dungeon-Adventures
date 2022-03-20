using System;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour {
    [SerializeField]
    private NavMeshAgent Agent;
    
    private GameObject Player;
    private bool isActive;

    private void Start() {
        Player = GameObject.FindWithTag("Player");
    }

    void Update() {
        if (isActive) {
            Agent.SetDestination(Player.transform.position);

            if (Agent.transform) {
                Agent.GetComponent<Animator>().SetBool("Move", true);
            } else {
                Agent.GetComponent<Animator>().SetBool("Move", false);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        isActive = true;
    }

    private void OnTriggerExit(Collider other) {
        isActive = false;
    }
}
