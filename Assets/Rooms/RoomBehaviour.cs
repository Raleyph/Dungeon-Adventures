using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoomBehaviour : MonoBehaviour
{
    public enum roomTypes {
        Room,
        Shop,
        Mine,
        Quarry,
        Rails,
        Prison,
        Altar,
        Storehouse,
        Forge,
        Chest,
        Potions
    };
    
    public enum enemyTypes {
        Skeleton,
        Holem,
        Ork,
        All
    };

    public roomTypes RoomType = roomTypes.Room;

    public GameObject[] walls; // 0 - Up 1 - Down 2 - Right 3 - Left
    public GameObject[] doors;
    public GameObject[] Enemy;
    public Transform[] Spawners;

    public enemyTypes EnemyType = enemyTypes.Skeleton;
    
    public bool haveSpawner;

    public float spawnTime;
    
    private GameObject DungeonController;
    private Menu Menu;

    private int killedCount = 0;
    private int killedCountMax;
    
    private bool isActiveRoom;
    private int SpawnerType;
    private List<Enemy> EnemiesInRoom = new List<Enemy>();
    
    private GameObject Player;
    private PlayerController Sam;

    private void Awake() {
        DungeonController = GameObject.FindWithTag("GameController");
        Menu = DungeonController.GetComponent<Menu>();
    }

    private void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
        Sam = Player.GetComponent<PlayerController>();

        killedCountMax = Random.Range(6, 12);

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
        if (other.tag == "Player") {
            Menu.NameRoom(RoomType.ToString());
            
            if (Time.timeScale == 1) {
                if (haveSpawner) {
                    isActiveRoom = true;
                    StartCoroutine(Spawn());
                
                    if (EnemiesInRoom.Count != 0) {
                        for (int i = 0; i < EnemiesInRoom.Count; i++) {
                            EnemiesInRoom[i].Active();
                        }
                    }
                }
            }
        }
        
        if (other.tag == "Enemy") {
            EnemiesInRoom.Add(other.GetComponent<Enemy>());
            other.GetComponent<Enemy>().Active();
        }
    }

    private void Update() {
        if (killedCount <= killedCountMax) {
            if (isActiveRoom & EnemiesInRoom.Count != 0) {
                for (int i = 0; i < EnemiesInRoom.Count; i++) {
                    if (!EnemiesInRoom[i]) {
                        EnemiesInRoom.RemoveAt(i);
                        killedCount += 1;
                    }
                }
            }
        } else {
            haveSpawner = false;
        }
    }
    

    private void OnTriggerExit(Collider other) {
        if (Time.timeScale == 1) {
            if (other.tag == "Player" && haveSpawner) {
                isActiveRoom = false;
                StopAllCoroutines();

                if (EnemiesInRoom.Count != 0) {
                    for (int i = 0; i < EnemiesInRoom.Count; i++) {
                        EnemiesInRoom[i].Deactive();
                    }
                }
            }
        }
    }

    public IEnumerator Spawn() {
        while (isActiveRoom && haveSpawner) {
            Instantiate(Enemy[SpawnerType], Spawners[Random.Range(0, Spawners.Length)].position, Quaternion.identity);
            Menu.PlaySound("Spawn");
            yield return new WaitForSeconds(5f);
        }
    }

    public IEnumerator DoneRoom() {
        yield return new WaitForSeconds(1f);
        
        int coins = Random.Range(5, 8);
        Sam.coins += coins;
        PlayerPrefs.SetInt("Coins", Sam.coins);
        Menu.PlaySound("Coin");
    }
}
