using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour{
    [System.Serializable]
    public class Chests {
        public GameObject Model;
        public float weight;
        public Drop[] ChestDrop;

        public List<string> Items = new List<string>();

        public void SetDrop() {
            for (int i = 0; i < ChestDrop.Length; i++) {
                if (UnityEngine.Random.value < ChestDrop[i].chance) {
                    Items.Add(ChestDrop[i].itemName);
                    return;
                }
            }
        }
    }

    [System.Serializable]
    public class Drop {
        public enum itemType {
            Weapon,
            Potions,
            Perkss
        };

        public itemType dropType = itemType.Weapon;
        public string itemName;
        public float chance;
    }

    [Range(0, 1)] public float AllChange = 0.5f;
    public Chests[] Types;
    public Inventory SamInventory;
    private int chestNum;

    private void Start() {
        SetChest();
    }

    public void SetChest() {
        SamInventory = GameObject.FindWithTag("GameController").GetComponent<Inventory>();
        
        if (UnityEngine.Random.value < AllChange) {
            for (int i = 0; i < Types.Length; i++) {
                var rand = Random.Range(0, Types[i].weight);

                if (rand < (Types[i].weight / 2)) {
                    Types[i].Model.SetActive(true);
                    Types[i].SetDrop();
                    chestNum = i;
                    return;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (Time.timeScale == 1) {
            if (other.tag == "Player") {
                if (Input.GetKeyDown(KeyCode.E)) {
                    OpenChest(SamInventory, Types[chestNum].Items);
                }
            }
        }
    }
    
    private void OpenChest(Inventory SamInv, List<string> items) {
        for (int i = 0; i < items.Count; i++) {
            SamInv.AddToInventory(items[i]);
        }

        GetComponent<BoxCollider>().enabled = false;
    }
}
