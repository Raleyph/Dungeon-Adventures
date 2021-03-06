using System;
using UnityEngine;

public class Ados : MonoBehaviour {
    private GameObject Menu;
    private Animator Anim;

    private void Start() {
        Anim = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            Anim.SetBool("isActive", true);
            
            if (Input.GetKeyDown(KeyCode.E)) {
                Menu = GameObject.Find("Dungeon Controller");
                Menu.GetComponent<Menu>().AdosMenus();
            }
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            Menu = GameObject.Find("Dungeon Controller");
            Menu.GetComponent<Menu>().AdosMenusClose();
        }
    }
}
