using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Altar : MonoBehaviour {
    public string[] Amulets;
    
    private PlayerController Sam;

    private void Start() {
        InitAltar();
    }

    public void InitAltar() {
        Sam = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    
    private void OnTriggerStay(Collider other) {
        if (Time.timeScale == 1) {
            if (other.tag == "Player") {
                if (Input.GetKeyDown(KeyCode.E)) {
                    GetAmulet();
                }
            }
        }
    }

    public void GetAmulet() {
        if (PlayerPrefs.GetInt("Coins") >= 35) {
            Sam.Buy(Amulets[Random.Range(0, Amulets.Length)], 35);
        
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Light>().enabled = false;
        }
    }
}
