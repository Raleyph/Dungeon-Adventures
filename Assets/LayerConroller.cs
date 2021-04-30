using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LayerConroller : MonoBehaviour {
    public RoomPlacer DungeonController;
    public GameObject Player;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        DungeonController = RoomPlacer.FindObjectOfType<RoomPlacer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(Player);
            DungeonController.StartRefreshNewLayer();
        }
    }
}
