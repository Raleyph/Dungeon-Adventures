using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aphaleon : MonoBehaviour {
    private GameObject Menu;
    private Animator Anim;

    private void Start() {
        Anim = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            if (Input.GetKeyDown(KeyCode.E)) {
                Menu = GameObject.Find("Dungeon Controller");
                Menu.GetComponent<Menu>().AphaleonMenus();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            Menu = GameObject.Find("Dungeon Controller");
            Menu.GetComponent<Menu>().AphaleonMenusClose();
        }
    }
}
