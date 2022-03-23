using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atack : MonoBehaviour {
    public PlayerController Sam;
    public Animator SamAnim;

    private void OnTriggerStay(Collider other) {
        if (Input.GetMouseButtonDown(0)) {
            if (other.tag == "Skeleton") {
                other.GetComponent<Skeleton>().Damage(1);
            }
            SamAnim.SetTrigger("Atack");
        }
    }
}
