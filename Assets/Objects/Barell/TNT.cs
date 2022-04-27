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
            other.GetComponent<PlayerController>().Damage(50);
            GetComponent<BoxCollider>().enabled = false;
        }
        
        if (other.tag == "Enemy") {
            Explosive.Play();
            Destroy(Barell);
            other.GetComponent<Enemy>().Damage(50);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
