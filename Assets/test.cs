using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    public GameObject b;
    
    void Update() {
        transform.LookAt(b.transform);
    }
}
