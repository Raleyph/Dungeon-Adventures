using System;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject FAQmenu;

    [Header("Character Menus")]
    public GameObject Menus;
    public GameObject AdosMenu;
    public GameObject AphaleonMenu;
    public GameObject MichaelMenu;
    
    [Header("Sound Controller")]
    public AudioSource Controller;
    public AudioMixerGroup MasterVolume;
    public AudioClip[] Sounds;

    [Header("Settings & Etc")]
    public Slider HealthBar;
    public Slider ArmorBar;
    public Text Level;
    public Text RoomName;
    public Text Coins;
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider VolumeSlider;
    public Dropdown QualityDropdown;
    public Dropdown ResolutionDropdown;
    public Dropdown AADropdown;
    public PostProcessVolume PostProcess;
    public Light SpotLight;
    public GameObject Camera;

    Resolution[] resolutions;

    private DungeonGenerator DG;
    
    private MotionBlur Blur;
    private DepthOfField DOF;
    
    private int[] aliasing = new int[]{0, 2, 4, 8};

    private bool isStarted = false;
    private bool isPaused = false;
    public bool isLoosed = false;

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
        
        PlayerPrefs.DeleteKey("Health");
        PlayerPrefs.DeleteKey("Armor");
        PlayerPrefs.DeleteKey("Coins");
        BuildSettings();
    }

    public void BuildSettings() {
        ResolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + 
                            resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width 
                && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        
        for(int i = 0; i < aliasing.Length; i++) {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = (aliasing[i] == 0) ? "Off" : aliasing[i] + "x Multi Sampling";
            AADropdown.options.Add(option);
            if(aliasing[i] == QualitySettings.antiAliasing) AADropdown.value = i;
        }
        
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.RefreshShownValue();
        
        LoadSettings(currentResolutionIndex);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!isPaused && isStarted && !isLoosed) {
                Pause();
            } else if (isPaused && isStarted) {
                Resume();
            } else if ((AdosMenu || AphaleonMenu || MichaelMenu) && isPaused) {
                AdosMenusClose();
                AphaleonMenusClose();
            }
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            if (MainMenu) {
                Play();
            }
        }

        if (Overlay) {
            HealthBar.value = PlayerPrefs.GetFloat("Health");
            ArmorBar.value = PlayerPrefs.GetFloat("Armor");
            Coins.text = PlayerPrefs.GetInt("Coins").ToString();
            Level.text = DG.Level.ToString();
        }

        if (isPaused || isLoosed) {
            DOF.active = true;
        } else if ((SettingsMenu.activeSelf || FAQmenu.activeSelf) && !isPaused) {
            DOF.active = true;
        } else {
            DOF.active = false;
        }
    }

    // Main Menu
    
    public void Play() {
        Cursor.visible = false;
        Time.timeScale = 1;

        SpotLight.enabled = false;
        MainMenu.SetActive(false);
        Overlay.SetActive(true);
        isStarted = true;
        GetComponent<Inventory>().InitInventory(true);
        Camera.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
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
        PlayerPrefs.DeleteKey("Health");
        PlayerPrefs.DeleteKey("Coins");
        
        // destroy rooms
        var rooms = GameObject.FindGameObjectsWithTag("Room");
        for (int i = 0; i < rooms.Length; i++) {
            Destroy(rooms[i]);
        }

        // destroy mobs
        var skeletons = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < skeletons.Length; i++) {
            Destroy(skeletons[i]);
        }
        
        // destroy chests
        var chests = GameObject.FindGameObjectsWithTag("Chest");
        for (int i = 0; i < chests.Length; i++) {
            Destroy(chests[i]);
        }
        
        GetComponent<Inventory>().ClearInventory();
        GetComponent<DungeonGenerator>().AllRefreshGenerate();
        Camera.transform.position = new Vector3(3f, 1.5f, 3f);
        Camera.transform.rotation = Quaternion.Euler(0f, -135f, 0f);

        isStarted = false;
        isPaused = false;
        isLoosed = false;

        SpotLight.enabled = true;
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
            DOF.active = true;
        }
        SettingsMenu.SetActive(true);
    }

    public void CloseSettings() {
        if (isPaused) {
            PauseMenu.SetActive(true);
        } else {
            MainMenu.SetActive(true);
            DOF.active = false;
        }
        SettingsMenu.SetActive(false);
        SaveSettings();
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
        
        var skeletons = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < skeletons.Length; i++) {
            Destroy(skeletons[i]);
        }

        var chests = GameObject.FindGameObjectsWithTag("Chest");
        for (int i = 0; i < chests.Length; i++) {
            Destroy(chests[i]);
        }

        NextLevelmenu.SetActive(false);
        Overlay.SetActive(true);
        isPaused = false;
        GetComponent<DungeonGenerator>().NewLayer();
        GetComponent<Inventory>().InitInventory(false);
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

    public void SetAntiAliasing(int index) {
        index = AADropdown.value;
        QualitySettings.antiAliasing = index;
    }

    public void Vsync(bool vsync) {
        QualitySettings.vSyncCount = Convert.ToInt32(vsync);
    }

    public void SetMotionBlur(bool blur) {
        Blur.active = blur;
    }

    public void SetFullscreen(bool fullscreen) {
        Screen.fullScreen = fullscreen;
    }

    public void SaveSettings() {
        PlayerPrefs.SetInt("QualitySettings", QualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionSettings", ResolutionDropdown.value);
        PlayerPrefs.SetInt("AntiAlisingSettings", AADropdown.value);
        PlayerPrefs.SetInt("Vsnyc", QualitySettings.vSyncCount);
        PlayerPrefs.SetInt("MotionBlurSettings", Convert.ToInt32(Blur.active));
        PlayerPrefs.SetInt("FullscreenSettings", Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void LoadSettings(int resolutionIndex) {
        if (PlayerPrefs.HasKey("QualitySettings")) {
            QualityDropdown.value = PlayerPrefs.GetInt("QualitySettings");
        } else {
            QualityDropdown.value = 0;
        }

        if (PlayerPrefs.HasKey("ResolutionSettings")) {
            ResolutionDropdown.value = PlayerPrefs.GetInt("ResolutionSettings");
        } else {
            ResolutionDropdown.value = resolutionIndex;
        }

        if (PlayerPrefs.HasKey("AntiAlisingSettings")) {
            AADropdown.value = PlayerPrefs.GetInt("AntiAlisingSettings");
        } else {
            AADropdown.value = 0;
        }

        if (PlayerPrefs.HasKey("FullscreenSettings")) {
            Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenSettings"));
        } else {
            Screen.fullScreen = true;
        }

        if (PlayerPrefs.HasKey("Vsync")) {
            QualitySettings.vSyncCount = PlayerPrefs.GetInt("Vsync");
        } else {
            QualitySettings.vSyncCount = 0;
        }

        if (PlayerPrefs.HasKey("MotionBlurSettings")) {
            Blur.active = Convert.ToBoolean(PlayerPrefs.GetInt("MotionBlurSettings"));
        } else {
            Blur.active = true;
        }

        if (PlayerPrefs.HasKey("MasterVolume")) {
            MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        } else {
            MasterSlider.value = 1f;
        }
        
        if (PlayerPrefs.HasKey("SoundVolume")) {
            VolumeSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        } else {
            VolumeSlider.value = 1f;
        }
        
        if (PlayerPrefs.HasKey("MusicVolume")) {
            MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        } else {
            MusicSlider.value = 1f;
        }
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
            case "DeathEnemy":
                Controller.clip = Sounds[3];
                Controller.Play();
                break;
            case "DamageEnemy":
                Controller.clip = Sounds[4];
                Controller.Play();
                break;
            case "Explosion":
                Controller.clip = Sounds[5];
                Controller.Play();
                break;
            case "Coin":
                Controller.clip = Sounds[6];
                Controller.Play();
                break;
            case "PoisonUse":
                Controller.clip = Sounds[7];
                Controller.Play();
                break;
            case "Dash":
                Controller.clip = Sounds[8];
                Controller.Play();
                break;
        }
    }
    
    public void SetMasterVolume(float volume) {
        MasterVolume.audioMixer.SetFloat("Master", Mathf.Lerp(-80, 0, volume));
        masterVolume = volume;
    }
    
    public void SetMusicVolume(float volume) {
        MasterVolume.audioMixer.SetFloat("Music", Mathf.Lerp(-80, 0, volume));
        musicVolume = volume;
    }

    public void SetSoundVolume(float volume) {
        MasterVolume.audioMixer.SetFloat("Sounds", Mathf.Lerp(-80, 0, volume));
        soundVolume = volume;
    }
    
    // Etc

    public void Loose() {
        Cursor.visible = true;

        isLoosed = true;
        isStarted = false;
        Overlay.SetActive(false);
        LooseMenu.SetActive(true);
        GetComponent<Inventory>().ClearInventory();
        PlayerPrefs.DeleteKey("Health");
        PlayerPrefs.DeleteKey("Coins");
        StopAllCoroutines();
    }
    
    public void NameRoom(string room) {
        RoomName.text = room;
    }

    // Ados

    public void AdosMenus() {
        Cursor.visible = true;
        
        Menus.SetActive(true);
        AdosMenu.SetActive(true);
        Overlay.SetActive(false);
        isPaused = true;
    }

    public void AdosMenusClose() {
        Cursor.visible = false;

        Menus.SetActive(false);
        AdosMenu.SetActive(false);
        Overlay.SetActive(true);
        isPaused = false;
    }
    
    // Aphaleon

    public void AphaleonMenus() {
        Cursor.visible = true;
        
        Menus.SetActive(true);
        AphaleonMenu.SetActive(true);
        Overlay.SetActive(false);
        isPaused = true;
    }
    
    public void AphaleonMenusClose() {
        Cursor.visible = false;
        
        Menus.SetActive(false);
        AphaleonMenu.SetActive(false);
        Overlay.SetActive(true);
        isPaused = false;
    }
}
