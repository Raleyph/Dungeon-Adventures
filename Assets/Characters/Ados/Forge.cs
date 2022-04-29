using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forge : MonoBehaviour {
    private PlayerController Sam;

    private void Start() {
        Sam = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void FixArmor() {
        if (PlayerPrefs.GetInt("Coins") >= 20) Sam.Fix("Armor", 20);
    }
}
