using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ados : MonoBehaviour {
    private GameObject AdosMenu;

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (Input.GetKeyDown(KeyCode.R)) {
                AdosMenu = GameObject.FindWithTag("Ados Menu");
                AdosMenu.SetActive(true);
            }
        }
    }
}
