using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TNT : MonoBehaviour {
    public ParticleSystem Explosive;
    public GameObject Barell;
    
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Explosive.Play();
            Destroy(Barell);
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().Damage(4);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
