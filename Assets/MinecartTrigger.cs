using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecartTrigger : MonoBehaviour {
    private GameObject Player;
    public PlayerController Sam;

    private void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
        Sam = Player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (other.tag == "Minecart") {
                Sam.Death();
            }
        }
    }
}
