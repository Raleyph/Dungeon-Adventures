using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomBehaviour : MonoBehaviour {
    public GameObject[] walls; // 0 - Up 1 - Down 2 - Right 3 - Left
    public GameObject[] doors;
    private GameObject RoomName;

    public string NameRoom;

    private void Awake() {
        RoomName = GameObject.FindWithTag("Room Name");
    }

    public void UpdateRoom(bool[] status) {
        for (int i = 0; i < status.Length; i++) {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (Time.timeScale == 1) {
            if (other.tag == "Player") {
                RoomName.GetComponent<Text>().text = NameRoom;
            }
        }
    }
}
