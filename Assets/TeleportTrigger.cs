using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour {
    public GameObject Sam;
    public GameObject Zetnr;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("авыаыва");
        Sam.transform.position = Zetnr.transform.position;
    }
}
