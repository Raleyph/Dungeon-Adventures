using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    private GameObject Player;
    private PlayerController Sam;

    private void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
        Sam = Player.GetComponent<PlayerController>();
    }

    public void HealthPotion() {
        Sam.Buy("HealthPotion", 15);
    }
}
