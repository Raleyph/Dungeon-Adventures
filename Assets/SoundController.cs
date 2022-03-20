using UnityEngine;

public class SoundController : MonoBehaviour {
    public AudioSource Controller;
    public AudioClip[] Sounds;
    
    /* 0 - Tap
     * 1 - Walk
     * 3 - Sword
     * 4 - Death
     * 5 - Coin Pass
     * 6 - New Level
     * 7 - Explosive
    */

    private void Start() {
        Controller = gameObject.GetComponent<AudioSource>();
    }

    public void Play(string sound) {
        switch (sound) {
            case "Tap":
                Controller.clip = Sounds[0];
                Controller.Play();
                break;
        }
    }
}
