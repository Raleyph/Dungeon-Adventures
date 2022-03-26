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
    public Sprite[] Things;
    public Cell[] Cells;
    public GameObject Overlay;
    
    private Dictionary<string, int> Items = new Dictionary<string, int>();
    private int cellIndex;

    [System.Serializable]
    public class Cell {
        public Image ThisCell;
        public bool isActive;

        private string itemName;
        private int itemCount;
        

        public void SetCell(string item, int count, Sprite thing) {
            ThisCell.sprite = thing;
            itemName = item;
            itemCount = count;
        }
    }

    public void AddToInventory(string item, int count) {
        Items.Add(item, count);
    }

    public void SetInventory() {
        //Cells[cellIndex].SetCell(Things[0]);
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
            } else if (Input.GetKeyDown(KeyCode.Alpha0)) {
                
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
