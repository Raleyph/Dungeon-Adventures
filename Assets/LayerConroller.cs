using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerConroller : MonoBehaviour {
    public RoomPlacer DungeonController;

    private void Start()
    {
        DungeonController = RoomPlacer.FindObjectOfType<RoomPlacer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DungeonController.StartRefreshNewLayer();
        }
    }
}
