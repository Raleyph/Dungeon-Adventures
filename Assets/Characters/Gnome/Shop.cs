using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    private PlayerController Sam;

    private void Start() {
        Sam = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void HealthPotion() {
        if (PlayerPrefs.GetInt("Coins") >= 15) Sam.Buy("Health Potion", 15);
    }
}
