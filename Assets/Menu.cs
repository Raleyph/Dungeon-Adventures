using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    [Header("UI Objects")]
    public GameObject MainMenu;
    public GameObject Overlay;
    public GameObject PauseMenu;
    public GameObject NextLevelmenu;
    public GameObject LooseMenu;
    public GameObject AdosMenu;

    [Header("Other")]
    public Slider HealthBar;
    public Text Level;
    public Text RoomName;
    public GameObject SpotLight;
    public GameObject Camera;

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

        if (HealthBar && Player) {
            HealthBar.value = Player.GetComponent<PlayerController>().health;
        }

        if (Level) {
            Level.text = GetComponent<DungeonGenerator>().Level.ToString();
        }
    }

    public void NameRoom(string room) {
        RoomName.text = room;
    }
    
    public void Play() {
        Cursor.visible = false;
        Time.timeScale = 1;

        SpotLight.SetActive(false);
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
        
        // destroy Sam
        Destroy(GameObject.FindWithTag("Player"));
        
        // destroy rooms
        var rooms = GameObject.FindGameObjectsWithTag("Room");
        for (int i = 0; i < rooms.Length; i++) {
            Destroy(rooms[i]);
        }

        // destroy skeleton`s
        var skeletons = GameObject.FindGameObjectsWithTag("Skeleton");
        for (int i = 0; i < skeletons.Length; i++) {
            Destroy(skeletons[i]);
        }
        
        GetComponent<DungeonGenerator>().AllRefreshGenerate();
        Camera.transform.position = new Vector3(3f, 1.5f, 3f);
        Camera.transform.rotation = Quaternion.Euler(0f, -135f, 0f);

        isStarted = false;
        isPaused = false;
        SpotLight.SetActive(true);
        MainMenu.SetActive(true);
        LooseMenu.SetActive(false);
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
        Player.GetComponent<PlayerController>().enabled = false;
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

    public void Exit() {
        Application.Quit();
    }

    public void GitHub() {
        Application.OpenURL("https://github.com/Raleyph/Dungeon-Adventures");
    }
}
