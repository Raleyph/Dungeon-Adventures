using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoomBehaviour : MonoBehaviour {
    public GameObject[] walls; // 0 - Up 1 - Down 2 - Right 3 - Left
    public GameObject[] doors;
    public GameObject[] Spawners;
    public GameObject[] Enemy;
    private GameObject RoomName;

    public string NameRoom;
    public int SpawnerType;
    public bool HaveSpawner;

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
                if (HaveSpawner) {
                    StartCoroutine(Spawn());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (Time.timeScale == 1) {
            if (other.tag == "Player" && HaveSpawner) {
                StopCoroutine(Spawn());
            }
        }
    }

    IEnumerator Spawn() {
        Instantiate(Enemy[SpawnerType], Spawners[Random.Range(0, Spawners.Length)].transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
    }
}
