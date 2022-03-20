using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;
using Slider = UnityEngine.UI.Slider;

public class Menu : MonoBehaviour {
    [Header("UI Objects")]
    public GameObject MainMenu;
    public GameObject Overlay;
    public GameObject SettingsMenu;
    public GameObject PauseMenu;
    public GameObject NextLevelmenu;
    public GameObject LooseMenu;
    public GameObject AdosMenu;
    
    [Header("Sound Controller")]
    public AudioSource Controller;
    public AudioClip[] Sounds;
    public AudioMixer MasterVolume;
    public AudioMixerGroup SoundsMix;
    public AudioMixerGroup MusicMix;

    [Header("Settings & Etc")]
    public Slider HealthBar;
    public Text Level;
    public Text RoomName;
    public Slider MusicSlider;
    public Slider VolumeSlider;
    public Dropdown QualityDropdown;
    public PostProcessVolume Blur;
    public GameObject SpotLight;
    public GameObject Camera;
    
    Resolution[] resolutions;

    private GameObject Player;

    private bool isStarted = false;
    private bool isPaused = false;
    private bool isLoosed = false;

    private float soundVolume;
    private float musicVolume;
    private float masterVolume;

    private void Start() {
        Cursor.visible = true;
        Time.timeScale = 0;

        Controller = gameObject.GetComponent<AudioSource>();
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

    // Main Menu
    
    public void Play() {
        Cursor.visible = false;
        Time.timeScale = 1;

        SpotLight.SetActive(false);
        MainMenu.SetActive(false);
        Overlay.SetActive(true);
        isStarted = true;
        isLoosed = false;
        Camera.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
        Player = GameObject.FindWithTag("Player");
    }
    
    public void Exit() {
        Application.Quit();
    }

    public void GitHub() {
        Application.OpenURL("https://github.com/Raleyph/Dungeon-Adventures");
    }
    
    // Pause

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

    public void OpenSettings() {
        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void CloseSettings() {
        if (isPaused) {
            PauseMenu.SetActive(true);
        } else {
            MainMenu.SetActive(true);
        }
        
        SettingsMenu.SetActive(false);
    }
    
    // New Layer
    
    public void OpenLevelMenu() {
        Cursor.visible = true;
        Time.timeScale = 0;
        
        NextLevelmenu.SetActive(true);
        Overlay.SetActive(false);
        isPaused = true;
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
    
    // Etc

    public void Loose() {
        Cursor.visible = true;
        
        isLoosed = true;
        isStarted = false;
        Overlay.SetActive(false);
        LooseMenu.SetActive(true);
        Player.GetComponent<PlayerController>().enabled = false;
    }
    
    public void NameRoom(string room) {
        RoomName.text = room;
    }
    
    // Ados

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
    
    // Settings

    public void SetQuality(int index) {
        index = QualityDropdown.value;
        QualitySettings.SetQualityLevel(index, true);
    }

    public void SetResolution(int index) {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMotionBlur(bool blur) {
        Blur.GetComponent<MotionBlur>().active = blur;
    }

    public void SetFullscreen(bool fullscreen) {
        Screen.fullScreen = fullscreen;
    }
    
    // Sound Controller

    public void PlaySound(string sound) {
        switch (sound) {
            case "Tap":
                Controller.clip = Sounds[0];
                Controller.Play();
                break;
        }
    }
    
    public void SetMusicVolume(float volume) {
        MusicMix.audioMixer.SetFloat("Music", volume);
        musicVolume = volume;
    }

    public void SetSoundVolume(float volume) {
        SoundsMix.audioMixer.SetFloat("Sounds", volume);
        soundVolume = volume;
    }
}
