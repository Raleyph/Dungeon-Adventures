using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using UnityEngine;

public class LayerConroller : MonoBehaviour {
    public Menu DungeonController;

    private void Start() {
        DungeonController = GameObject.FindObjectOfType<Menu>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            DungeonController.OpenLevelMenu();
        }
    }
}
