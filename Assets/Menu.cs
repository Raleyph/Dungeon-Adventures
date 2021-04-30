using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    public GameObject MainMenu;
    public GameObject Overlay;
    public GameObject Camera;
    
    private void Start()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        Overlay.SetActive(false);
    }
    
    public void Play()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        MainMenu.SetActive(false);
        Overlay.SetActive(true);

        Camera.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
    }
}
