using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public Sprite DefaultCell;
    public Sprite SelectedCell;
    public Text[] Labels;
    public Thing[] Things;
    public Cell[] Cells;
    public GameObject Overlay;
    
    public bool full = false;
    
    private Dictionary<int, Thing> Items = new Dictionary<int, Thing>();
    private int cellIndex;
    private int id;

    [System.Serializable]
    public class Thing {
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

        private string itemName;
        private int itemBroken;

        public void SetCell(string item, int broken, Sprite icon) {
            Item.enabled = true;
            Item.sprite = icon;
            itemName = item;
            itemBroken = broken;
            busy = true;
        }

        public void ClearCell() {
            Item.enabled = false;
            Item.sprite = null;
            itemName = null;
            itemBroken = 0;
            busy = false;
        }
    }

    public void AddToInventory(int itemType) {
        id += 1;
        Items.Add(id, Things[itemType]);

        if (!full) {
            if (!Cells[0].busy) {
                Cells[0].SetCell(Things[itemType].itemName, Things[itemType].broken, Things[itemType].Icon);
            } else {
                if (!Cells[1].busy) {
                    Cells[1].SetCell(Things[itemType].itemName, Things[itemType].broken, Things[itemType].Icon);
                } else {
                    if (!Cells[2].busy) {
                        Cells[2].SetCell(Things[itemType].itemName, Things[itemType].broken, Things[itemType].Icon);
                        full = true;
                    }
                }
            }
        }
    }

    public void RemoveFromInventory(int cell) {
        Cells[cell].ClearCell();
        full = false;
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
                }
                else cellIndex = 2;
            }
            
            for (int i = 0; i < Cells.Length; i++) {
                if (Cells[i].isActive && i != cellIndex) {
                    Cells[i].isActive = false;
                    Cells[i].ThisCell.sprite = DefaultCell;
                }
            }

            Cells[cellIndex].isActive = true;
            Cells[cellIndex].ThisCell.sprite = SelectedCell;
        }
    }
}
