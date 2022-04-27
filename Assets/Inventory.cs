using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public Sprite DefaultCell;
    public Sprite SelectedCell;
    public Thing[] Things;
    public Cell[] Cells;
    public GameObject Overlay;
    public PlayerController Sam;

    public bool full = false;
    
    private int cellIndex;

    [System.Serializable]
    public class Thing {
        public enum itemType {
            Weapon,
            Potions,
            Amulets
        };

        public itemType ItemType = itemType.Weapon;
        public Sprite Icon;
        public string itemName;
        public int broken;
    }

    [System.Serializable]
    public class Cell {
        public Image ThisCell;
        public Image Item;
        public bool isActive;
        public bool busy;

        public string itemType;
        public string itemName;
        public int itemBroken;

        public void SetCell(string type, string item, int broken, Sprite icon) {
            Item.enabled = true;
            Item.sprite = icon;
            itemType = type;
            itemName = item;
            itemBroken = broken;
            busy = true;
        }

        public void ClearCell() {
            Item.enabled = false;
            Item.sprite = null;
            itemType = null;
            itemName = null;
            itemBroken = 0;
            busy = false;
        }
    }
    
    public void InitInventory(bool newLayer) {
        if (newLayer) {
            AddToInventory("Default Sword");
        }
        Sam = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
        for (int i = 0; i < Cells.Length; i++) {
            if (Cells[i].isActive) {
                Cells[i].isActive = false;
            }
        }
        Cells[0].isActive = true;
    }

    public void AddToInventory(string itemName) {
        int itemType = 0;

        switch (itemName) {
            case "Health Potion":
                itemType = 0;
                break;
            case "Default Sword":
                itemType = 1;
                break;
            case "Axe":
                itemType = 2;
                break;
        }
        
        if (!full) {
            if (!Cells[0].busy) {
                Cells[0].SetCell(Things[itemType].ItemType.ToString(), Things[itemType].itemName, Things[itemType].broken, Things[itemType].Icon);
            } else {
                if (!Cells[1].busy) {
                    Cells[1].SetCell(Things[itemType].ItemType.ToString(), Things[itemType].itemName, Things[itemType].broken, Things[itemType].Icon);
                } else {
                    if (!Cells[2].busy) {
                        Cells[2].SetCell(Things[itemType].ItemType.ToString(), Things[itemType].itemName, Things[itemType].broken, Things[itemType].Icon);
                        full = true;
                    }
                }
            }
        }
    }

    public void UseItem(int cell) {
        if (Cells[cell].itemType == "Potions") {
            switch (Cells[cell].itemName) {
                case "Health Potion":
                    Sam.HealthPotion();
                    break;
            }
            Cells[cell].ClearCell();
        }
    }

    public void ClearInventory() {
        for (int i = 0; i < Cells.Length; i++) {
            Cells[i].ClearCell();
            full = false;
        }
    }

    private void Update() {
        if (Overlay) {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
                if (cellIndex < 2) {
                    cellIndex++;
                } else cellIndex = 0;
            } else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
                if (cellIndex > 0) {
                    cellIndex--;
                } else cellIndex = 2;
            }
            
            for (int i = 0; i < Cells.Length; i++) {
                if (Cells[i].isActive && i != cellIndex) {
                    Cells[i].isActive = false;
                    Cells[i].ThisCell.sprite = DefaultCell;
                } else if (Cells[i].isActive) {
                    if (Cells[i].itemType == "Weapon") {
                        if (Sam) {
                            Sam.SetWeapon(Cells[i].itemName);
                        }
                    }
                }
            }

            Cells[cellIndex].isActive = true;
            Cells[cellIndex].ThisCell.sprite = SelectedCell;
            

            if (Input.GetKeyDown(KeyCode.F)) {
                for (int g = 0; g < Cells.Length; g++) {
                    if (Cells[g].isActive && Cells[g].busy) {
                        UseItem(g);
                    }
                }
            }
        }
    }
}
