using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoomBehaviour : MonoBehaviour {
    public GameObject[] walls; // 0 - Up 1 - Down 2 - Right 3 - Left
    public GameObject[] doors;
    public GameObject[] Enemy;
    private GameObject DungeonController;
    public Transform[] Spawners;

    public string NameRoom;
    public int SpawnerType;
    public bool HaveSpawner;
    public bool isActiveRoom;

    private List<Skeleton> SkeletonsInRoom = new List<Skeleton>();

    private void Awake() {
        DungeonController = GameObject.FindWithTag("GameController");
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
                if (DungeonController) {
                    DungeonController.GetComponent<Menu>().NameRoom(NameRoom);
                }
                
                if (HaveSpawner) {
                    StartCoroutine(Spawn());
                    isActiveRoom = true;
                }
            }

            if (other.tag == "Skeleton") {
                SkeletonsInRoom.Add(other.GetComponent<Skeleton>());
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (Time.timeScale == 1) {
            if (other.tag == "Player" && HaveSpawner) {
                StopCoroutine(Spawn());
                isActiveRoom = false;
            }
        }
    }

    IEnumerator Spawn() {
        Instantiate(Enemy[SpawnerType], Spawners[Random.Range(0, Spawners.Length)].position, Quaternion.identity);
        DungeonController.GetComponent<Menu>().PlaySound("Spawn");
        yield return new WaitForSeconds(1f);
    }

    private void Update() {
        if (SkeletonsInRoom.Count != 0) {
            if (isActiveRoom) {
                for (int i = 0; i < SkeletonsInRoom.Count; i++) {
                    if (SkeletonsInRoom[i]) {
                        SkeletonsInRoom[i].GetComponent<Skeleton>().isActive = true;
                    } else {
                        SkeletonsInRoom.RemoveAt(i);
                    }
                }
            } else {
                for (int i = 0; i < SkeletonsInRoom.Count; i++) {
                    if (SkeletonsInRoom[i]) {
                        SkeletonsInRoom[i].GetComponent<Skeleton>().isActive = false;
                    } else {
                        SkeletonsInRoom.RemoveAt(i);
                    }
                }
            }
        }
    }
}
