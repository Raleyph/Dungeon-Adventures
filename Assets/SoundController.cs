using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour {
    public AudioSource Controller;
    public AudioClip[] Sounds;
    
    /* 0 - Tap
     * 1 - Walk
     * 3 - Sword
     * 4 - Death
     * 5 - Coin Pass
     * 6 - New Level
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
