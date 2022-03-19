using System;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour {
    [SerializeField]
    private NavMeshAgent Agent;
    
    private GameObject Player;

    private void Start() {
        Player = GameObject.FindWithTag("Player");
    }

    void Update() {
        Agent.SetDestination(Player.transform.position);

        if (Agent.transform) {
            Agent.GetComponent<Animator>().SetBool("Move", true);
        } else {
            Agent.GetComponent<Animator>().SetBool("Move", false);
        }
    }
}
