using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    public GameObject MainMenu;
    public GameObject Overlay;
    public GameObject Camera;
    public GameObject PauseMenu;
    public GameObject NextLevelmenu;
    public GameObject LooseMenu;
    public GameObject AdosMenu;

    public Slider HealthBar;

    private GameObject Player;

    private bool isStarted = false;
    private bool isPaused = false;
    private bool isLoosed = false;
    
    private void Start() {
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false && isStarted && isLoosed == false) {
            Pause();
        } else if (Input.GetKeyDown(KeyCode.Escape) && isPaused) {
            Resume();
        }

        if (isStarted) {
            HealthBar.value = Player.GetComponent<PlayerController>().health;
        }
    }
    
    public void Play() {
        Cursor.visible = false;
        Time.timeScale = 1;

        MainMenu.SetActive(false);
        Overlay.SetActive(true);
        isStarted = true;
        Camera.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
        Player = GameObject.FindWithTag("Player");
    }

    public void Pause() {
        Cursor.visible = true;
        Time.timeScale = 0;
        isPaused = true;
        
        PauseMenu.SetActive(true);
        Overlay.SetActive(false);
    }
    
    public void Resume() {
        Cursor.visible = false;
        Time.timeScale = 1;
        isPaused = false;
        
        PauseMenu.SetActive(false);
        Overlay.SetActive(true);
    }
    
    public void OpenLevelMenu() {
        Cursor.visible = true;
        Time.timeScale = 0;
        
        NextLevelmenu.SetActive(true);
        Overlay.SetActive(false);
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
        Cursor.visible = false;
        Time.timeScale = 1;
        
        Destroy(GameObject.FindWithTag("Player"));
        var rooms = GameObject.FindGameObjectsWithTag("Room");

        for (int i = 0; i < rooms.Length; i++) {
            Destroy(rooms[i]);
        }
        
        NextLevelmenu.SetActive(false);
        Overlay.SetActive(true);
        GetComponent<DungeonGenerator>().NewLayer();
    }

    public void NotNewLayer() {
        Cursor.visible = false;
        Time.timeScale = 1;
        
        NextLevelmenu.SetActive(false);
        Overlay.SetActive(true);
    }

    public void Loose() {
        Cursor.visible = true;
        
        isLoosed = true;
        isStarted = false;
        Overlay.SetActive(false);
        LooseMenu.SetActive(true);
        Player.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void AdosMenus() {
        Cursor.visible = true;

        AdosMenu.SetActive(true);
        Overlay.SetActive(false);
        isPaused = true;
    }

    public void AdosMenusClose() {
        Cursor.visible = false;

        AdosMenu.SetActive(false);
        Overlay.SetActive(true);
        isPaused = false;
    }
}
