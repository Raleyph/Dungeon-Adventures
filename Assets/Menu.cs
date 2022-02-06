using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    public GameObject MainMenu;
    public GameObject Overlay;
    public GameObject Camera;
    public GameObject PauseMenu;
    public GameObject NextLevelmenu;

    private bool isStarted = false;
    private bool isPaused = false;
    
    private void Start() {
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false && isStarted) {
            Pause();
            isPaused = true;
        } else if (Input.GetKeyDown(KeyCode.Escape) && isPaused) {
            Resume();
            isPaused = false;
        }
    }
    
    public void Play() {
        Cursor.visible = false;
        Time.timeScale = 1;
        MainMenu.SetActive(false);
        Overlay.SetActive(true);
        isStarted = true;
        Camera.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().SetBar();
    }

    public void Pause() {
        Cursor.visible = true;
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        Overlay.SetActive(false);
    }
    
    public void Resume() {
        Cursor.visible = false;
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        Overlay.SetActive(true);
    }
    
    public void OpenLevelMenu() {
        NextLevelmenu.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ToMenu() {
        Cursor.visible = true;
        Time.timeScale = 0;
        isStarted = false;
        MainMenu.SetActive(true);
        PauseMenu.SetActive(false);
        Overlay.SetActive(false);
    }
    
    public void DoNewLayer() {
        Destroy(GameObject.FindWithTag("Player"));
        var rooms = GameObject.FindGameObjectsWithTag("Room");

        for (int i = 0; i < rooms.Length; i++) {
            Destroy(rooms[i]);
        }
        
        NextLevelmenu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
        
        GetComponent<DungeonGenerator>().NewLayer();
    }

    public void NotNewLayer() {
        NextLevelmenu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
    }
}
