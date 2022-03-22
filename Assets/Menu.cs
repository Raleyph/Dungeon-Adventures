using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    [Header("UI Objects")]
    public GameObject MainMenu;
    public GameObject Overlay;
    public GameObject SettingsMenu;
    public GameObject PauseMenu;
    public GameObject NextLevelmenu;
    public GameObject LooseMenu;
    
    [Header("Character Menus")]
    public GameObject AdosMenu;
    public GameObject AphaleonMenu;
    public GameObject MichaelMenu;
    
    [Header("Sound Controller")]
    public AudioSource Controller;
    public AudioMixerGroup MasterVolume;
    public AudioClip[] Sounds;

    [Header("Settings & Etc")]
    public Slider HealthBar;
    public Text Level;
    public Text RoomName;
    public Slider MusicSlider;
    public Slider VolumeSlider;
    public Dropdown QualityDropdown;
    public PostProcessVolume PostProcess;
    public GameObject SpotLight;
    public GameObject Camera;

    Resolution[] resolutions;

    private GameObject Player;
    private DungeonGenerator DG;
    
    private MotionBlur Blur;
    private DepthOfField DOF;

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
        
        Blur = ScriptableObject.CreateInstance<MotionBlur>();
        DOF = ScriptableObject.CreateInstance<DepthOfField>();
        PostProcess.profile.TryGetSettings(out Blur);
        PostProcess.profile.TryGetSettings(out DOF);

        DG = GetComponent<DungeonGenerator>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false && isStarted && isLoosed == false) {
            Pause();
        } else if (Input.GetKeyDown(KeyCode.Escape) && isPaused) {
            Resume();
        } else if (Input.GetKeyDown(KeyCode.Space) && MainMenu) {
            Play();
        }

        if (HealthBar && Player) {
            HealthBar.value = Player.GetComponent<PlayerController>().health;
        }

        if (Level) {
            Level.text = DG.Level.ToString();
        }

        if (isPaused || isLoosed) {
            DOF.active = true;
        } else {
            DOF.active = false;
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
        isLoosed = false;
        
        SpotLight.SetActive(true);
        MainMenu.SetActive(true);
        LooseMenu.SetActive(false);
        PauseMenu.SetActive(false);
        Overlay.SetActive(false);
    }

    public void OpenSettings() {
        if (isPaused) {
            PauseMenu.SetActive(false);
        } else {
            MainMenu.SetActive(false);
        }
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
        
        var skeletons = GameObject.FindGameObjectsWithTag("Skeleton");
        for (int i = 0; i < skeletons.Length; i++) {
            Destroy(skeletons[i]);
        }

        NextLevelmenu.SetActive(false);
        Overlay.SetActive(true);
        isPaused = false;
        GetComponent<DungeonGenerator>().NewLayer();
    }

    public void NotNewLayer() {
        Cursor.visible = false;
        Time.timeScale = 1;
        
        NextLevelmenu.SetActive(false);
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
        Blur.active = blur;
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
            case "Spawn":
                Controller.clip = Sounds[1];
                Controller.Play();
                break;
            case "Damage":
                Controller.clip = Sounds[2];
                Controller.Play();
                break;
        }
    }
    
    public void SetMasterVolume(float volume) {
        MasterVolume.audioMixer.SetFloat("Master", Mathf.Lerp(-80, 0, volume));
    }
    
    public void SetMusicVolume(float volume) {
        MasterVolume.audioMixer.SetFloat("Music", Mathf.Lerp(-80, 0, volume));
    }

    public void SetSoundVolume(float volume) {
        MasterVolume.audioMixer.SetFloat("Sounds", Mathf.Lerp(-80, 0, volume));
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
    
    // Aphaleon

    public void AphaleonMenus() {
        Cursor.visible = true;
        
        AphaleonMenu.SetActive(true);
        Overlay.SetActive(false);
        isPaused = true;
    }
    
    public void AphaleonMenusClose() {
        Cursor.visible = false;
        
        AphaleonMenu.SetActive(false);
        Overlay.SetActive(true);
        isPaused = false;
    }
}
