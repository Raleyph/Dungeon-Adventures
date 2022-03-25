using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoomBehaviour : MonoBehaviour {
    public enum enemyTypes {
        Skeleton,
        Holem,
        Ork,
        All
    };

    public GameObject[] walls; // 0 - Up 1 - Down 2 - Right 3 - Left
    public GameObject[] doors;
    public GameObject[] Enemy;
    public Transform[] Spawners;
    
    public enemyTypes EnemyType = enemyTypes.Skeleton;
    
    public string NameRoom;
    public bool HaveSpawner;
    
    private GameObject DungeonController;
    private Menu Menu;

    private bool isActiveRoom;
    private int SpawnerType;
    private List<Enemy> EnemiesInRoom = new List<Enemy>();

    private void Awake() {
        DungeonController = GameObject.FindWithTag("GameController");
        Menu = DungeonController.GetComponent<Menu>();
    }

    private void Start() {
        if (EnemyType == enemyTypes.Skeleton) {
            SpawnerType = 0;
        } else if (EnemyType == enemyTypes.Holem) {
            SpawnerType = 1;
        } else if (EnemyType == enemyTypes.Ork) {
            SpawnerType = 2;
        }
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
                    Menu.NameRoom(NameRoom);
                }
                if (HaveSpawner) {
                    isActiveRoom = true;
                    StartCoroutine(Spawn());
                    if (EnemiesInRoom.Count != 0) {
                        for (int i = 0; i < EnemiesInRoom.Count; i++) {
                            EnemiesInRoom[i].Active();
                        }
                    }
                }
            }
            if (other.tag == "Enemy") {
                EnemiesInRoom.Add(other.GetComponent<Enemy>());
                other.GetComponent<Enemy>().Active();
            }
        }
    }

    private void Update() {
        if (isActiveRoom) {
            if (EnemiesInRoom.Count != 0) {
                for (int i = 0; i < EnemiesInRoom.Count; i++) {
                    if (!EnemiesInRoom[i]) {
                        EnemiesInRoom.RemoveAt(i);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (Time.timeScale == 1) {
            if (other.tag == "Player" && HaveSpawner) {
                isActiveRoom = false;
                if (EnemiesInRoom.Count != 0) {
                    for (int i = 0; i < EnemiesInRoom.Count; i++) {
                        EnemiesInRoom[i].Deactive();
                    }
                }
            }
        }
    }

    private IEnumerator Spawn() {
        while (isActiveRoom) {
            Instantiate(Enemy[SpawnerType], Spawners[Random.Range(0, Spawners.Length)].position, Quaternion.identity);
            Menu.PlaySound("Spawn");
            yield return new WaitForSeconds(5f);
        }
    }
}
