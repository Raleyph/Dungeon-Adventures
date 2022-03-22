using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atack : MonoBehaviour {
    public PlayerController Sam;
    
    private void OnTriggerStay(Collider other) {
        if (other.tag == "Skeleton") {
            if (Input.GetMouseButtonDown(0)) {
                other.GetComponent<Skeleton>().Damage(1);
            }
        }
    }
}
