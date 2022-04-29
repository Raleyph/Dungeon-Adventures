using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public Sprite DefaultCell;
    public Sprite SelectedCell;
    public Items[] Things;
    public Cell[] Cells;
    public GameObject Overlay;
    public PlayerController Sam;

    public bool full = false;
    
    private int cellIndex;

    [System.Serializable]
    public class Cell {
        public Image ThisCell;
        public Image Item;
        public bool isActive;
        public bool busy;

        public string itemType;
        public string itemName;
        public float value;

        public void SetCell(string type, string item, Sprite icon, float itemValue) {
            Item.enabled = true;
            Item.sprite = icon;
            itemType = type;
            itemName = item;
            value = itemValue;
            busy = true;
        }

        public void ClearCell() {
            Item.enabled = false;
            Item.sprite = null;
            itemType = null;
            itemName = null;
            value = 0f;
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
        string itemType = null;
        string name = null;
        Sprite icon = null;
        float value = 0f;
        
        for (int i = 0; i < Things.Length; i++) {
            if (Things[i].itemName == itemName) {
                itemType = Things[i].ItemTypes.ToString();
                name = Things[i].itemName;
                icon = Things[i].Icon;

                switch (itemType) {
                    case "Weapon":
                        value = Things[i].damage;
                        break;
                    case "Potion":
                        value = Things[i].healthOfset;
                        break;
                    case "Amulets":
                        value = Things[i].amuletValue;
                        break;
                }
            }
        }
        
        if (!full) {
            if (!Cells[0].busy) {
                Cells[0].SetCell(itemType, name, icon, value);
            } else {
                if (!Cells[1].busy) {
                    Cells[1].SetCell(itemType, name, icon, value);
                } else {
                    if (!Cells[2].busy) {
                        Cells[2].SetCell(itemType, name, icon, value);
                        full = true;
                    }
                }
            }
        }
    }

    public void UseItem(int cell) {
        switch (Cells[cell].itemName) {
            case "Health Potion":
                Sam.HealthPotion();
                Cells[cell].ClearCell();
                break;
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
