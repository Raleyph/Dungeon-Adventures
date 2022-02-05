using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : MonoBehaviour {
    public GameObject Explosive;
    
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Destroy(gameObject);
            Instantiate(Explosive, gameObject.transform.position, Quaternion.identity);
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().Damage(1);
        }
    }
}
