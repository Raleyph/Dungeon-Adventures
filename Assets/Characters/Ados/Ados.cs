using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ados : MonoBehaviour {
    private GameObject Menu;
    private Animator Anim;

    private void Start() {
        Anim = GetComponent<Animator>();
    }

    public void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            Anim.SetBool("isActive", true);
            
            if (Input.GetKeyDown(KeyCode.R)) {
                Menu = GameObject.Find("Dungeon Controller");
                Menu.GetComponent<Menu>().AdosMenus();
            }
        }
    }
}
