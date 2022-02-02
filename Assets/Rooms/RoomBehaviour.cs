using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomBehaviour : MonoBehaviour {
    public GameObject[] walls; // 0 - Up 1 - Down 2 - Right 3 - Left
    public GameObject[] doors;
    private GameObject RoomName;
    
    private Spawner Spawner;
    
    public string NameRoom;

    private void Awake() {
        RoomName = GameObject.FindWithTag("Room Name");
    }

    public void UpdateRoom(bool[] status) {
        Spawner = GameObject.FindObjectOfType<Spawner>();
        
        for (int i = 0; i < status.Length; i++) {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            RoomName.GetComponent<Text>().text = NameRoom;
        }
    }
/*
    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player") Spawner.DoSpawn();
    }

    private void OnTriggerExit(Collider other) {
        throw new NotImplementedException();
    }*/
}
